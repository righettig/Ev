using Ev.Common.Core;
using Ev.Common.Core.Interfaces;
using Ev.Domain.Server.Actions.Core;
using Ev.Domain.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ev.Domain.Server.World.Core
{
    public abstract class BaseWorld : IWorld 
    {
        private const int WORLD_STATE_SIZE = WorldState.WORLD_STATE_SIZE;

        public IWorldEntity[,] State => _state;

        public int Size => _size;

        public ITribe[] Tribes => _tribes.ToArray();

        public bool Finished { get; private set; }

        public ITribe Winner { get; private set; }

        protected List<ITribe> _tribes = new();

        protected readonly IWorldEntity[,] _state;
        protected readonly IRandom _rnd;
        protected readonly int _size;

        protected BaseWorld(int size, IRandom rnd)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be a positive value.");

            _size = size;
            _state = new IWorldEntity[size, size];
            _rnd = rnd ?? throw new ArgumentNullException(nameof(rnd));
        }

        /// <summary>
        /// Generates the world state for the given tribe.
        /// </summary>
        /// <remarks>The world state is defined as the 5x5 matrix surrounding the given tribe.</remarks>
        /// <param name="tribe">The tribe which the returned world state is centered on.</param>
        /// <returns>The world state.</returns>
        public IWorldState GetWorldState(ITribe tribe)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            var pos = tribe.Position;

            var result = new IWorldEntity[1 + 2 * WORLD_STATE_SIZE, 1 + 2 * WORLD_STATE_SIZE];
            var notReachable = EntityFactory.NotReachable();

            for (var x = 0; x < result.GetLength(0); x++)
            {
                for (var y = 0; y < result.GetLength(1); y++)
                {
                    result[x, y] = notReachable;
                }
            }

            var ws_y = 0;

            for (var y = pos.y - WORLD_STATE_SIZE; y <= pos.y + WORLD_STATE_SIZE; y++)
            {
                var ws_x = 0;

                for (var x = pos.x - WORLD_STATE_SIZE; x <= pos.x + WORLD_STATE_SIZE; x++)
                {
                    if (x >= 0 && y >= 0 && x < Size && y < Size && State[x, y] != null)
                    {
                        result[ws_x, ws_y] = State[x, y];
                    }

                    ws_x++;
                }

                ws_y++;
            }

            return new WorldState(result);
        }

        /// <summary>
        /// Gets all the alive tribes.
        /// </summary>
        /// <remarks>A tribe is considered alive as long as it has a non-zero population.</remarks>
        /// <returns>An array of tribes.</returns>
        public ITribe[] GetAliveTribes()
        {
            return _tribes.FindAll(t => t.Population > 0).ToArray();
        }

        /// <summary>
        /// Checks whether it's possible to move in the given direction from the specified position.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        /// <returns>True if it's possible to move.</returns>
        public bool CanMove((int x, int y) pos, Direction direction) => direction switch
        {
            Direction.N  => pos.y > 0                            && Available(pos.x,     pos.y - 1),
            Direction.S  => pos.y < Size - 1                     && Available(pos.x,     pos.y + 1),
            Direction.W  => pos.x > 0                            && Available(pos.x - 1, pos.y    ),
            Direction.E  => pos.x < Size - 1                     && Available(pos.x + 1, pos.y    ),
            Direction.NW => pos.x > 0        && pos.y > 0        && Available(pos.x - 1, pos.y - 1),
            Direction.SE => pos.x < Size - 1 && pos.y < Size - 1 && Available(pos.x + 1, pos.y + 1),
            Direction.NE => pos.x < Size - 1 && pos.y > 0        && Available(pos.x + 1, pos.y - 1),
            Direction.SW => pos.x > 0        && pos.y < Size - 1 && Available(pos.x - 1, pos.y + 1),
            _ => false,
        };

        /// <summary>
        /// Updates the tribe's position by moving in the given direction.
        /// </summary>
        /// <param name="tribe"></param>
        /// <param name="direction"></param>
        public void Move(ITribe tribe, Direction direction)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            var (x, y) = tribe.Position;

            tribe.Position = direction switch
            {
                Direction.N  => (tribe.Position.x,     tribe.Position.y - 1),
                Direction.S  => (tribe.Position.x,     tribe.Position.y + 1),
                Direction.W  => (tribe.Position.x - 1, tribe.Position.y),
                Direction.E  => (tribe.Position.x + 1, tribe.Position.y),
                Direction.NW => (tribe.Position.x - 1, tribe.Position.y - 1),
                Direction.SE => (tribe.Position.x + 1, tribe.Position.y + 1),
                Direction.NE => (tribe.Position.x + 1, tribe.Position.y - 1),
                Direction.SW => (tribe.Position.x - 1, tribe.Position.y + 1),

                _ => throw new NotImplementedException()
            };

            State[x, y] = null;

            if (State[tribe.Position.x, tribe.Position.y] is CollectableWorldEntity c)
            {
                switch (c.Type)
                {
                    case CollectableWorldEntityType.Food: tribe.Population += c.Value; break;
                    case CollectableWorldEntityType.Wood: tribe.Wood       += c.Value; break;
                    case CollectableWorldEntityType.Iron: tribe.Iron       += c.Value; break;

                    default:
                        throw new NotImplementedException();
                }
            }

            State[tribe.Position.x, tribe.Position.y] = tribe;
        }

        public abstract void AddTribe(string tribeName, Color color);

        /// <summary>
        /// Updates the world state by having the specified tribe executing the given move.
        /// </summary>
        /// <param name="tribe"></param>
        /// <param name="move"></param>
        /// <param name="iteration"></param>
        /// <param name="actionProcessor"></param>
        /// <returns>True if game has finished after the turn.</returns>
        public bool Update(ITribe tribe, IGameAction move, int iteration, IGameActionProcessor actionProcessor)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            if (move is null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            if (actionProcessor is null)
            {
                throw new ArgumentNullException(nameof(actionProcessor));
            }

            actionProcessor.Update(move, tribe, this, iteration);

            // remove losers
            if (tribe.Population <= 0)
            {
                WipeTribe(tribe, iteration);
            }

            // when only one tribe is left alive OR the only tribe has died
            if (_tribes.Count > 1) 
            {
                if (_tribes.Count(t => t.Population > 0) == 1) 
                {
                    Finished = true;
                    Winner = _tribes.First(t => t.Population > 0);
                }
            }
            else if (_tribes[0].Population <= 0)
            {
                Finished = true;
                Winner = _tribes[0];
            }
            
            _tribes.ForEach(t => t.IsAttacking = false);

            return Finished;
        }

        /// <summary>
        /// Removes the specified from the world state.
        /// </summary>
        /// <param name="tribe"></param>
        /// <param name="iteration"></param>
        public void WipeTribe(ITribe tribe, int iteration)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            State[tribe.Position.x, tribe.Position.y] = null;
            tribe.DeadAtIteration = iteration;
        }

        private bool Available(int x, int y)
        {
            return !(State[x, y] is IBlockingWorldEntity || State[x, y] is ITribe);
        }
    }
}

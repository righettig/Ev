using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Behaviours.Core;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ev.Domain.World.Core
{
    public abstract class BaseWorld : IWorld 
    {
        public IWorldEntity[,] State => _state;

        public int Size => _size;

        public ITribe[] Tribes => _tribes.ToArray();

        public bool Finished { get; protected set; }

        public ITribe Winner { get; protected set; }

        protected List<ITribe> _tribes = new List<ITribe>();

        protected readonly IWorldEntity[,] _state;
        protected readonly IRandom _rnd;
        protected readonly int _size;

        public BaseWorld(int size, IRandom rnd)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be a positive value.");

            _size = size;
            _state = new IWorldEntity[size, size];
            _rnd = rnd ?? throw new ArgumentNullException(nameof(rnd));
        }

        public IWorldState GetWorldState(ITribe tribe)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            var pos = tribe.Position;

            var result = new IWorldEntity[1 + 2 * WorldState.WORLD_STATE_SIZE, 1 + 2 * WorldState.WORLD_STATE_SIZE];

            var ws_y = 0;

            for (var y = pos.y - WorldState.WORLD_STATE_SIZE; y <= pos.y + WorldState.WORLD_STATE_SIZE; y++)
            {
                var ws_x = 0;

                for (var x = pos.x - WorldState.WORLD_STATE_SIZE; x <= pos.x + WorldState.WORLD_STATE_SIZE; x++)
                {
                    if (x >= 0 && y >= 0 && x < Size && y < Size)
                    {
                        result[ws_x, ws_y] = State[x, y];
                    }

                    ws_x++;
                }

                ws_y++;
            }

            return new WorldState(result);
        }

        public ITribe[] GetAliveTribes()
        {
            return _tribes.FindAll(t => t.Population > 0).ToArray();
        }

        bool IWorld.CanMove((int x, int y) position, Direction direction)
        {
            return CanMove(position, direction);
        }

        void IWorld.Move(ITribe tribe, Direction direction)
        {
            Move(tribe, direction);
        }

        #region Internal members

        // TODO: unit test
        internal bool CanMove((int x, int y) pos, Direction direction) => direction switch
        {
            Direction.N => pos.y > 0 && !(State[pos.x, pos.y - 1] is IBlockingEntity),
            Direction.S => pos.y < Size - 1 && !(State[pos.x, pos.y + 1] is IBlockingEntity),
            Direction.W => pos.x > 0 && !(State[pos.x -1, pos.y] is IBlockingEntity),
            Direction.E => pos.x < Size - 1 && !(State[pos.x + 1, pos.y] is IBlockingEntity),
            Direction.NW => pos.x > 0 && pos.y > 0 && !(State[pos.x - 1, pos.y - 1] is IBlockingEntity),
            Direction.SE => pos.x < Size - 1 && pos.y < Size - 1 && !(State[pos.x + 1, pos.y + 1] is IBlockingEntity),
            Direction.NE => pos.x < Size - 1 && pos.y > 0 && !(State[pos.x + 1, pos.y - 1] is IBlockingEntity),
            Direction.SW => pos.x > 0 && pos.y < Size - 1 && !(State[pos.x - 1, pos.y + 1] is IBlockingEntity),
            _ => false,
        };

        // TODO: unit test
        internal void Move(ITribe tribe, Direction direction)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            var (x, y) = tribe.Position;

            switch (direction)
            {
                case Direction.N:
                    tribe.Position = (tribe.Position.x, tribe.Position.y - 1);
                    break;

                case Direction.S:
                    tribe.Position = (tribe.Position.x, tribe.Position.y + 1);
                    break;

                case Direction.W:
                    tribe.Position = (tribe.Position.x - 1, tribe.Position.y);
                    break;

                case Direction.E:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y);
                    break;

                case Direction.NW:
                    tribe.Position = (tribe.Position.x - 1, tribe.Position.y - 1);
                    break;

                case Direction.SE:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y + 1);
                    break;

                case Direction.NE:
                    tribe.Position = (tribe.Position.x + 1, tribe.Position.y - 1);
                    break;

                case Direction.SW:
                    tribe.Position = (tribe.Position.x - 1, tribe.Position.y + 1);
                    break;
            }

            State[x, y] = null;

            switch (State[tribe.Position.x, tribe.Position.y])
            {
                case Food food: tribe.Population += food.Value; break;
                case Wood wood: tribe.Wood += wood.Value; break;
                case Iron iron: tribe.Iron += iron.Value; break;
            }

            State[tribe.Position.x, tribe.Position.y] = tribe;
        }

        #endregion

        public abstract IWorld WithTribe(string tribeName, Color darkYellow, ITribeBehaviour behaviour);

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

            if (_tribes.Count(t => t.Population > 0) == 1) // when only one tribe is left alive
            {
                Finished = true;
                Winner = _tribes.First(t => t.Population > 0);
            }

            _tribes.ForEach(t => t.IsAttacking = false);

            return Finished;
        }

        public void WipeTribe(ITribe tribe, int iteration)
        {
            if (tribe is null)
            {
                throw new ArgumentNullException(nameof(tribe));
            }

            State[tribe.Position.x, tribe.Position.y] = null;
            tribe.DeadAtIteration = iteration;
        }
    }
}

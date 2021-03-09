using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Blocking;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using Ev.Domain.Utils;
using Ev.Domain.World.Core;
using Ev.Tribe;
using System;
using System.Collections.Generic;
using System.Linq;
using static Ev.Tribe.DoMoveReply.Types;
using static Ev.Tribe.DoMoveRequest.Types;
using Action = Ev.Tribe.DoMoveReply.Types.Action;
using WorldState = Ev.Tribe.DoMoveRequest.Types.WorldState;

namespace Ev.Agents.Core
{
    public class GameStateSerialiser
    {
        private const int WORLD_STATE_SIZE = Domain.World.WorldState.WORLD_STATE_SIZE;

        // used by game master
        public DoMoveRequest CreateDoMoveRequest(IWorldState state, ITribe tribe)
        {
            var msg = new DoMoveRequest
            {
                Busy = tribe.BusyDoing != null,

                TribeState = new DoMoveRequest.Types.TribeState
                {
                    Population   = tribe.Population,
                    Position     = new Position { X = tribe.Position.x,     Y = tribe.Position.y     },
                    PrevPosition = new Position { X = tribe.PrevPosition.x, Y = tribe.PrevPosition.y },
                    Iron         = tribe.Iron,
                    Wood         = tribe.Wood,
                    //Defense      = tribe.Defense
                    //Attack       = tribe.Attack
                }
            };

            state.Traverse((e, x, y) =>
            {
                var worldState = e switch
                {
                    null => new WorldState { Null = Google.Protobuf.WellKnownTypes.NullValue.NullValue },

                    IBlockingWorldEntity    blocking    => new WorldState { BlockingEntityType = blocking.Type.ToString() },

                    ICollectableWorldEntity collectable => new WorldState
                    {
                        Collectable = new Collectable
                        {
                            Type  = collectable.Type.ToString(),
                            Value = collectable.Value
                        }
                    },

                    ITribeState t => new WorldState
                    {
                        Tribe = new EnemyTribe
                        {
                            Name       = t.Name,
                            Population = t.Population,
                            Position   = new Position { X = t.Position.x, Y = t.Position.y }
                        }
                    },

                    _ => throw new ArgumentException("Unsupported world entity found.", nameof(state))
                };

                msg.WorldState.Add(worldState);

            }, ignoreSelf: false);

            return msg;
        }

        public IGameAction Deserialise(DoMoveReply reply, ITribe tribe, IEnumerable<ITribe> alive)
        {
            IGameAction move = null;

            if (reply.KindCase == DoMoveReply.KindOneofCase.Action)
            {
                move = reply.Action.Type switch
                {
                    actions.Move           => new MoveAction(Enum.Parse<Direction>(reply.Action.Direction.ToString(), true)),
                    actions.Attack         => new AttackAction(alive.First(el => el.Name == reply.Action.Enemy).Name) { Tribe = tribe },
                    actions.Suicide        => new SuicideAction(),
                    actions.UpgradeAttack  => new UpgradeAttackAction(),
                    actions.UpgradeDefense => new UpgradeDefensesAction(),
                    actions.Hold           => new HoldAction(),

                    _ => throw new ArgumentException("Unsupported game action found.", nameof(reply.Action.Type)),
                };
            }

            return move;
        }

        // used by game master when talking to spectator
        // serialiseEntireWorld

        // used by tribe
        public DoMoveReply CreateDoMoveReply(IGameAction move)
        {
            var action = move switch
            {
                HoldAction            => new Action { Type = actions.Hold },
                SuicideAction         => new Action { Type = actions.Suicide },
                UpgradeAttackAction   => new Action { Type = actions.UpgradeAttack },
                UpgradeDefensesAction => new Action { Type = actions.UpgradeDefense },
                MoveAction a          => new Action { Type = actions.Move, Direction = Enum.Parse<direction>(a.Direction.ToString(), true) },
                AttackAction a        => new Action { Type = actions.Attack, Enemy = a.Target.Name },

                _ => throw new ArgumentException("Unsupported game action found.", nameof(move)),
            };

            return new DoMoveReply { Action = action };
        }

        public IWorldState DeserialiseWorldState(DoMoveRequest request, Color _color)
        {
            var worldStateEntities = new IWorldEntity[1 + 2 * WORLD_STATE_SIZE, 1 + 2 * WORLD_STATE_SIZE];

            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    var i = (5 * x) + y;

                    worldStateEntities[x, y] = request.WorldState[i].KindCase switch
                    {
                        WorldState.KindOneofCase.BlockingEntityType => new BlockingWorldEntity(request.WorldState[i].BlockingEntityType),

                        WorldState.KindOneofCase.Collectable        => new CollectableWorldEntity(request.WorldState[i].Collectable.Type,
                                                                                                  request.WorldState[i].Collectable.Value),

                        WorldState.KindOneofCase.Tribe =>
                            // TODO: can I use TribeState in here?!
                            new Domain.Entities.Tribe(
                                request.WorldState[i].Tribe.Name,
                                (request.WorldState[i].Tribe.Position.X, request.WorldState[i].Tribe.Position.Y),
                                _color)
                            {
                                Population = request.WorldState[i].Tribe.Population
                            },

                        WorldState.KindOneofCase.Null => null,

                        _ => throw new NotImplementedException()
                    };
                }
            }

            var worldState = new Domain.World.WorldState(worldStateEntities);

            return worldState;
        }

        // used by spectator when receiving message from game master
        // (_world, iteration, move, next) deserialize
    }
}
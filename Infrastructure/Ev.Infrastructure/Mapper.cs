using Ev.Domain.Client.Actions;
using Ev.Domain.Client.Core;
using Ev.Domain.Server.Entities;
using Ev.Domain.Server.Entities.Collectables;
using System;

namespace Ev.Infrastructure
{
    public class Mapper
    {
        public Domain.Client.Core.IWorldState Map(Domain.Server.Core.IWorldState worldState)
        {
            const int size = Domain.Server.World.WorldState.WORLD_STATE_SIZE * 2 + 1;
            
            var entities = new IWorldEntity[size, size];

            worldState.Traverse((entity, x, y) =>
            {
                entities[x, y] = entity switch
                {
                    Domain.Server.Core.ITribe tribe => Map(tribe),
                    Food food   => new Domain.Client.Entities.Collectables.Food { Value = food.Value },
                    Iron iron   => new Domain.Client.Entities.Collectables.Iron { Value = iron.Value },
                    Wood wood   => new Domain.Client.Entities.Collectables.Wood { Value = wood.Value },
                    Wall        => new Domain.Client.Entities.Wall(),
                    Water       => new Domain.Client.Entities.Water(),
                    null        => null,

                    _ => throw new NotSupportedException()
                };

            }, ignoreSelf: false);

            var result = new Domain.Client.World.WorldState(entities);

            return result;
        }

        public Domain.Client.Core.ITribe Map(Domain.Server.Core.ITribe tribe)
        {
            var result = new Domain.Client.Entities.Tribe
            {
                Name         = tribe.Name,
                Color        = tribe.Color,
                Population   = tribe.Population,
                Position     = tribe.Position,
                PrevPosition = tribe.PrevPosition,
                Iron         = tribe.Iron,
                Wood         = tribe.Wood,
            };

            return result;
        }

        public Domain.Server.Core.IGameAction Map(Domain.Client.Core.IGameAction action)
        {
            Domain.Server.Core.IGameAction result = action switch
            {
                AttackAction a          => new Domain.Server.Actions.AttackAction(a.TargetName),
                HoldAction              => new Domain.Server.Actions.HoldAction(),
                MoveAction a            => new Domain.Server.Actions.MoveAction(a.Direction),
                SuicideAction           => new Domain.Server.Actions.SuicideAction(),
                UpgradeAttackAction     => new Domain.Server.Actions.UpgradeAttackAction(),
                UpgradeDefensesAction   => new Domain.Server.Actions.UpgradeDefensesAction(),

                _ => throw new NotSupportedException()
            };

            return result;
        }
    }
}
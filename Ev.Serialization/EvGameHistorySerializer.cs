using AutoMapper;
using Ev.Domain.Actions;
using Ev.Domain.Actions.Core;
using Ev.Domain.Entities.Collectables;
using Ev.Domain.Entities.Core;
using Ev.Domain.World.Core;
using Ev.Serialization.Dto.Actions;
using Ev.Serialization.Dto.Actions.Core;
using Ev.Serialization.Dto.Entities;
using Ev.Serialization.Dto.Entities.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ev.Serialization
{
    public class EvGameHistorySerializer
    {
        private readonly IMapper _mapper;

        public EvGameHistorySerializer()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ITribe,                TribeDto>();
                cfg.CreateMap<ITribe,                EnemyTribeDto>();

                cfg.CreateMap<Food,                  CollectableWorldEntityDto>().AfterMap((_, result) => result.EntityType = "Food");
                cfg.CreateMap<Iron,                  CollectableWorldEntityDto>().AfterMap((_, result) => result.EntityType = "Iron");
                cfg.CreateMap<Wood,                  CollectableWorldEntityDto>().AfterMap((_, result) => result.EntityType = "Wood");

                cfg.CreateMap<Wall,                  BlockingWorldEntityDto>().AfterMap((_, result) => result.EntityType = "Wall");
                cfg.CreateMap<Water,                 BlockingWorldEntityDto>().AfterMap((_, result) => result.EntityType = "Water");

                cfg.CreateMap<MoveAction,            MoveActionDto>();
                cfg.CreateMap<HoldAction,            HoldActionDto>();
                cfg.CreateMap<AttackAction,          AttackActionDto>();
                cfg.CreateMap<SuicideAction,         SuicideActionDto>();
                cfg.CreateMap<UpgradeDefensesAction, UpgradeDefensesActionDto>();
            });

            _mapper = config.CreateMapper();
        }

        public async Task SaveToFile(IEnumerable<(IGameAction, IWorldState)> winnerHistory, string filename)
        {
            var result = new List<GameState>();

            foreach (var item in winnerHistory)
            {
                IGameActionDto actionDto = null;

                actionDto = item.Item1 switch
                {
                    MoveAction a            => _mapper.Map<MoveActionDto>(a),
                    HoldAction a            => _mapper.Map<HoldActionDto>(a),
                    AttackAction a          => _mapper.Map<AttackActionDto>(a),
                    SuicideAction a         => _mapper.Map<SuicideActionDto>(a),
                    UpgradeDefensesAction a => _mapper.Map<UpgradeDefensesActionDto>(a),
                    _ => throw new ArgumentException("Unknown type: " + item.Item1.GetType()),
                };
                var gameState = new GameState { Action = actionDto };

                item.Item2.Traverse((entity, x, y) =>
                {
                    IWorldEntityDto worldEntityDto = null;

                    switch (entity)
                    {
                        case CollectableWorldEntity:
                            worldEntityDto = _mapper.Map<CollectableWorldEntityDto>(entity).WithPosition(x, y);
                            break;

                        case IBlockingWorldEntity:
                            worldEntityDto = _mapper.Map<BlockingWorldEntityDto>(entity).WithPosition(x, y);
                            break;

                        case ITribe e when e.Name != item.Item1.Tribe.Name:
                            worldEntityDto = _mapper.Map<EnemyTribeDto>(entity).WithPosition(x, y);
                            break;
                    }

                    if (worldEntityDto != null)
                    {
                        gameState.State.Add(worldEntityDto);
                    }
                });

                result.Add(gameState);
            }

            string json = JsonConvert.SerializeObject(
                result, 
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Converters = new List<JsonConverter>() 
                    { 
                        new BlockingWorldEntityDtoConverter(),
                        new CollectableWorldEntityDtoConverter(), 
                        new EnemyTribeDtoConverter(),
                        new AttackActionDtoConverter(),
                        new HoldActionDtoConverter(),
                        new SuicideActionDtoConverter(),
                        new UpgradeDefensesActionDtoConverter(),
                        new MoveActionDtoConverter()
                    }
                });

            await File.WriteAllTextAsync($"{filename}-{Guid.NewGuid()}.json", json);
        }
    }
}

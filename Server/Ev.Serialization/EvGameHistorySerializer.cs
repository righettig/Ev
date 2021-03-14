﻿using AutoMapper;
using Ev.Domain.Server.Actions;
using Ev.Domain.Server.Core;
using Ev.Domain.Server.Entities;
using Ev.Domain.Server.Entities.Collectables;
using Ev.Domain.Server.Entities.Core;
using Ev.Serialization.Dto.Actions;
using Ev.Serialization.Dto.Actions.Core;
using Ev.Serialization.Dto.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ev.Common.Core;

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
                cfg.CreateMap<UpgradeAttackAction,   UpgradeAttackActionDto>();
            });

            _mapper = config.CreateMapper();
        }

        public async Task SaveToFile(IEnumerable<(IGameAction, IWorldState)> winnerHistory, string filename)
        {
            var result = new List<GameState>();

            foreach (var (gameAction, worldState) in winnerHistory)
            {
                IGameActionDto actionDto = gameAction switch
                {
                    MoveAction a            => _mapper.Map<MoveActionDto>(a),
                    HoldAction a            => _mapper.Map<HoldActionDto>(a),
                    AttackAction a          => _mapper.Map<AttackActionDto>(a),
                    SuicideAction a         => _mapper.Map<SuicideActionDto>(a),
                    UpgradeDefensesAction a => _mapper.Map<UpgradeDefensesActionDto>(a),
                    UpgradeAttackAction   a => _mapper.Map<UpgradeAttackActionDto>(a),

                    _ => throw new ArgumentException("Unknown type: " + gameAction.GetType()),
                };
                var gameState = new GameState { Action = actionDto };

                worldState.Traverse((entity, x, y) =>
                {
                    var worldEntityDto = entity switch
                    {
                        CollectableWorldEntity                             => _mapper.Map<CollectableWorldEntityDto>(entity).WithPosition(x, y),
                        IBlockingWorldEntity                               => _mapper.Map<BlockingWorldEntityDto>(entity).WithPosition(x, y),
                        ITribe e when e.Name != gameAction.Tribe.Name      => _mapper.Map<EnemyTribeDto>(entity).WithPosition(x, y),

                        _ => null
                    };

                    if (worldEntityDto != null)
                    {
                        gameState.State.Add(worldEntityDto);
                    }
                });

                result.Add(gameState);
            }

            var json = JsonConvert.SerializeObject(
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
                        new UpgradeAttackActionDtoConverter(),
                        new MoveActionDtoConverter()
                    }
                });

            var fullFileName = filename ?? $"ev_winner_history-{Guid.NewGuid()}.json";
                
            await File.WriteAllTextAsync(fullFileName, json);
        }
    }
}

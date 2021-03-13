using Ev.Serialization.Dto.Entities.Core;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Entities
{
    public class EnemyTribeDto : TribeDto
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IWorldEntityDto WithPosition(int x, int y)
        {
            X = x;
            Y = y;

            return this;
        }
    }

    public class EnemyTribeDtoConverter : JsonConverter<EnemyTribeDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       EnemyTribeDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Value");
            writer.WriteValue($"Enemy,{value.Name},{value.Population},{value.X},{value.Y}");

            writer.WriteEndObject();
        }

        public override EnemyTribeDto ReadJson(JsonReader reader,
                                               Type objectType,
                                               EnemyTribeDto existingValue,
                                               bool hasExistingValue,
                                               JsonSerializer serializer) => null;
    }
}
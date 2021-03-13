using Ev.Domain.Server.Actions;
using Ev.Serialization.Dto.Actions.Core;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Actions
{
    public class MoveActionDto : GameActionDto
    {
        public Direction Direction { get; set; }
    }

    public class MoveActionDtoConverter : JsonConverter<MoveActionDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       MoveActionDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Type");
            writer.WriteValue("Move");

            writer.WritePropertyName("Direction");
            writer.WriteValue(value.Direction);

            writer.WritePropertyName("Population");
            serializer.Serialize(writer, value.Tribe.Population);

            writer.WriteEndObject();
        }

        public override MoveActionDto ReadJson(JsonReader reader,
                                               Type objectType,
                                               MoveActionDto existingValue,
                                               bool hasExistingValue,
                                               JsonSerializer serializer) => null;
    }
}

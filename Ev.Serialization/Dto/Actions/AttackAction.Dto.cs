using Ev.Serialization.Dto.Actions.Core;
using Ev.Serialization.Dto.Entities;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Actions
{
    public class AttackActionDto : GameActionDto
    {
        public TribeDto Target { get; set; }
    }

    public class AttackActionDtoConverter : JsonConverter<AttackActionDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       AttackActionDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Type");
            writer.WriteValue("Attack");

            writer.WritePropertyName("Population");
            serializer.Serialize(writer, value.Tribe.Population);

            writer.WritePropertyName("Target");
            //serializer.Serialize(writer, value.Target);
            writer.WriteValue($"{value.Target.Name},{value.Target.Population}");

            writer.WriteEndObject();
        }

        public override AttackActionDto ReadJson(JsonReader reader,
                                                 Type objectType,
                                                 AttackActionDto existingValue,
                                                 bool hasExistingValue,
                                                 JsonSerializer serializer) => null;
    }
}

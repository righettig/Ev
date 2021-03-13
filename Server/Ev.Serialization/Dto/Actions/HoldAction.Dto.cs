using Ev.Serialization.Dto.Actions.Core;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Actions
{
    public class HoldActionDto : GameActionDto
    {
    }

    public class HoldActionDtoConverter : JsonConverter<HoldActionDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       HoldActionDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Type");
            writer.WriteValue("Hold");

            writer.WritePropertyName("Population");
            serializer.Serialize(writer, value.Tribe.Population);

            writer.WriteEndObject();
        }

        public override HoldActionDto ReadJson(JsonReader reader,
                                               Type objectType,
                                               HoldActionDto existingValue,
                                               bool hasExistingValue,
                                               JsonSerializer serializer) => null;
    }
}

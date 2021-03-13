using Ev.Serialization.Dto.Actions.Core;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Actions
{
    public class SuicideActionDto : GameActionDto
    {
    }

    public class SuicideActionDtoConverter : JsonConverter<SuicideActionDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       SuicideActionDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Type");
            writer.WriteValue("Suicide");

            writer.WritePropertyName("Population");
            serializer.Serialize(writer, value.Tribe.Population);

            writer.WriteEndObject();
        }

        public override SuicideActionDto ReadJson(JsonReader reader,
                                                  Type objectType,
                                                  SuicideActionDto existingValue,
                                                  bool hasExistingValue,
                                                  JsonSerializer serializer) => null;
    }
}

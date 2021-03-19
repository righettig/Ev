using Ev.Serialization.Dto.Entities.Core;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Entities
{
    public class BlockingWorldEntityDto : BaseWorldEntityDto
    {
    }

    public class BlockingWorldEntityDtoConverter : JsonConverter<BlockingWorldEntityDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       BlockingWorldEntityDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Value");
            writer.WriteValue($"Blocking,{value.EntityType},{value.X},{value.Y}");

            writer.WriteEndObject();
        }

        public override BlockingWorldEntityDto ReadJson(JsonReader reader,
                                                        Type objectType,
                                                        BlockingWorldEntityDto existingValue,
                                                        bool hasExistingValue,
                                                        JsonSerializer serializer) => null;
    }
}

using Ev.Serialization.Dto.Entities.Core;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Entities
{
    public class CollectableWorldEntityDto : BaseWorldEntityDto
    {
        public int Value { get; set; }
    }

    public class CollectableWorldEntityDtoConverter : JsonConverter<CollectableWorldEntityDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       CollectableWorldEntityDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            //writer.WritePropertyName("Type");
            //writer.WriteValue("CollectableWorldEntity");

            //writer.WritePropertyName("EntityType"); 
            //writer.WriteValue(value.EntityType);

            //writer.WritePropertyName("Value");
            //writer.WriteValue(value.Value);

            //writer.WritePropertyName("X");
            //writer.WriteValue(value.X);

            //writer.WritePropertyName("Y");
            //writer.WriteValue(value.Y);

            writer.WritePropertyName("Value");
            writer.WriteValue($"Collectable,{value.EntityType},{value.Value},{value.X},{value.Y}");

            writer.WriteEndObject();
        }

        public override CollectableWorldEntityDto ReadJson(JsonReader reader,
                                                           Type objectType,
                                                           CollectableWorldEntityDto existingValue,
                                                           bool hasExistingValue,
                                                           JsonSerializer serializer) => null;
    }
}

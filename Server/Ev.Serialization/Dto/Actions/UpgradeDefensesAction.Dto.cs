using Ev.Serialization.Dto.Actions.Core;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Actions
{
    public class UpgradeDefensesActionDto : GameActionDto
    {
    }

    public class UpgradeDefensesActionDtoConverter : JsonConverter<UpgradeDefensesActionDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       UpgradeDefensesActionDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Type");
            writer.WriteValue("UpgradeDefenses");

            writer.WritePropertyName("Population");
            serializer.Serialize(writer, value.Tribe.Population);

            writer.WriteEndObject();
        }

        public override UpgradeDefensesActionDto ReadJson(JsonReader reader,
                                                          Type objectType,
                                                          UpgradeDefensesActionDto existingValue,
                                                          bool hasExistingValue,
                                                          JsonSerializer serializer) => null;
    }
}

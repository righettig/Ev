using Ev.Serialization.Dto.Actions.Core;
using Newtonsoft.Json;
using System;

namespace Ev.Serialization.Dto.Actions
{
    public class UpgradeAttackActionDto : GameActionDto
    {
    }

    public class UpgradeAttackActionDtoConverter : JsonConverter<UpgradeAttackActionDto>
    {
        public override void WriteJson(JsonWriter writer,
                                       UpgradeAttackActionDto value,
                                       JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Type");
            writer.WriteValue("UpgradeAttack");

            writer.WritePropertyName("Population");
            serializer.Serialize(writer, value.Tribe.Population);

            writer.WriteEndObject();
        }

        public override UpgradeAttackActionDto ReadJson(JsonReader reader,
                                                        Type objectType,
                                                        UpgradeAttackActionDto existingValue,
                                                        bool hasExistingValue,
                                                        JsonSerializer serializer) => null;
    }
}

using Ev.Common.Core;
using Ev.Serialization.Dto.Actions;
using Ev.Serialization.Dto.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Ev.Serialization.Tests
{
    [TestClass]
    public class SerializationTests
    {
        #region Actions

        [TestMethod]
        public void AttackActionDtoConverter_Should_Serialize_AttackActionDto()
        {
            // Arrange
            var sb              = new StringBuilder();
            var writer          = new JsonTextWriter(new StringWriter(sb));
            var uat             = new AttackActionDtoConverter();
            var attackActionDto = new AttackActionDto
            {
                Target = new EnemyTribeDto {Name = "enemy", Population = 321, X = 1, Y = 2},
                Tribe  = new TribeDto      {Name = "tribe", Population = 123}
            };

            // Act
            uat.WriteJson(writer, attackActionDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Type\":\"Attack\",\"Population\":123,\"Target\":\"enemy,321\"}", sb.ToString());
        }

        [TestMethod]
        public void HoldActionDtoConverter_Should_Serialize_HoldActionDto()
        {
            // Arrange
            var sb            = new StringBuilder();
            var writer        = new JsonTextWriter(new StringWriter(sb));
            var uat           = new HoldActionDtoConverter();
            var holdActionDto = new HoldActionDto {Tribe = new TribeDto {Name = "tribe", Population = 123}};

            // Act
            uat.WriteJson(writer, holdActionDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Type\":\"Hold\",\"Population\":123}", sb.ToString());
        }

        [TestMethod]
        public void MoveActionDtoConverter_Should_Serialize_MoveActionDto()
        {
            // Arrange
            var sb            = new StringBuilder();
            var writer        = new JsonTextWriter(new StringWriter(sb));
            var uat           = new MoveActionDtoConverter();
            var moveActionDto = new MoveActionDto
            {
                Direction = Direction.NW,
                Tribe     = new TribeDto { Name = "tribe", Population = 123 }
            };

            // Act
            uat.WriteJson(writer, moveActionDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Type\":\"Move\",\"Direction\":5,\"Population\":123}", sb.ToString());
        }

        [TestMethod]
        public void SuicideActionDtoConverter_Should_Serialize_SuicideActionDto()
        {
            // Arrange
            var sb               = new StringBuilder();
            var writer           = new JsonTextWriter(new StringWriter(sb));
            var uat              = new SuicideActionDtoConverter();
            var suicideActionDto = new SuicideActionDto {Tribe = new TribeDto {Name = "tribe", Population = 123}};

            // Act
            uat.WriteJson(writer, suicideActionDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Type\":\"Suicide\",\"Population\":123}", sb.ToString());
        }

        [TestMethod]
        public void UpgradeAttackActionDtoConverter_Should_Serialize_UpgradeAttackActionDto()
        {
            // Arrange
            var sb                     = new StringBuilder();
            var writer                 = new JsonTextWriter(new StringWriter(sb));
            var uat                    = new UpgradeAttackActionDtoConverter();
            var upgradeAttackActionDto = new UpgradeAttackActionDto {Tribe = new TribeDto {Name = "tribe", Population = 123}};

            // Act
            uat.WriteJson(writer, upgradeAttackActionDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Type\":\"UpgradeAttack\",\"Population\":123}", sb.ToString());
        }

        [TestMethod]
        public void UpgradeDefenseActionDtoConverter_Should_Serialize_UpgradeDefenseActionDto()
        {
            // Arrange
            var sb                       = new StringBuilder();
            var writer                   = new JsonTextWriter(new StringWriter(sb));
            var uat                      = new UpgradeDefensesActionDtoConverter();
            var upgradeDefensesActionDto = new UpgradeDefensesActionDto {Tribe = new TribeDto { Name = "tribe", Population = 123 }};

            // Act
            uat.WriteJson(writer, upgradeDefensesActionDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Type\":\"UpgradeDefenses\",\"Population\":123}", sb.ToString());
        }

        #endregion

        #region Entities

        [TestMethod]
        public void BlockingWorldEntityDtoConverter_Should_Serialize_BlockingWorldEntityDto()
        {
            // Arrange
            var sb                     = new StringBuilder();
            var writer                 = new JsonTextWriter(new StringWriter(sb));
            var uat                    = new BlockingWorldEntityDtoConverter();
            var blockingWorldEntityDto = new BlockingWorldEntityDto {EntityType = "Wall"};
            blockingWorldEntityDto.WithPosition(1, 2);

            // Act
            uat.WriteJson(writer, blockingWorldEntityDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Value\":\"Blocking,Wall,1,2\"}", sb.ToString());
        }

        [TestMethod]
        public void CollectableWorldEntityDtoConverter_Should_Serialize_CollectableWorldEntityDto()
        {
            // Arrange
            var sb                        = new StringBuilder();
            var writer                    = new JsonTextWriter(new StringWriter(sb));
            var uat                       = new CollectableWorldEntityDtoConverter();
            var collectableWorldEntityDto = new CollectableWorldEntityDto {EntityType = "Food", Value = 123};
            collectableWorldEntityDto.WithPosition(1, 2);

            // Act
            uat.WriteJson(writer, collectableWorldEntityDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Value\":\"Collectable,Food,123,1,2\"}", sb.ToString());
        }

        [TestMethod]
        public void EnemyTribeDtoConverter_Should_Serialize_EnemyTribeDto()
        {
            // Arrange
            var sb            = new StringBuilder();
            var writer        = new JsonTextWriter(new StringWriter(sb));
            var uat           = new EnemyTribeDtoConverter();
            var enemyTribeDto = new EnemyTribeDto {Name = "enemy", Population = 123, X = 1, Y = 2};

            // Act
            uat.WriteJson(writer, enemyTribeDto, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("{\"Value\":\"Enemy,enemy,123,1,2\"}", sb.ToString());
        }

        #endregion
    }
}

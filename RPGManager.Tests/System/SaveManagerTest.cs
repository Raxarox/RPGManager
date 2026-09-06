using RPGManager.GameData.Campaigns;
using RPGManager.GameData.Characters;
using RPGManager.GameData.Items;
using RPGManager.System;

namespace RPGManager.Tests.System
{
    public class SaveManagerTests : IDisposable
    {
        private readonly CharacterClass _testClass;
        private readonly GameAssetRegistry _testRegistry;

        private const string TestSaveName = "test_campaign_temp";

        public SaveManagerTests()
        {
            _testClass = new CharacterClass("Fighter", "Fighter", 10, [], [], [], [], 0);
            _testRegistry = new GameAssetRegistry(
                new Dictionary<string, CharacterClass>
                {
                    { "Fighter", _testClass }
                },
                new Dictionary<string, Item>(),
                new Dictionary<string, Archetype>()
            );
        }

        public void Dispose()
        {
            var path = Path.Combine("Saved Campaigns", TestSaveName, $"campaign_{TestSaveName}.json");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void SaveAndLoad_ValidCampaign_RestoresIdenticalData()
        {
            var campaign = new Campaign();
            Character.TryCreate("Cedric", _testClass, 20, out var character, out _);
            campaign.AddCharacter(character!);
            SaveManager.Save(campaign, TestSaveName);
            var loadedCampaign = SaveManager.Load(TestSaveName, _testRegistry);
            Assert.NotNull(loadedCampaign);
            // After loading, we need to check if the character data matches
            Assert.Single(loadedCampaign.Characters);
            Assert.Equal("Cedric", loadedCampaign.Characters[0].Name);
            Assert.Single(loadedCampaign.Characters[0].Classes);
            Assert.Equal("Fighter", loadedCampaign.Characters[0].Classes[0].ClassAsset?.Name);
        }

        [Fact]
        public void SaveExists_WhenFileExists_ReturnsTrue()
        {
            var campaign = new Campaign();
            SaveManager.Save(campaign, TestSaveName);
            Assert.True(SaveManager.SaveExists(TestSaveName));
        }

        [Fact]
        public void Load_NonExistentFile_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => SaveManager.Load("non_existent_save_file", _testRegistry));
        }
    }
}
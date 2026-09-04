using RPGManager;
using RPGManager.CharacterClasses;

namespace RPGManager.Tests
{
    public class SaveManagerTests : IDisposable
    {
        private const string TestSaveName = "test_campaign_temp";

        public void Dispose()
        {
            var path = Path.Combine("Saves", TestSaveName + ".json");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void SaveAndLoad_ValidCampaign_RestoresIdenticalData()
        {
            var campaign = new Campaign();
            Character.TryCreate("Cedric", new Fighter(), 20, out var character, out _);
            campaign.AddCharacter(character!);
            SaveManager.Save(campaign, TestSaveName);
            var loadedCampaign = SaveManager.Load(TestSaveName);
            Assert.NotNull(loadedCampaign);
            // After loading, we need to check if the character data matches
            Assert.Single(loadedCampaign.Characters);
            Assert.Equal("Cedric", loadedCampaign.Characters[0].Name);
            Assert.Equal("Fighter", loadedCampaign.Characters[0].Class.Name);
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
            Assert.Throws<InvalidOperationException>(() => SaveManager.Load("non_existent_save_file"));
        }
    }
}
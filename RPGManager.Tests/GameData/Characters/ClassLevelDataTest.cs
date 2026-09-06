using RPGManager.GameData.Characters;

namespace RPGManager.Tests.GameData.Characters;

public class ClassLevelDataTest
{
    [Fact]
    public void Constructor_ValidParameters_CreatesClassLevelData()
    {
        var levelData = new ClassLevelData(
            1,
            2,
            ["Fighting Style", "Second Wind"],
            [0, 0, 0, 0, 0, 0, 0, 0, 0],
            new Dictionary<string, int> { { "Action Surge", 1 } });

        Assert.Equal(1, levelData.Level);
        Assert.Equal(2, levelData.ProficiencyBonus);
        Assert.Equal(2, levelData.ClassFeatures.Count);
        Assert.Contains("Fighting Style", levelData.ClassFeatures);
        Assert.Equal(9, levelData.SpellSlots.Count);
        Assert.Single(levelData.ResourcePools);
        Assert.True(levelData.ResourcePools.ContainsKey("Action Surge"));
    }

    [Fact]
    public void Constructor_NullLists_DefaultsToEmptyLists()
    {
        var levelData = new ClassLevelData(1, 2, null, null, null);

        Assert.Empty(levelData.ClassFeatures);
        Assert.Equal(9, levelData.SpellSlots.Count);
        Assert.All(levelData.SpellSlots, slot => Assert.Equal(0, slot));
        Assert.Empty(levelData.ResourcePools);
    }

    [Fact]
    public void Constructor_DefaultSpellSlots_AllZeros()
    {
        var levelData = new ClassLevelData(1, 2, [], null, []);

        Assert.Equal(9, levelData.SpellSlots.Count);
        Assert.All(levelData.SpellSlots, slot => Assert.Equal(0, slot));
    }

    [Fact]
    public void Constructor_CustomSpellSlots_PreservesValues()
    {
        var spellSlots = new List<int> { 4, 3, 2, 0, 0, 0, 0, 0, 0 };
        var levelData = new ClassLevelData(3, 2, [], spellSlots, []);

        Assert.Equal(spellSlots, levelData.SpellSlots);
    }

    [Fact]
    public void Constructor_Level_PreservesValue()
    {
        var levelData = new ClassLevelData(5, 3, [], [], []);

        Assert.Equal(5, levelData.Level);
    }

    [Fact]
    public void Constructor_ProficiencyBonus_PreservesValue()
    {
        var levelData = new ClassLevelData(10, 4, [], [], []);

        Assert.Equal(4, levelData.ProficiencyBonus);
    }

    [Fact]
    public void Constructor_ClassFeatures_PreservesList()
    {
        var features = new List<string> { "Extra Attack", "Archetype Feature" };
        var levelData = new ClassLevelData(5, 3, features, [], []);

        Assert.Equal(features, levelData.ClassFeatures);
    }

    [Fact]
    public void Constructor_ResourcePools_PreservesDictionary()
    {
        var resources = new Dictionary<string, int>
        {
            { "Ki Points", 6 },
            { "Martial Arts Dice", 2 }
        };
        var levelData = new ClassLevelData(5, 3, [], [], resources);

        Assert.Equal(2, levelData.ResourcePools.Count);
        Assert.Equal(6, levelData.ResourcePools["Ki Points"]);
        Assert.Equal(2, levelData.ResourcePools["Martial Arts Dice"]);
    }
}

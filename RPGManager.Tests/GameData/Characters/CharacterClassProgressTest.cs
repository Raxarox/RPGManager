using RPGManager.GameData.Characters;

namespace RPGManager.Tests.GameData.Characters;

public class CharacterClassProgressTest
{
    private readonly CharacterClass _testClass = new("Fighter", "Fighter", 10, [], [], [], [], 0);
    private readonly Archetype _testArchetype = new("champion", "Champion", "fighter", []);

    [Fact]
    public void Constructor_ValidParameters_CreatesProgress()
    {
        var progress = new CharacterClassProgress(_testClass, 5, "fighter");

        Assert.Equal(_testClass, progress.ClassAsset);
        Assert.Equal(5, progress.Level);
        Assert.Equal("Fighter", progress.ClassId);
    }

    [Fact]
    public void Constructor_LevelBelowOne_ClampsToOne()
    {
        var progress = new CharacterClassProgress(_testClass, 0, "fighter");

        Assert.Equal(1, progress.Level);
    }

    [Fact]
    public void Constructor_NullClassAsset_UsesClassId()
    {
        var progress = new CharacterClassProgress(null, 3, "wizard");

        Assert.Null(progress.ClassAsset);
        Assert.Equal(3, progress.Level);
        Assert.Equal("wizard", progress.ClassId);
    }

    [Fact]
    public void Constructor_WithArchetype_SetsArchetypeId()
    {
        var progress = new CharacterClassProgress(_testClass, 3, "fighter", _testArchetype, "champion");

        Assert.Equal(_testArchetype, progress.ArchetypeAsset);
        Assert.Equal("champion", progress.ArchetypeId);
    }

    [Fact]
    public void SetClassAsset_UpdatesClassId()
    {
        var progress = new CharacterClassProgress(null, 1, "fighter");
        progress.ClassAsset = _testClass;

        Assert.Equal(_testClass, progress.ClassAsset);
        Assert.Equal("Fighter", progress.ClassId);
    }

    [Fact]
    public void SetArchetypeAsset_UpdatesArchetypeId()
    {
        var progress = new CharacterClassProgress(_testClass, 1, "fighter");
        progress.ArchetypeAsset = _testArchetype;

        Assert.Equal(_testArchetype, progress.ArchetypeAsset);
        Assert.Equal("champion", progress.ArchetypeId);
    }

    [Fact]
    public void TrySelectArchetype_ValidArchetypeAtCorrectLevel_Succeeds()
    {
        var characterClass = new CharacterClass("fighter", "Fighter", 10, [], [], [], ["champion", "battle-master"], 3);
        var progress = new CharacterClassProgress(characterClass, 3, "fighter");

        var result = progress.TrySelectArchetype(_testArchetype);

        Assert.True(result);
        Assert.Equal(_testArchetype, progress.ArchetypeAsset);
    }

    [Fact]
    public void TrySelectArchetype_NullClassAsset_ReturnsFalse()
    {
        var progress = new CharacterClassProgress(null, 3, "fighter");

        var result = progress.TrySelectArchetype(_testArchetype);

        Assert.False(result);
    }

    [Fact]
    public void TrySelectArchetype_BelowChoiceLevel_ReturnsFalse()
    {
        var characterClass = new CharacterClass("fighter", "Fighter", 10, [], [], [], ["champion"], 3);
        var progress = new CharacterClassProgress(characterClass, 2, "fighter");

        var result = progress.TrySelectArchetype(_testArchetype);

        Assert.False(result);
    }

    [Fact]
    public void TrySelectArchetype_WrongParentClass_ReturnsFalse()
    {
        var wrongClass = new CharacterClass("wizard", "Wizard", 6, [], [], [], ["champion"], 3);
        var progress = new CharacterClassProgress(wrongClass, 3, "wizard");

        var result = progress.TrySelectArchetype(_testArchetype);

        Assert.False(result);
    }

    [Fact]
    public void TrySelectArchetype_NotInAvailableList_ReturnsFalse()
    {
        var characterClass = new CharacterClass("fighter", "Fighter", 10, [], [], [], ["battle-master"], 3);
        var progress = new CharacterClassProgress(characterClass, 3, "fighter");

        var result = progress.TrySelectArchetype(_testArchetype);

        Assert.False(result);
    }

    [Fact]
    public void Equals_SameClassIdAndLevel_ReturnsTrue()
    {
        var progress1 = new CharacterClassProgress(_testClass, 5, "fighter");
        var progress2 = new CharacterClassProgress(_testClass, 5, "fighter");

        Assert.Equal(progress1, progress2);
        Assert.Equal(progress1.GetHashCode(), progress2.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentClassId_ReturnsFalse()
    {
        var progress1 = new CharacterClassProgress(_testClass, 5, "fighter");
        var otherClass = new CharacterClass("wizard", "Wizard", 6, [], [], [], [], 0);
        var progress2 = new CharacterClassProgress(otherClass, 5, "wizard");

        Assert.NotEqual(progress1, progress2);
    }

    [Fact]
    public void Equals_DifferentLevel_ReturnsFalse()
    {
        var progress1 = new CharacterClassProgress(_testClass, 5, "fighter");
        var progress2 = new CharacterClassProgress(_testClass, 3, "fighter");

        Assert.NotEqual(progress1, progress2);
    }

    [Fact]
    public void Equals_DifferentArchetypeId_ReturnsFalse()
    {
        var progress1 = new CharacterClassProgress(_testClass, 5, "fighter", _testArchetype, "champion");
        var otherArchetype = new Archetype("battle-master", "Battle Master", "fighter", []);
        var progress2 = new CharacterClassProgress(_testClass, 5, "fighter", otherArchetype, "battle-master");

        Assert.NotEqual(progress1, progress2);
    }

    [Fact]
    public void Equals_Null_ReturnsFalse()
    {
        var progress = new CharacterClassProgress(_testClass, 5, "fighter");

        Assert.False(progress.Equals(null));
    }
}

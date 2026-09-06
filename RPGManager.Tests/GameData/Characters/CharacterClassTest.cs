using RPGManager.GameData.Characters;

namespace RPGManager.Tests.GameData.Characters;

public class CharacterClassTest
{
    [Fact]
    public void Constructor_ValidParameters_CreatesCharacterClass()
    {
        var characterClass = new CharacterClass(
            "fighter",
            "Fighter",
            10,
            ["Longsword", "Shield"],
            ["Light Armor", "Medium Armor"],
            [],
            [],
            0);

        Assert.Equal("fighter", characterClass.ClassId);
        Assert.Equal("Fighter", characterClass.Name);
        Assert.Equal(10, characterClass.HitDieValue);
        Assert.Equal(2, characterClass.WeaponProficiencies.Count);
        Assert.Contains("Longsword", characterClass.WeaponProficiencies);
        Assert.Equal(2, characterClass.ArmorProficiencies.Count);
        Assert.Contains("Light Armor", characterClass.ArmorProficiencies);
        Assert.Empty(characterClass.Progression);
        Assert.Empty(characterClass.AvailableArchetypeIds);
        Assert.Equal(0, characterClass.ArchetypeChoiceLevel);
    }

    [Fact]
    public void Constructor_NullLists_DefaultsToEmptyLists()
    {
        var characterClass = new CharacterClass(
            "wizard",
            "Wizard",
            6,
            null,
            null,
            null,
            null,
            0);

        Assert.Empty(characterClass.WeaponProficiencies);
        Assert.Empty(characterClass.ArmorProficiencies);
        Assert.Empty(characterClass.Progression);
        Assert.Empty(characterClass.AvailableArchetypeIds);
    }

    [Fact]
    public void Constructor_InvalidHitDie_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CharacterClass("test", "Test", 0, [], [], [], [], 0));
    }

    [Fact]
    public void Constructor_EmptyName_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new CharacterClass("test", null!, 10, [], [], [], [], 0));
    }

    [Fact]
    public void Constructor_EmptyClassId_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new CharacterClass(null!, "Test", 10, [], [], [], [], 0));
    }

    [Fact]
    public void ToString_ReturnsName()
    {
        var characterClass = new CharacterClass("fighter", "Fighter", 10, [], [], [], [], 0);
        Assert.Equal("Fighter", characterClass.ToString());
    }
}

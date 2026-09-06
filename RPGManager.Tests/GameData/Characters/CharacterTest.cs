using RPGManager.GameData.Characters;

namespace RPGManager.Tests;

public class CharacterTest
{
    private readonly CharacterClass _testClass2 = new ("Wizard", "Wizard", 6, [], [], [], [], 0);
    private readonly CharacterClass _testClass = new ("Fighter", "Fighter", 10, [], [], [], [], 0);

    [Theory]
    [InlineData("Cedric", 20, true)]
    [InlineData("", 20, false)]
    [InlineData("Cedric", -10, false)]
    [InlineData("Cedric", 0, false)]
    public void TryCreate_ValidatesAndUpdatesState(string nameInput, int maxHealth, bool expectedSuccess)
    {
        var success = Character.TryCreate(nameInput, _testClass, maxHealth, out var character, out var message);
        Assert.Equal(expectedSuccess, success);

        if (expectedSuccess)
        {
            Assert.NotNull(character);
            Assert.Equal(nameInput, character.Name);
            Assert.Single(character.Classes);
            Assert.Equal("Fighter", character.Classes[0].ClassAsset?.Name);
            Assert.Equal(string.Empty, message);
        }
        else
        {
            Assert.Null(character);
            Assert.NotEmpty(message);
        }
    }

    [Fact]
    public void TryCreate_NullClass_ReturnsFalse()
    {
        var success = Character.TryCreate("Cedric", null, 20, out var character, out var message);
        Assert.False(success);
        Assert.Null(character);
        Assert.NotEmpty(message);
    }

    [Theory]
    [InlineData("Roland", true)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData(null, false)]
    public void SetName_ValidatesAndUpdatesState(string? nameInput, bool expectedSuccess)
    {
        Assert.True(Character.TryCreate("Cedric", _testClass, 20, out var character, out _));
        var success = character.SetName(nameInput);
        Assert.Equal(expectedSuccess, success);
        if(expectedSuccess) Assert.Equal(character.Name, nameInput);
        else Assert.Equal("Cedric", character.Name);
    }

    [Theory]
    [InlineData(40, true)]
    [InlineData(20, true)]
    [InlineData(0, false)]
    [InlineData(-20, false)]
    public void SetMaxHealth_ValidatesAndUpdatesState(int maxHealthInput, bool expectedSuccess)
    {
        Assert.True(Character.TryCreate("Cedric", _testClass, 20, out var character, out _));
        var originalHealth = character.Health;
        var originalMaxHealth = character.MaxHealth;
        var success = character.SetMaxHealth(maxHealthInput);
        Assert.Equal(expectedSuccess, success);
        if (expectedSuccess)
        {
            Assert.Equal(maxHealthInput, character.MaxHealth);

            // If max health dropped below current health, health should be capped
            // If max health stayed the same or increased, current health shouldn't change
            Assert.Equal(maxHealthInput < originalHealth ? maxHealthInput : originalHealth, character.Health);
        }
        else
        {
            // Ensure state didn't change on failure
            Assert.Equal(originalMaxHealth, character.MaxHealth);
            Assert.Equal(originalHealth, character.Health);
        }
    }

    [Theory]
    [InlineData(30, 30 )]
    [InlineData(50, 40)]
    [InlineData(0, 0)]
    [InlineData(-50, 0)]
    public void SetHealth_ValidatesAndUpdatesState(int healthInput, int expectedResult)
    {
        Assert.True(Character.TryCreate("Cedric", _testClass, 40, out var character, out _));
        character.SetHealth(healthInput);
        Assert.Equal(expectedResult, character.Health);
    }

    [Fact]
    public void LevelUp_ExistingClass_IncreasesLevel()
    {
        Assert.True(Character.TryCreate("Cedric", _testClass, 20, out var character, out _));
        var originalLevel = character.Classes[0].Level;
        var originalMaxHealth = character.MaxHealth;

        character.LevelUp(_testClass, 5); // Hit die roll + Con

        Assert.Equal(originalLevel + 1, character.Classes[0].Level);
        Assert.True(character.MaxHealth > originalMaxHealth);
    }

    [Fact]
    public void LevelUp_NewClass_AddsMulticlass()
    {
        Assert.True(Character.TryCreate("Cedric", _testClass, 20, out var character, out _));
        var originalClassCount = character.Classes.Count;

        character.LevelUp(_testClass2, 3); // Hit die roll + Con

        Assert.Equal(originalClassCount + 1, character.Classes.Count);
        Assert.Contains(character.Classes, c => c.ClassAsset?.Name == "Wizard");
    }

    [Fact]
    public void TotalLevel_SumsAllClassLevels()
    {
        Assert.True(Character.TryCreate("Cedric", _testClass, 20, out var character, out _));
        character.LevelUp(_testClass, 4);
        character.LevelUp(_testClass2, 3);

        Assert.Equal(3, character.TotalLevel); // 1 (initial) + 1 (level up) + 1 (multiclass)
    }

    [Fact]
    public void SetInitialClass_ReplacesExistingClasses()
    {
        Assert.True(Character.TryCreate("Cedric", _testClass, 20, out var character, out _));
        character.LevelUp(_testClass2, 3);

        character.SetInitialClass(_testClass);

        Assert.Single(character.Classes);
        Assert.Equal("Fighter", character.Classes[0].ClassAsset?.Name);
        Assert.Equal(1, character.Classes[0].Level);
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrueAndMatchesHashCodes()
    {
        var fighterClass = _testClass;
        Character.TryCreate("Cedric", fighterClass, 20, out var char1, out _);
        Character.TryCreate("Cedric", fighterClass, 20, out var char2, out _);

        Assert.True(char1!.Equals(char2));
        Assert.True(char2!.Equals(char1));
        Assert.True(char1.Equals((object)char2));
        Assert.Equal(char1.GetHashCode(), char2.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentValues_ReturnsFalse()
    {
        var fighterClass = _testClass;
        var wizardClass = _testClass2;
        Character.TryCreate("Cedric", fighterClass, 20, out var char1, out _);
        Character.TryCreate("Roland", fighterClass, 20, out var char2, out _); // Different name
        Character.TryCreate("Cedric", wizardClass, 20, out var char3, out _);  // Different class

        Assert.False(char1!.Equals(char2));
        Assert.False(char1.Equals(char3));
    }

    [Fact]
    public void Equals_NullOrWrongType_ReturnsFalse()
    {
        Character.TryCreate("Cedric", _testClass, 20, out var character, out _);

        Assert.False(character!.Equals(null));
        Assert.False(character.Equals("NotACharacter"));
    }
}
using RPGManager.GameData.Characters;

namespace RPGManager.Tests;

public class CharacterTest
{
    CharacterClass testClass2 = new ("Wizard", "Wizard", 6, true, [], []);
    CharacterClass testClass = new ("Fighter", "Fighter", 10, false, [], []);
    [Theory]
    [InlineData("Cedric", 20, true)]
    [InlineData("", 20, false)]
    [InlineData("Cedric", -10, false)]
    [InlineData("Cedric", 0, false)]
    public void TryCreate_ValidatesAndUpdatesState(string nameInput, int maxHealth, bool expectedSuccess)
    {
        
        var success = Character.TryCreate(nameInput, testClass, maxHealth, out var character, out var message);
        Assert.Equal(expectedSuccess, success);

        if (expectedSuccess)
        {
            Assert.NotNull(character);
            Assert.Equal(nameInput, character.Name);
            Assert.Equal("Fighter", character.Class.Name);
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
        Assert.True(Character.TryCreate("Cedric", testClass, 20, out var character, out _));
        var success = character.SetName(nameInput);
        Assert.Equal(expectedSuccess, success);
        if(expectedSuccess) Assert.Equal(character.Name, nameInput);
        else Assert.Equal("Cedric", character.Name);
    }    
    
    [Fact]
    public void SetClass_ValidClass_UpdatesState()
    {
        Assert.True(Character.TryCreate("Cedric", testClass, 20, out var character, out _));
        var success = character.SetCharacterClass(testClass2);
        Assert.True(success);
        Assert.Equal("Wizard", character.Class.Name);
    }

    [Fact]
    public void SetClass_NullClass_ReturnsFalse()
    {
        Assert.True(Character.TryCreate("Cedric", testClass, 20, out var character, out _));
        var success = character.SetCharacterClass(null);
        Assert.False(success);
        Assert.Equal("Fighter", character.Class.Name);
    }

    [Theory]
    [InlineData(40, true)]
    [InlineData(20, true)]
    [InlineData(0, false)]
    [InlineData(-20, false)]
    public void SetMaxHealth_ValidatesAndUpdatesState(int maxHealthInput, bool expectedSuccess)
    {
        Assert.True(Character.TryCreate("Cedric", testClass, 20, out var character, out _));
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
        Assert.True(Character.TryCreate("Cedric", testClass, 40, out var character, out _));
        character.SetHealth(healthInput);
        Assert.Equal(expectedResult, character.Health);
    }   
    
    [Fact]
    public void Equals_SameValues_ReturnsTrueAndMatchesHashCodes()
    {
        var fighterClass = testClass;
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
        var fighterClass = testClass;
        var wizardClass = testClass2;
        Character.TryCreate("Cedric", fighterClass, 20, out var char1, out _);
        Character.TryCreate("Roland", fighterClass, 20, out var char2, out _); // Different name
        Character.TryCreate("Cedric", wizardClass, 20, out var char3, out _);  // Different class

        Assert.False(char1!.Equals(char2));
        Assert.False(char1.Equals(char3));
    }

    [Fact]
    public void Equals_NullOrWrongType_ReturnsFalse()
    {
        Character.TryCreate("Cedric", testClass, 20, out var character, out _);

        Assert.False(character!.Equals(null));
        Assert.False(character.Equals("NotACharacter"));
    }
}
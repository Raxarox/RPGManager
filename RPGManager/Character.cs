using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPGManager.CharacterClasses;
namespace RPGManager;

public class Character
{
    public string Name { get; private set; } = string.Empty;
    public CharacterClass Class { get; private set; } = null!;
    public AbilityScore AbilityScores { get; private set; } = new();
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }

    private Character()
    {
        Class = null!;
    }

    [JsonConstructor]
    public Character(
        string name,
        CharacterClass @class,
        int maxHealth,
        int health,
        AbilityScore? abilityScores = null)
    {
        if (!SetName(name))
            throw new JsonException("Invalid character name.");

        if (!SetCharacterClass(@class))
            throw new JsonException("Invalid character class.");

        if (!SetMaxHealth(maxHealth))
            throw new JsonException("Invalid max health.");

        SetHealth(health);

        AbilityScores = abilityScores ?? new AbilityScore();
    }

    public static bool TryCreate(
        string? name,
        CharacterClass? characterClass,
        int maxHealth,
        [NotNullWhen(true)] out Character? character,
        out string errorMessage)
    {
        character = null;
        errorMessage = string.Empty;

        var newCharacter = new Character();

        if (!newCharacter.SetName(name))
        {
            errorMessage = "Name cannot be empty.";
            return false;
        }

        if (!newCharacter.SetCharacterClass(characterClass))
        {
            errorMessage = $"Class must be specified.";
            return false;
        }

        if (!newCharacter.SetMaxHealth(maxHealth))
        {
            errorMessage = "Max health must be greater than zero.";
            return false;
        }

        newCharacter.SetHealth(maxHealth);

        character = newCharacter;
        return true;
    }

    public bool SetName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        Name = name;
        return true;
    }

    public bool SetCharacterClass(CharacterClass? newClass)
    {
        if (newClass == null)
            return false;

        Class = newClass;
        return true;
    }

    public bool SetMaxHealth(int maxHealth)
    {
        if (maxHealth <= 0)
            return false;

        MaxHealth = maxHealth;
        // A character's current HP can never exceed their maximum HP.
        Health = Math.Min(Health, MaxHealth);
        return true;
    }

    public void SetHealth(int health)
    {
        Health = Math.Clamp(health, 0, MaxHealth);
    }

    public bool IsDown()
    {
        return Health <= 0;
    }

    public bool Equals(Character? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Name == other.Name &&
               Class.Name == other.Class.Name &&
               MaxHealth == other.MaxHealth &&
               Health == other.Health &&
               AbilityScores.Equals(other.AbilityScores);
    }

    public override bool Equals(object? obj)
    {
        return obj is Character other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Class.Name, MaxHealth, Health, AbilityScores);
    }
}

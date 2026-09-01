using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace RPGManager;

public class Character
{
    public string Name { get; private set; } = string.Empty;
    public string CharacterClass { get; private set;} = string.Empty;
    public static readonly List<string> ValidClasses = ["Warrior", "Wizard", "Rogue"];
    public int AttackPower { get; private set;}
    public int MaxHealth { get; private set;}
    public int Health { get; private set;}

    private Character()
    {
    }
    [JsonConstructor]
    public Character(
        string name,
        string characterClass,
        int attackPower,
        int maxHealth,
        int health)
    {
        if (!SetName(name))
            throw new JsonException("Invalid character name.");

        if (!SetCharacterClass(characterClass))
            throw new JsonException("Invalid character class.");

        if (!SetMaxHealth(maxHealth))
            throw new JsonException("Invalid max health.");

        if (!SetHealth(health))
            throw new JsonException("Invalid health.");

        if (!SetAttackPower(attackPower))
            throw new JsonException("Invalid attack power.");
    }
    public static bool TryCreate(
        string? name,
        string? characterClass,
        int maxHealth,
        int attackPower,
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
            errorMessage = $"Class must be one of: {string.Join(", ", ValidClasses)}.";
            return false;
        }

        if (!newCharacter.SetMaxHealth(maxHealth))
        {
            errorMessage = "Max health must be greater than zero.";
            return false;
        }

        if (!newCharacter.SetHealth(maxHealth))
        {
            errorMessage = "Health must be greater than zero.";
            return false;
        }

        if (!newCharacter.SetAttackPower(attackPower))
        {
            errorMessage = "Attack power must be greater than zero.";
            return false;
        }

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

    public bool SetCharacterClass(string? characterClass)
    {
        var validClass = ValidClasses.FirstOrDefault(
            c => string.Equals(c, characterClass, StringComparison.OrdinalIgnoreCase));

        if (validClass == null)
            return false;

        CharacterClass = validClass;
        return true;
    }

    public bool SetAttackPower(int attackPower)
    {
        if (attackPower <= 0)
            return false;

        AttackPower = attackPower;
        return true;
    }

    public bool SetMaxHealth(int maxHealth)
    {
        if (maxHealth <= 0)
            return false;

        MaxHealth = maxHealth;
        Health = Math.Min(Health, MaxHealth);
        return true;
    }

    public bool SetHealth(int health)
    {
        if (health <= 0)
            return false;

        Health = Math.Min(health, MaxHealth);
        return true;
    }

    public bool DiffersFrom(Character? otherCharacter)
    {
        foreach (var prop in typeof(Character).GetProperties())
        {
            //for future custom List<Object> properties:
            //if (prop.Name == nameof(Inventory)) continue; 
            var thisValue = prop.GetValue(this);
            var otherValue = prop.GetValue(otherCharacter);
            if (!Equals(thisValue, otherValue)) return true;
        }
        //if (this.Inventory.HasChanges(other.Inventory)) return true;

        return false;
    }
}

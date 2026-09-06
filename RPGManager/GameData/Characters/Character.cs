using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGManager.GameData.Characters;

public class Character
{
    public string Name { get; private set; } = string.Empty;
    public List<CharacterClassProgress> Classes { get; private set; } = new();
    public AbilityScore AbilityScores { get; private set; } = new();
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }

    // Total character level across all classes
    [JsonIgnore]
    public int TotalLevel => Classes.Sum(c => c.Level);

    private Character() { }

    [JsonConstructor]
    public Character(
        string name,
        List<CharacterClassProgress>? classes,
        int maxHealth,
        int health,
        AbilityScore? abilityScores = null)
    {
        if (!SetName(name))
            throw new JsonException("Invalid character name.");

        Classes = classes ?? [];
        if (Classes.Count == 0)
            throw new JsonException("Character must have at least one class.");

        if (!SetMaxHealth(maxHealth))
            throw new JsonException("Invalid max health.");

        SetHealth(health);
        AbilityScores = abilityScores ?? new AbilityScore();
    }

    public static bool TryCreate(
        string? name,
        CharacterClass? initialClass,
        int maxHealth,
        [NotNullWhen(true)] out Character? character,
        out string errorMessage)
    {
        character = null;
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(name))
        {
            errorMessage = "Name cannot be empty.";
            return false;
        }

        if (initialClass == null)
        {
            errorMessage = "Initial class must be specified.";
            return false;
        }

        if (maxHealth <= 0)
        {
            errorMessage = "Max health must be greater than zero.";
            return false;
        }

        var newCharacter = new Character
        {
            Name = name,
            Classes = [new CharacterClassProgress(initialClass, 1, initialClass.ClassId)],
            MaxHealth = maxHealth,
            Health = maxHealth,
            AbilityScores = new AbilityScore()
        };

        character = newCharacter;
        return true;
    }

    /// <summary>
    /// Levels up an existing class or adds a new multiclass, increasing max health using that class's hit die.
    /// </summary>
    public void LevelUp(CharacterClass targetClass, int rolledHitDiePlusCon)
    {
        var classProgress = Classes.FirstOrDefault(c => c.ClassAsset?.ClassId == targetClass.ClassId);

        if (classProgress != null)
        {
            classProgress.Level++;
        }
        else
        {
            Classes.Add(new CharacterClassProgress(targetClass, 1, targetClass.ClassId));
        }

        // Increase max health by the specific hit die roll + Con modifier passed into the method
        SetMaxHealth(MaxHealth + Math.Max(1, rolledHitDiePlusCon));
        // Fully heal or scale current health proportionally depending on your preference; usually, HP increases raw max.
        Health = MaxHealth;
    }

    public bool SetName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        Name = name;
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

    public void SetHealth(int health)
    {
        Health = Math.Clamp(health, 0, MaxHealth);
    }

    public bool IsDown() => Health <= 0;

    public void SetInitialClass(CharacterClass newClass)
    {
        Classes.Clear();
        Classes.Add(new CharacterClassProgress(newClass, 1, newClass.ClassId));
    }
    
    public bool Equals(Character? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return Name == other.Name &&
               MaxHealth == other.MaxHealth &&
               Health == other.Health &&
               AbilityScores.Equals(other.AbilityScores) &&
               Classes.SequenceEqual(other.Classes);
    }

    public override bool Equals(object? obj) => obj is Character other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Name, MaxHealth, Health, AbilityScores);
}


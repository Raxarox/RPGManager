using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGManager;

public class AbilityScore: IEquatable<AbilityScore>
{
    public int Strength { get; private set; }
    public int Dexterity { get; private set; }
    public int Constitution { get; private set; }
    public int Intelligence { get; private set; }
    public int Wisdom { get; private set; }
    public int Charisma { get; private set; }

    public AbilityScore() : this(10, 10, 10, 10, 10, 10)
    {
    }

    [JsonConstructor]
    public AbilityScore(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
    {
        if (!SetStrength(strength) || !SetDexterity(dexterity) || !SetConstitution(constitution) ||
            !SetIntelligence(intelligence) || !SetWisdom(wisdom) || !SetCharisma(charisma))
        {
            throw new JsonException("Ability scores must be greater than zero.");
        }
    }

    public bool SetStrength(int value) => SetScore(value, score => Strength = score);
    public bool SetDexterity(int value) => SetScore(value, score => Dexterity = score);
    public bool SetConstitution(int value) => SetScore(value, score => Constitution = score);
    public bool SetIntelligence(int value) => SetScore(value, score => Intelligence = score);
    public bool SetWisdom(int value) => SetScore(value, score => Wisdom = score);
    public bool SetCharisma(int value) => SetScore(value, score => Charisma = score);

    public bool DiffersFrom(AbilityScore? otherAbilityScores)
    {
        if (otherAbilityScores == null) return true;

        return Strength != otherAbilityScores.Strength ||
               Dexterity != otherAbilityScores.Dexterity ||
               Constitution != otherAbilityScores.Constitution ||
               Intelligence != otherAbilityScores.Intelligence ||
               Wisdom != otherAbilityScores.Wisdom ||
               Charisma != otherAbilityScores.Charisma;
    }

    public bool Equals(AbilityScore? other)
    {
        if (other is null) return false;

        return Strength == other.Strength &&
               Dexterity == other.Dexterity &&
               Constitution == other.Constitution &&
               Intelligence == other.Intelligence &&
               Wisdom == other.Wisdom &&
               Charisma == other.Charisma;
    }

    public override bool Equals(object? obj)
    {
        return obj is AbilityScore other && Equals(other);
    }
    
    public override int GetHashCode()
    {
      return HashCode.Combine(Strength, Dexterity, Constitution, Intelligence,  Wisdom, Charisma);
    }

    private static bool SetScore(int value, Action<int> setValue)
    {
        if (value <= 0) return false;

        setValue(value);
        return true;
    }
}

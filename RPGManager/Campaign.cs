using System.Text.Json.Serialization;
namespace RPGManager;

public class Campaign
{
    public List<Character> Characters { get; private set; }

    public Campaign()
    {
        Characters = [];
    }

    [JsonConstructor]
    public Campaign(List<Character> characters)
    {
        Characters = characters;
    }

    public void AddCharacter(Character character)
    {
        ArgumentNullException.ThrowIfNull(character);
        Characters.Add(character);
    }

    public void RemoveCharacter(Character character)
    {
        ArgumentNullException.ThrowIfNull(character);
        Characters.Remove(character);
    }

    public bool Equals(Campaign? other)
    {
        if (other == null) return false;
        if (this.Characters.Count != other.Characters.Count) return false;
        for (int i = 0; i < Characters.Count; i++)
        {
            if (!this.Characters[i].Equals(other.Characters[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        return obj is Campaign other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var character in Characters)
        {
            hash.Add(character);
        }

        return hash.ToHashCode();
    }
}
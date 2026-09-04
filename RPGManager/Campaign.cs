using System.Text.Json.Serialization;
using RPGManager.CharacterClasses;

namespace RPGManager;

public class Campaign
{
    public List<Character> Characters { get; private set; }
    private readonly List<CharacterClass> _availableClasses = new();
    public IReadOnlyCollection<CharacterClass> AvailableClasses => _availableClasses.AsReadOnly();

    public Campaign()
    {
        Characters = [];
        _availableClasses.AddRange([
            new Fighter(),
            new Rogue(),
            new Wizard()
        ]);
    }

    [JsonConstructor]
    public Campaign(List<Character> characters)
    {
        Characters = characters;
        _availableClasses.AddRange([
            new Fighter(),
            new Rogue(),
            new Wizard()
        ]);
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
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        
        if (this.Characters.Count != other.Characters.Count) return false;
        for (int i = 0; i < Characters.Count; i++)
        {
            if (!this.Characters[i].Equals(other.Characters[i]))
            {
                return false;
            }
        }
        
        if (this._availableClasses.Count != other._availableClasses.Count) return false;
        for (int i = 0; i < _availableClasses.Count; i++)
        {

            if (this._availableClasses[i].Name != other._availableClasses[i].Name)
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj) => obj is Campaign other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
    
        foreach (var character in Characters)
        {
            hash.Add(character);
        }

        foreach (var cls in _availableClasses)
        {
            hash.Add(cls);
        }

        return hash.ToHashCode();
    }
}
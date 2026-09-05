using System.Text.Json.Serialization;
using RPGManager.GameData.Characters;

namespace RPGManager.GameData.Campaigns;

public class Campaign
{
    public string CampaignName { get; init; }
    public List<Character> Characters { get; init; }
    
    // Stores the identifiers of classes allowed in this specific campaign
    public List<string> AvailableClasses { get; init; }

    // Default constructor for standard programmatic creation.
    // Starts with no available classes — call EnableAllClasses() once
    // the caller has access to the master class registry.
    public Campaign()
    {
        CampaignName = "Default Campaign";
        Characters = [];
        AvailableClasses = [];
    }

    [JsonConstructor]
    public Campaign(string campaignName, List<Character> characters, List<string> availableClasses)
    {
        CampaignName = campaignName ?? "Default Campaign";
        Characters = characters ?? [];

        // An old save with no restrictions recorded loads as "nothing available"
        // rather than guessing at what the master registry currently contains.
        AvailableClasses = availableClasses ?? [];
    }

    // Seeds AvailableClasses with every class ID currently in the master registry.
    // Mutates the list in place (rather than reassigning it) since AvailableClasses
    // is an init-only property and can't be reassigned outside the constructor.
    public void EnableAllClasses(IEnumerable<string> classIds)
    {
        AvailableClasses.Clear();
        AvailableClasses.AddRange(classIds);
    }

    // Removes a class from this campaign's available list only.
    // The class itself still exists in the master registry — this never deletes it.
    public bool RemoveAvailableClass(string classId) =>
        AvailableClasses.RemoveAll(id => string.Equals(id, classId, StringComparison.OrdinalIgnoreCase)) > 0;

    // Re-adds a previously removed class back to this campaign's available list.
    // Does not validate that classId exists in the master registry — Campaign has
    // no reference to it, so that check belongs at the call site.
    public void AddAvailableClass(string classId)
    {
        if (!AvailableClasses.Contains(classId, StringComparer.OrdinalIgnoreCase))
            AvailableClasses.Add(classId);
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
    
        if (this.CampaignName != other.CampaignName) return false;
        if (this.Characters.Count != other.Characters.Count) return false;
    
        for (int i = 0; i < Characters.Count; i++)
        {
            if (!this.Characters[i].Equals(other.Characters[i])) return false;
        }
    
        if (this.AvailableClasses.Count != other.AvailableClasses.Count) return false;
        for (int i = 0; i < AvailableClasses.Count; i++)
        {
            if (this.AvailableClasses[i] != other.AvailableClasses[i]) return false;
        }

        return true;
    }

    public override bool Equals(object? obj) => obj is Campaign other && Equals(other);

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(CampaignName);
    
        foreach (var character in Characters)
        {
            hash.Add(character);
        }

        foreach (var cls in AvailableClasses)
        {
            hash.Add(cls);
        }

        return hash.ToHashCode();
    }
}
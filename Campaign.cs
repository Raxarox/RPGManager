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
        Characters.Add(character);
    }
    public void RemoveCharacter(Character character)
    {
        Characters.Remove(character);
    }

    public bool DiffersFrom(Campaign? otherCampaign)
    {
        if (otherCampaign == null) return true;
        if (this.Characters.Count != otherCampaign.Characters.Count) return true;
        for (int i = 0; i < Characters.Count; i++)
        {
            if (this.Characters[i].DiffersFrom(otherCampaign.Characters[i]))
            {
                return true;
            }
        }
        return false;
    }
}
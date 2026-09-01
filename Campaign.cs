namespace RPGManager;

public class Campaign
{
    public List<Character> Characters { get; private set; } = [];

    public void AddCharacter(Character character)
    {
        Characters.Add(character);
    }
    public void RemoveCharacter(Character character)
    {
        Characters.Remove(character);
    }
}
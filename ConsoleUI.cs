namespace RPGManager;

public static class ConsoleUI
{
    public static string GetCharacterInfo(Character character)
    {
        var scores = character.AbilityScores;
        return character.Name + " - " +
               character.CharacterClass + " - " +
               character.MaxHealth + " Max HP - " +
               character.Health + " HP." +
               $"\nSTR: {scores.Strength} | DEX: {scores.Dexterity} | CON: {scores.Constitution} | " +
               $"INT: {scores.Intelligence} | WIS: {scores.Wisdom} | CHA: {scores.Charisma}";
    }

    public static int GetPositiveIntegerFor(string valueName)
    {
        while (true)
        {
            Console.WriteLine("What is the character's new " + valueName + "? (Only positive values allowed).");
            var input = Console.ReadLine();
            if (int.TryParse(input, out var result) && result > 0)
            {
                return result;
            }

            Console.WriteLine("Invalid input.");
        }
    }

    public static bool ConfirmOperation()
    {
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");
        var confirmationInput = Console.ReadLine();
        while (confirmationInput != "1" && confirmationInput != "2")
        {
            Console.WriteLine("Invalid input.");
            confirmationInput = Console.ReadLine();
        }

        if (confirmationInput != "2") return true;
        Console.WriteLine("Operation cancelled.");
        return false;
    }
    
    public static (string? Name, string? CharacterClass, int Hp)
        GetNewCharacterDetails(IEnumerable<string> validClasses)
    {
        Console.WriteLine("What is the character's name?");
        var name = Console.ReadLine();

        Console.WriteLine(
            "What is the character's class?\n" +
            string.Join("\n", validClasses));
        var characterClass = Console.ReadLine();

        var hp = GetPositiveIntegerFor("HP");
        return (name, characterClass, hp);
    }
}

namespace RPGManager;

public static class ConsoleUI
{
    public static string GetCharacterInfo(Character character)
    {
        return character.Name + " - " +
               character.CharacterClass + " - " +
               character.MaxHealth + " Max HP - " +
               character.Health + " HP - " +
               character.AttackPower + " ATK.";
    }

    public static void PrintCharacters(Campaign campaign)
    {
        for (var i = 0; i < campaign.Characters.Count; i++)
        {
            Console.WriteLine("ID: " + i + ". " +
                              GetCharacterInfo(campaign.Characters[i]));
        }
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

    public static int GetCharacterIndex(List<Character> characters)
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (int.TryParse(input, out var result) && result >= 0 && result < characters.Count)
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
    
    public static (string? Name, string? CharacterClass, int Hp, int Ap)
        GetNewCharacterDetails(IEnumerable<string> validClasses)
    {
        Console.WriteLine("What is the character's name?");
        var name = Console.ReadLine();

        Console.WriteLine(
            "What is the character's class?\n" +
            string.Join("\n", validClasses));
        var characterClass = Console.ReadLine();

        var hp = GetPositiveIntegerFor("HP");
        var ap = GetPositiveIntegerFor("attack power");

        return (name, characterClass, hp, ap);
    }
}

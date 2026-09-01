namespace RPGManager;

public static class CharacterEditor
{
    public static void Run(Campaign campaign)
    {
        var exit = false;
        while (!exit)
        {
            Console.WriteLine("CHARACTER EDITOR");
            Console.WriteLine("1. Edit Character");
            Console.WriteLine("2. Remove character");
            Console.WriteLine("3. Exit");
            var editMenuInput = Console.ReadLine();
            switch (editMenuInput)
            {
                case "1":
                    EditCharacter(campaign);
                    break;
                case "2":
                    RemoveCharacter(campaign);
                    break;
                case "3":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        }
    }

    private static void RemoveCharacter(Campaign campaign)
    {
        ConsoleUI.PrintCharacters(campaign);
        Console.WriteLine("Which character would you like to remove? Please enter the character's ID");
        var character = campaign.Characters[ConsoleUI.GetCharacterIndex(campaign.Characters)];
        Console.WriteLine("Are you sure you want to remove " + character.Name + "?");
        Console.WriteLine(ConsoleUI.GetCharacterInfo(character));
        if (ConsoleUI.ConfirmOperation()) campaign.RemoveCharacter(character);
    }

    private static void EditCharacter(Campaign campaign)
    {
        ConsoleUI.PrintCharacters(campaign);
        Console.WriteLine("Which character would you like to edit? Please enter the character's ID");
        var character = campaign.Characters[ConsoleUI.GetCharacterIndex(campaign.Characters)];
        var close = false;
        while (!close)
        {
            Console.WriteLine(ConsoleUI.GetCharacterInfo(character));
            Console.WriteLine("What would you like to edit?");
            Console.WriteLine("1. Edit Name.");
            Console.WriteLine("2. Edit Class.");
            Console.WriteLine("3. Edit Max HP.");
            Console.WriteLine("4. Edit Attack Power.");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1": EditName(character); close = true; break;
                case "2": EditClass(character); close = true; break;
                case "3": EditMaxHp(character); close = true; break;
                case "4": EditAttackPower(character); close = true; break;
                default: Console.WriteLine("Invalid input."); break;
            }
        }
    }

    private static void EditName(Character character)
    {
        Console.WriteLine("What would you like to change " + character.Name + "'s name to?");
        string? input = Console.ReadLine();
        while (!character.SetName(input))
        {
            Console.WriteLine("Please enter a name.");
            input = Console.ReadLine();
        }

        Console.WriteLine("Character's name changed to '" + character.Name + "' successfully");
    }

    private static void EditClass(Character character)
    {
        Console.WriteLine("What would you like to change " + character.Name + "'s class to?" +
                          "\n" + string.Join("\n", Character.ValidClasses));
        string? input = Console.ReadLine();
        while (input == null || !character.SetCharacterClass(input))
        {
            Console.WriteLine("Please enter a valid class.");
            input = Console.ReadLine();
        }

        Console.WriteLine(character.Name + "'s class changed to '" + character.CharacterClass + "' successfully");
    }

    private static void EditMaxHp(Character character)
    {
        Console.WriteLine("What would you like to change " + character.Name + "'s max HP to?");
        character.SetMaxHealth(ConsoleUI.GetPositiveIntegerFor("max HP"));
        Console.WriteLine("Character's max HP changed to '" + character.MaxHealth + "' successfully");
    }

    private static void EditAttackPower(Character character)
    {
        Console.WriteLine("What would you like to change " + character.Name + "'s attack power to?");
        character.SetAttackPower(ConsoleUI.GetPositiveIntegerFor("attack power"));
        Console.WriteLine("Character's attack power changed to '" + character.AttackPower + "' successfully");
    }
}

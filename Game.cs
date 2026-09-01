namespace RPGManager;

public static class Game
{
    public static void RunGame()
    {
        var campaign = new Campaign();
        var exit = false;
        while (!exit)
        {
            Console.WriteLine("""
                              ================================
                                      RPG CAMPAIGN MANAGER
                              ================================
                              """);
            Console.WriteLine("1. Create Character");
            Console.WriteLine("2. Edit characters");
            Console.WriteLine("3. Exit");
            var mainMenuInput = Console.ReadLine();
            switch (mainMenuInput)
            {
                case "1":
                    CreateCharacter(campaign);
                    break;
                case "2":
                    if (campaign.Characters.Count == 0)
                    {
                        Console.WriteLine("Please create a character first.");
                        break;
                    }
                    PrintCharacters(campaign);
                    EditCharactersMenu(campaign);
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

    private static void CreateCharacter(Campaign campaign)
    {
        Console.WriteLine("What is the character's name?");
        var name = Console.ReadLine();
        Console.WriteLine("What is the character's class?" + "\n" + string.Join("\n", Character.ValidClasses));
        var characterClass = Console.ReadLine();
        var hp = GetPositiveIntegerFor("HP");
        var ap = GetPositiveIntegerFor("attack power");
        if (!Character.TryCreate(name, characterClass, hp, ap, out var character, out var errorMessage))
        {
            Console.WriteLine($"Could not create character: {errorMessage}");
            return;
        }

        campaign.AddCharacter(character);
    }

    private static string GetCharacterInfo(Character character)
    {
            return character.Name + " - " +
                   character.CharacterClass + " - " +
                   character.MaxHealth + " Max HP - " +
                   character.Health + " HP - " +
                   character.AttackPower + " ATK.";
        
    }
    
    private static void PrintCharacters(Campaign campaign)
    {
        for (int i = 0; i < campaign.Characters.Count; i++)
        {
            Console.WriteLine("ID: " + i + ". " +
                              GetCharacterInfo(campaign.Characters[i]));
        }
    }

    private static int GetPositiveIntegerFor(string valueName)
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

    private static void EditCharactersMenu(Campaign campaign)
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
        PrintCharacters(campaign);
        Console.WriteLine("Which character would you like to remove? Please enter the character's ID");
        var character = campaign.Characters[GetCharacterIndex(campaign.Characters)];
            var close = false;
            while (!close)
            {
                Console.WriteLine("Are you sure you want to remove " + character.Name + "?");
                Console.WriteLine(GetCharacterInfo(character));
                Console.WriteLine("1. Remove Character.");
                Console.WriteLine("2. Cancel.");
                var confirmationInput = Console.ReadLine();
                if (confirmationInput == "1")
                {
                    Console.WriteLine(character.Name + " removed.");
                    campaign.RemoveCharacter(character);
                    close = true;
                }
                else if (confirmationInput == "2") close = true;
                else Console.WriteLine("Invalid input.");
            }

    }
    private static int GetCharacterIndex(List<Character> characters)
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

    private static void EditCharacter(Campaign campaign)
    {
        PrintCharacters(campaign);
        Console.WriteLine("Which character would you like to edit? Please enter the character's ID");
        var character = campaign.Characters[GetCharacterIndex(campaign.Characters)];
        var close = false;
        while (!close)
        {
            Console.WriteLine(GetCharacterInfo(character));
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
        character.SetMaxHealth(GetPositiveIntegerFor("max HP"));
        Console.WriteLine("Character's max HP changed to '" + character.MaxHealth + "' successfully");
    }
    
    private static void EditAttackPower(Character character)
    {
        Console.WriteLine("What would you like to change " + character.Name + "'s attack power to?");
        character.SetAttackPower(GetPositiveIntegerFor("attack power"));
        Console.WriteLine("Character's attack power changed to '" + character.AttackPower + "' successfully");
    }
}

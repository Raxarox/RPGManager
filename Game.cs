namespace RPGManager;

public static class Game
{
    public static void RunGame()
    {
        Campaign campaign = new Campaign();
        var exit = false;
        Campaign savedCampaign = new Campaign();
        while (!exit)
        {
            Console.WriteLine("""
                              ================================
                                      RPG CAMPAIGN MANAGER
                              ================================
                              """);
            Console.WriteLine("1. Create Character");
            Console.WriteLine("2. Edit characters");
            Console.WriteLine("3. Save Campaign");
            Console.WriteLine("4. Load Campaign");
            Console.WriteLine("5. Exit");
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
                    savedCampaign= SaveCampaignMenu(campaign);
                    Console.WriteLine(campaign.DiffersFrom(savedCampaign));
                    break;
                case "4":
                    if(ExitConfirmationMenu(campaign, savedCampaign))
                    {
                        (campaign, savedCampaign) = LoadCampaignMenu(campaign, savedCampaign);
                    }
                    break;
                case "5":
                    if(ExitConfirmationMenu(campaign, savedCampaign)) 
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
        Console.WriteLine("Are you sure you want to remove " + character.Name + "?");
        Console.WriteLine(GetCharacterInfo(character));
        if (OperationConfirmationMenu())campaign.RemoveCharacter(character);
                   
            

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

    private static Campaign SaveCampaignMenu(Campaign campaign)
    {
        Console.WriteLine("What would you like to save the campaign as?");
        var input = Console.ReadLine();
        if (!string.IsNullOrEmpty(input)&&!File.Exists(SaveManager.GetFileNamed(input)))
        {
            var savedPath = SaveManager.GetFileNamed(input);
            SaveManager.Serialize(campaign, input);
            return SaveManager.Deserialize(savedPath);
        }
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Please enter a campaign name.");
            return campaign;
        }
        Console.WriteLine("The campaign file already exists. Do you want to overwrite it?");
            if(OperationConfirmationMenu())
            {
                var savedPath = SaveManager.GetFileNamed(input);
                SaveManager.Serialize(campaign, input);
                return SaveManager.Deserialize(savedPath);
            }

            return campaign;
    }
    private static (Campaign Campaign, Campaign SavedCampaign) LoadCampaignMenu(Campaign campaign, Campaign savedCampaign)
    {
        while (true)
        {
            Console.WriteLine("What campaign would you like to load?");
            var saves = SaveManager.GetSaves();
            int i;
            for (i = 0; i < saves.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(saves[i])}");
            }

            Console.WriteLine($"{i + 1}. Cancel load.");
            var selectedFile = Console.ReadLine();
            if (int.TryParse(selectedFile, out var selectedFileNumber) &&
                selectedFileNumber > 0 &&
                selectedFileNumber <= saves.Length)
            {
                var loadedCampaign = SaveManager.Deserialize(saves[selectedFileNumber - 1]);
                savedCampaign = SaveManager.Deserialize(saves[selectedFileNumber - 1]);
                
                return (loadedCampaign, savedCampaign);
            }
            else if(selectedFile == $"{i+1}")
            {
                Console.WriteLine("Load cancelled.");
                return (campaign, savedCampaign);
            }
            else Console.WriteLine("Invalid input.");
        }
    }

    private static bool ExitConfirmationMenu(Campaign campaign, Campaign? savedCampaign)
    {
        if (!campaign.DiffersFrom(savedCampaign) || campaign.Characters.Count == 0) return true;
        Console.WriteLine("You haven't saved your current campaign. Are you sure you want to proceed?");
        return OperationConfirmationMenu();
    }

    private static bool OperationConfirmationMenu()
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
}

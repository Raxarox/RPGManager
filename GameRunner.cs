namespace RPGManager;

public static class GameRunner
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
                    CharacterEditor.Run(campaign);
                    break;
                case "3":
                    savedCampaign= SaveCampaignMenu(campaign);
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
        var input = PromptForNewCharacterDetails();

        if (!Character.TryCreate(
                input.Name, input.CharacterClass, input.Hp,
                out var character, out var errorMessage))
        {
            Console.WriteLine($"Could not create character: {errorMessage}");
            return;
        }

        campaign.AddCharacter(character);
    }

    private static (string? Name, string? CharacterClass, int Hp) PromptForNewCharacterDetails()
    {
        Console.WriteLine("What is the character's name?");
        var name = Console.ReadLine();

        Console.WriteLine(
            "What is the character's class?\n" +
            string.Join("\n", Character.ValidClasses));
        var characterClass = Console.ReadLine();

        var hp = ConsolePrompts.GetPositiveIntegerFor("HP");
        return (name, characterClass, hp);
    }

    private static Campaign SaveCampaignMenu(Campaign campaign)
    {
        Console.WriteLine("What would you like to save the campaign as?");
        var input = Console.ReadLine();
        try
        {
            if (!string.IsNullOrEmpty(input) && !SaveManager.SaveExists(input))
            {
                SaveManager.Save(campaign, input);
                return SaveManager.Load(input);
            }

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please enter a campaign name.");
                return campaign;
            }

            Console.WriteLine("The campaign file already exists. Do you want to overwrite it?");
            if (!ConsolePrompts.ConfirmOperation()) return campaign;
            SaveManager.Save(campaign, input);
            return SaveManager.Load(input);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[SAVE ERROR] {ex.Message}");
            return campaign;
        }
    }
    private static (Campaign Campaign, Campaign SavedCampaign)
        LoadCampaignMenu(Campaign campaign, Campaign savedCampaign)
    {
        try
        {
            var selectedSave = ConsolePrompts.SelectFromList(
                SaveManager.GetSaveNames,
                saveName => saveName,
                "What campaign would you like to load?");
            if (selectedSave == null)
            {
                Console.WriteLine("Load cancelled.");
                return (campaign, savedCampaign);
            }

            var loadedCampaign = SaveManager.Load(selectedSave);
            return (loadedCampaign, loadedCampaign);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"[LOAD ERROR] {ex.Message}");
            return (campaign, savedCampaign);
        }
    }

    private static bool ExitConfirmationMenu(Campaign campaign, Campaign? savedCampaign)
    {
        if (!campaign.DiffersFrom(savedCampaign) || campaign.Characters.Count == 0) return true;
        Console.WriteLine("You haven't saved your current campaign. Are you sure you want to proceed?");
        return ConsolePrompts.ConfirmOperation();
    }
}

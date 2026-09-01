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
        var input = ConsoleUI.GetNewCharacterDetails(Character.ValidClasses);

        if (!Character.TryCreate(
                input.Name, input.CharacterClass, input.Hp, input.Ap,
                out var character, out var errorMessage))
        {
            Console.WriteLine($"Could not create character: {errorMessage}");
            return;
        }

        campaign.AddCharacter(character);
    }

    private static Campaign SaveCampaignMenu(Campaign campaign)
    {
        Console.WriteLine("What would you like to save the campaign as?");
        var input = Console.ReadLine();
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
            if(ConsoleUI.ConfirmOperation())
            {
                SaveManager.Save(campaign, input);
                return SaveManager.Load(input);
            }

            return campaign;
    }
    private static (Campaign Campaign, Campaign SavedCampaign) LoadCampaignMenu(Campaign campaign, Campaign savedCampaign)
    {
        while (true)
        {
            Console.WriteLine("What campaign would you like to load?");
            var saves = SaveManager.GetSaveNames();
            int i;
            for (i = 0; i < saves.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {saves[i]}");
            }

            Console.WriteLine($"{i + 1}. Cancel load.");
            var selectedFile = Console.ReadLine();
            if (int.TryParse(selectedFile, out var selectedFileNumber) &&
                selectedFileNumber > 0 &&
                selectedFileNumber <= saves.Length)
            {
                var loadedCampaign = SaveManager.Load(saves[selectedFileNumber - 1]);
                savedCampaign = SaveManager.Load(saves[selectedFileNumber - 1]);
                
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
        return ConsoleUI.ConfirmOperation();
    }
}

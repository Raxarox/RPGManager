using RPGManager.GameData.Campaigns;
using RPGManager.GameData.Characters;
using RPGManager.GameData.Items;
using RPGManager.System;

namespace RPGManager.UI;

public static class GameRunner
{
    public static void RunGame(GameAssetRegistry  assetRegistry)
    {
        Campaign campaign = new Campaign();
        campaign.EnableAllClasses(assetRegistry.Classes.Keys);
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
                    CreateCharacter(campaign, assetRegistry);
                    break;
                case "2":
                    if (campaign.Characters.Count == 0)
                    {
                        Console.WriteLine("Please create a character first.");
                        break;
                    }
                    CharacterEditor.Run(campaign, assetRegistry);
                    break;
                case "3":
                    // Reload the saved campaign to create an independent snapshot.
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
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }
    
    private static void CreateCharacter(Campaign campaign, GameAssetRegistry assetRegistry)
    {
        var input = PromptForNewCharacterDetails(campaign, assetRegistry.Classes);

        if (string.IsNullOrWhiteSpace(input.Name) || input.CharacterClass == null)
        {
            Console.WriteLine("Could not create character: Name and class must be provided.");
            return;
        }

        if (!Character.TryCreate(
                input.Name, input.CharacterClass, input.Hp,
                out var character, out var errorMessage))
        {
            Console.WriteLine($"Could not create character: {errorMessage}");
            return;
        }

        campaign.AddCharacter(character);
        Console.WriteLine($"Successfully created {character.Name} the {character.Class.Name}!");
    }

    private static (string? Name, CharacterClass? CharacterClass, int Hp) PromptForNewCharacterDetails(
        Campaign campaign, IReadOnlyDictionary<string, CharacterClass> masterClassRegistry)
    {
        Console.WriteLine("What is the character's name?");
        var name = Console.ReadLine();

        var classIds = campaign.AvailableClasses;
        Console.WriteLine("What is the character's class?");
        for (int i = 0; i < classIds.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {classIds[i]}");
        }

        CharacterClass? selectedClass = null;
        while (selectedClass == null)
        {
            Console.Write("Enter the number of the class: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= classIds.Count)
            {
                string selectedId = classIds[choice - 1];

                if (masterClassRegistry.TryGetValue(selectedId, out var foundClass))
                {
                    selectedClass = foundClass;
                }
                else
                {
                    Console.WriteLine($"Error: The class definition for '{selectedId}' could not be found in the registry.");
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number from the list.");
            }
        }

        var hp = ConsolePrompts.GetPositiveIntegerFor("HP");
        return (name, selectedClass, hp);
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
        if (campaign.Equals(savedCampaign) || campaign.Characters.Count == 0) return true;
        Console.WriteLine("You haven't saved your current campaign. Are you sure you want to proceed?");
        return ConsolePrompts.ConfirmOperation();
    }
}
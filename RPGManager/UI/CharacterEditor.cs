using RPGManager.GameData.Campaigns;
using RPGManager.GameData.Characters;
using RPGManager.System;

namespace RPGManager.UI;

public static class CharacterEditor
{
    public static void Run(Campaign campaign, GameAssetRegistry  assetRegistry)
    {
        var exit = false;
        while (!exit && campaign.Characters.Count > 0)
        {
            Console.WriteLine("CHARACTER EDITOR");
            Console.WriteLine("1. Edit Character");
            Console.WriteLine("2. Remove character");
            Console.WriteLine("3. Exit");
            var editMenuInput = Console.ReadLine();
            switch (editMenuInput)
            {
                case "1":
                    EditCharacter(campaign, assetRegistry);
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

    private static string FormatCharacter(Character character)
    {
        var scores = character.AbilityScores;
        return character.Name + " - " +
               character.Class.Name + " - " +
               character.MaxHealth + " Max HP - " +
               character.Health + " HP." +
               $"\nSTR: {scores.Strength} | DEX: {scores.Dexterity} | CON: {scores.Constitution} | " +
               $"INT: {scores.Intelligence} | WIS: {scores.Wisdom} | CHA: {scores.Charisma}";
    }

    private static void RemoveCharacter(Campaign campaign)
    {
        var character = SelectCharacter(campaign,
            "Which character would you like to remove? Please enter the character's ID");
        if (character is null) return;

        Console.WriteLine("Are you sure you want to remove " + character.Name + "?");
        Console.WriteLine(FormatCharacter(character));
        if (ConsolePrompts.ConfirmOperation()) campaign.RemoveCharacter(character);
    }

    private static void EditCharacter(Campaign campaign, GameAssetRegistry  assetRegistry)
    {
        var character = SelectCharacter(campaign,
            "Which character would you like to edit? Please enter the character's ID");
        if (character is null) return;

        var close = false;
        while (!close)
        {
            Console.WriteLine(FormatCharacter(character));
            Console.WriteLine("What would you like to edit?");
            Console.WriteLine("1. Edit Name.");
            Console.WriteLine("2. Edit Class.");
            Console.WriteLine("3. Edit Max HP.");
            Console.WriteLine("4. Edit Ability Scores.");
            Console.WriteLine("5. Cancel.");
            var input = Console.ReadLine();
            switch (input)
            {
                case "1": EditName(character); close = true; break;
                case "2": EditClass(character, campaign, assetRegistry.Classes); close = true; break;
                case "3": EditMaxHp(character); close = true; break;
                case "4": EditAbilityScores(character); close = true; break;
                case "5": close = true; break;
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

    private static void EditClass(Character character, Campaign campaign, IReadOnlyDictionary<string, CharacterClass> masterClassRegistry)
    {
        var classList = campaign.AvailableClasses;

        Console.WriteLine($"What would you like to change {character.Name}'s class to?");
        for (int i = 0; i < classList.Count; i++)
        {
            // If you want it to look pretty (e.g., capitalizing the ID), you can format it here
            Console.WriteLine($"{i + 1}. {classList[i]}");
        }
    
        CharacterClass? selectedClass = null;
        while (selectedClass == null)
        {
            Console.Write("Enter the number of the new class: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= classList.Count)
            {
                string selectedId = classList[choice - 1];

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

        character.SetCharacterClass(selectedClass);
        Console.WriteLine($"{character.Name}'s class has been updated to {character.Class.Name}.");
    }

    private static void EditMaxHp(Character character)
    {
        Console.WriteLine("What would you like to change " + character.Name + "'s max HP to?");
        character.SetMaxHealth(ConsolePrompts.GetPositiveIntegerFor("max HP"));
        Console.WriteLine("Character's max HP changed to '" + character.MaxHealth + "' successfully");
    }

    private static void EditAbilityScores(Character character)
    {
        var exit = false;
        while (!exit)
        {
            var scores = character.AbilityScores;
            Console.WriteLine("ABILITY SCORES");
            Console.WriteLine($"1. Strength: {scores.Strength}");
            Console.WriteLine($"2. Dexterity: {scores.Dexterity}");
            Console.WriteLine($"3. Constitution: {scores.Constitution}");
            Console.WriteLine($"4. Intelligence: {scores.Intelligence}");
            Console.WriteLine($"5. Wisdom: {scores.Wisdom}");
            Console.WriteLine($"6. Charisma: {scores.Charisma}");
            Console.WriteLine("7. Return to character editor.");

            switch (Console.ReadLine())
            {
                case "1": EditAbilityScore("Strength", scores.SetStrength); break;
                case "2": EditAbilityScore("Dexterity", scores.SetDexterity); break;
                case "3": EditAbilityScore("Constitution", scores.SetConstitution); break;
                case "4": EditAbilityScore("Intelligence", scores.SetIntelligence); break;
                case "5": EditAbilityScore("Wisdom", scores.SetWisdom); break;
                case "6": EditAbilityScore("Charisma", scores.SetCharisma); break;
                case "7": exit = true; break;
                default: Console.WriteLine("Invalid input."); break;
            }
        }
    }

    private static void EditAbilityScore(string scoreName, Func<int, bool> setScore)
    {
        var value = ConsolePrompts.GetPositiveIntegerFor(scoreName);
        setScore(value);
        Console.WriteLine($"{scoreName} changed to '{value}' successfully");
    }

    private static IReadOnlyList<Character> SortCharacters(IReadOnlyList<Character> characters)
    {
        Console.WriteLine("What would you like to sort the characters by?");
        Console.WriteLine("1. Name.");
        Console.WriteLine("2. Class.");
        Console.WriteLine("3. HP.");
        Console.WriteLine("4. Ability Scores.");
        Console.WriteLine("5. Cancel.");
        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                return characters.OrderBy(c => c.Name).ToList();
            case "2":
                return characters.OrderBy(c => c.Class.Name).ToList();
            case "3":
                return characters.OrderBy(c => c.MaxHealth).ToList();
            case "4":
                return SortByAbilityScore(characters);
            case "5":
                return characters;
            default:
                Console.WriteLine("Invalid input. Sorting by default.");
                return characters;
        }
    }

    private static IReadOnlyList<Character> SortByAbilityScore(IReadOnlyList<Character> characters)
    {
        Console.WriteLine("What ability score would you like to sort the characters by?");
        Console.WriteLine("1. STR.");
        Console.WriteLine("2. DEX.");
        Console.WriteLine("3. CON.");
        Console.WriteLine("4. INT.");
        Console.WriteLine("5. WSD.");
        Console.WriteLine("6. CHA.");
        Console.WriteLine("7. Cancel.");
        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                return characters.OrderBy(c => c.AbilityScores.Strength).ToList();
            case "2":
                return characters.OrderBy(c => c.AbilityScores.Dexterity).ToList();
            case "3":
                return characters.OrderBy(c => c.AbilityScores.Constitution).ToList();
            case "4":
                return characters.OrderBy(c => c.AbilityScores.Intelligence).ToList();
            case "5":
                return characters.OrderBy(c => c.AbilityScores.Wisdom).ToList();
            case "6":
                return characters.OrderBy(c => c.AbilityScores.Charisma).ToList();
            case "7":
                return characters;
            default:
                Console.WriteLine("Invalid input. Sorting by default.");
                return characters;
        }
    }

    private static Character? SelectCharacter(Campaign campaign, string prompt)
    {
        IReadOnlyList<Character> displayedCharacters = campaign.Characters;

        return ConsolePrompts.SelectFromList(
            () => displayedCharacters,
            FormatCharacter,
            prompt,
            "Sort characters",
            () => displayedCharacters = SortCharacters(displayedCharacters));
    }
}

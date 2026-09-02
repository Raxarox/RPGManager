namespace RPGManager;

public static class ConsolePrompts
{
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
    
    public static T? SelectFromList<T>(
        Func<IReadOnlyList<T>> getItems,
        Func<T, string> display,
        string prompt,
        string? specialOptionLabel = null,
        Action? onSpecialOption = null)
    {
        while (true)
        {
            var items = getItems();

            Console.WriteLine(prompt);

            for (var i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"ID: {i}. {display(items[i])}");
            }

            var cancelIndex = items.Count;

            if (specialOptionLabel is not null)
            {
                Console.WriteLine($"{items.Count}. {specialOptionLabel}");
                cancelIndex++;
            }

            Console.WriteLine($"{cancelIndex}. Cancel.");

            var input = Console.ReadLine();

            if (int.TryParse(input, out var result) &&
                result >= 0 &&
                result < items.Count)
            {
                return items[result];
            }

            if (specialOptionLabel is not null &&
                result == items.Count)
            {
                onSpecialOption?.Invoke();
                continue;
            }

            if (result == cancelIndex)
                return default;

            Console.WriteLine("Invalid input.");
        }
    }

}

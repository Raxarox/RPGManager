using RPGManager.GameData.Characters;
using RPGManager.GameData.Items;

namespace RPGManager.System;

public class GameAssetRegistry(
    IReadOnlyDictionary<string, CharacterClass> classes,
    IReadOnlyDictionary<string, Item> items)
{
    public IReadOnlyDictionary<string, CharacterClass> Classes { get; init; } = classes;
    public IReadOnlyDictionary<string, Item> Items { get; init; } = items;
}
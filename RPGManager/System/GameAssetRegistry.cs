using RPGManager.GameData.Characters;
using RPGManager.GameData.Items;

namespace RPGManager.System;

public class GameAssetRegistry(
    IReadOnlyDictionary<string, CharacterClass> classes,
    IReadOnlyDictionary<string, Item> items,
    IReadOnlyDictionary<string, Archetype> archetypes)
{
    public IReadOnlyDictionary<string, CharacterClass> Classes { get; init; } = classes;
    public IReadOnlyDictionary<string, Item> Items { get; init; } = items;
    public IReadOnlyDictionary<string, Archetype> Archetypes { get; init; } = archetypes;
}
using RPGManager.GameData.Characters;
using RPGManager.GameData.Items;
using RPGManager.System;

namespace RPGManager.Tests.System;

public class GameAssetRegistryTest
{
    [Fact]
    public void Constructor_ValidParameters_CreatesRegistry()
    {
        var classes = new Dictionary<string, CharacterClass>
        {
            { "fighter", new CharacterClass("fighter", "Fighter", 10, [], [], [], [], 0) }
        };
        var items = new Dictionary<string, Item>
        {
            { "sword", new Weapon("sword", "Longsword", 3, 0, "A standard longsword", "1d8", "Slashing", WeaponCategory.Martial, WeaponAttackType.Melee, WeaponHandedness.SingleHanded) }
        };
        var archetypes = new Dictionary<string, Archetype>
        {
            { "champion", new Archetype("champion", "Champion", "fighter", []) }
        };

        var registry = new GameAssetRegistry(classes, items, archetypes);

        Assert.Single(registry.Classes);
        Assert.Single(registry.Items);
        Assert.Single(registry.Archetypes);
    }

    [Fact]
    public void Classes_PreservesDictionary()
    {
        var classes = new Dictionary<string, CharacterClass>
        {
            { "fighter", new CharacterClass("fighter", "Fighter", 10, [], [], [], [], 0) },
            { "wizard", new CharacterClass("wizard", "Wizard", 6, [], [], [], [], 0) }
        };
        var registry = new GameAssetRegistry(classes, new Dictionary<string, Item>(), new Dictionary<string, Archetype>());

        Assert.Equal(2, registry.Classes.Count);
        Assert.True(registry.Classes.ContainsKey("fighter"));
        Assert.True(registry.Classes.ContainsKey("wizard"));
    }

    [Fact]
    public void Items_PreservesDictionary()
    {
        var items = new Dictionary<string, Item>
        {
            { "sword", new Weapon("sword", "Longsword", 3, 0, "A standard longsword", "1d8", "Slashing", WeaponCategory.Martial, WeaponAttackType.Melee, WeaponHandedness.SingleHanded) },
            { "shield", new Armor("shield", "Shield", 6, 0, "A wooden shield", 2, ArmorType.Shield) }
        };
        var registry = new GameAssetRegistry(new Dictionary<string, CharacterClass>(), items, new Dictionary<string, Archetype>());

        Assert.Equal(2, registry.Items.Count);
        Assert.True(registry.Items.ContainsKey("sword"));
        Assert.True(registry.Items.ContainsKey("shield"));
    }

    [Fact]
    public void Archetypes_PreservesDictionary()
    {
        var archetypes = new Dictionary<string, Archetype>
        {
            { "champion", new Archetype("champion", "Champion", "fighter", []) },
            { "evocation", new Archetype("evocation", "Evocation School", "wizard", []) }
        };
        var registry = new GameAssetRegistry(new Dictionary<string, CharacterClass>(), new Dictionary<string, Item>(), archetypes);

        Assert.Equal(2, registry.Archetypes.Count);
        Assert.True(registry.Archetypes.ContainsKey("champion"));
        Assert.True(registry.Archetypes.ContainsKey("evocation"));
    }

    [Fact]
    public void Constructor_EmptyDictionaries_CreatesEmptyRegistry()
    {
        var registry = new GameAssetRegistry(
            new Dictionary<string, CharacterClass>(),
            new Dictionary<string, Item>(),
            new Dictionary<string, Archetype>());

        Assert.Empty(registry.Classes);
        Assert.Empty(registry.Items);
        Assert.Empty(registry.Archetypes);
    }
}

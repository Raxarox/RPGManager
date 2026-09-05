using RPGManager.GameData.Items;

namespace RPGManager.Tests.Items;

public class WeaponTest
{
    [Fact]
    public void Constructor_LightButTwoHanded_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => 
            new Weapon("sword", "Longsword", 3m, 150, "A versatile sword", "1d8", "Slashing", 
                WeaponCategory.Martial, WeaponAttackType.Melee, WeaponHandedness.TwoHanded, true));
    }

    [Fact]
    public void Constructor_EmptyDamageDice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => 
            new Weapon("sword", "Longsword", 3m, 150, "A versatile sword", "", "Slashing", 
                WeaponCategory.Martial, WeaponAttackType.Melee, WeaponHandedness.SingleHanded, false));
    }

    [Fact]
    public void Constructor_NullDamageDice_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => 
            new Weapon("sword", "Longsword", 3m, 150, "A versatile sword", null!, "Slashing", 
                WeaponCategory.Martial, WeaponAttackType.Melee, WeaponHandedness.SingleHanded, false));
    }

    [Fact]
    public void Constructor_NullDamageType_UsesDefaultSlashing()
    {
        var weapon = new Weapon("sword", "Longsword", 3m, 150, "A versatile sword", "1d8", null!, 
            WeaponCategory.Martial, WeaponAttackType.Melee, WeaponHandedness.SingleHanded, false);

        Assert.Equal("Slashing", weapon.DamageType);
    }

    [Fact]
    public void Constructor_ValidParameters_CreatesWeaponWithCorrectProperties()
    {
        var weapon = new Weapon("longsword", "Longsword", 3m, 150, "A versatile martial weapon",
            "1d8", "Slashing", WeaponCategory.Martial, WeaponAttackType.Melee,
            WeaponHandedness.SingleHanded, false);

        Assert.Equal("longsword", weapon.TemplateId);
        Assert.Equal("Longsword", weapon.Name);
        Assert.Equal(3m, weapon.Weight);
        Assert.Equal(150, weapon.ValueInCopper);
        Assert.Equal("1d8", weapon.DamageDice);
        Assert.Equal("Slashing", weapon.DamageType);
        Assert.Equal(WeaponCategory.Martial, weapon.Category);
        Assert.Equal(WeaponAttackType.Melee, weapon.AttackType);
        Assert.Equal(WeaponHandedness.SingleHanded, weapon.Handedness);
        Assert.False(weapon.IsLight);
    }

    [Fact]
    public void Constructor_LightSingleHanded_DoesNotThrow()
    {
        var weapon = new Weapon("dagger", "Dagger", 1m, 20, "A small light weapon", "1d4", "Piercing",
            WeaponCategory.Simple, WeaponAttackType.Melee, WeaponHandedness.SingleHanded, true);

        Assert.True(weapon.IsLight);
        Assert.Equal(WeaponHandedness.SingleHanded, weapon.Handedness);
    }

    [Fact]
    public void Constructor_TwoHandedNotLight_DoesNotThrow()
    {
        var weapon = new Weapon("greatsword", "Greatsword", 6m, 200, "A heavy two-handed weapon",
            "2d6", "Slashing", WeaponCategory.Martial, WeaponAttackType.Melee,
            WeaponHandedness.TwoHanded, false);

        Assert.Equal(WeaponHandedness.TwoHanded, weapon.Handedness);
        Assert.False(weapon.IsLight);
    }

    [Fact]
    public void Constructor_RangedWeapon_CreatesCorrectly()
    {
        var weapon = new Weapon("longbow", "Longbow", 2m, 250, "A ranged weapon", "1d8", "Piercing",
            WeaponCategory.Martial, WeaponAttackType.Ranged, WeaponHandedness.TwoHanded, false);

        Assert.Equal(WeaponAttackType.Ranged, weapon.AttackType);
        Assert.Equal(WeaponHandedness.TwoHanded, weapon.Handedness);
    }

    [Fact]
    public void Constructor_SimpleWeapon_CreatesCorrectly()
    {
        var weapon = new Weapon("club", "Club", 2m, 10, "A simple club", "1d6", "Bludgeoning",
            WeaponCategory.Simple, WeaponAttackType.Melee, WeaponHandedness.SingleHanded, false);

        Assert.Equal(WeaponCategory.Simple, weapon.Category);
    }

    [Fact]
    public void InheritsFromItem_HasBaseProperties()
    {
        var weapon = new Weapon("sword", "Longsword", 3m, 150, "A versatile sword", "1d8", "Slashing",
            WeaponCategory.Martial, WeaponAttackType.Melee, WeaponHandedness.SingleHanded, false);

        Assert.IsAssignableFrom<Item>(weapon);
        Assert.Equal("sword", weapon.TemplateId);
        Assert.Equal("Longsword", weapon.Name);
        Assert.Equal(3m, weapon.Weight);
        Assert.Equal(150, weapon.ValueInCopper);
    }
}

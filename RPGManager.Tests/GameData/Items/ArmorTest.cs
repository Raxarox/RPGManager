using RPGManager.GameData.Items;

namespace RPGManager.Tests.Items;

public class ArmorTest
{
    [Fact]
    public void Constructor_NegativeArmorClassBonus_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            new Armor("plate", "Plate Armor", 65m, 1500, "Heavy armor", -1, ArmorType.Heavy));
    }

    [Fact]
    public void Constructor_ZeroArmorClassBonus_Allowed()
    {
        var armor = new Armor("padded", "Padded Armor", 8m, 50, "Light armor", 0, ArmorType.Light);
        Assert.Equal(0, armor.ArmorClassBonus);
    }

    [Fact]
    public void Constructor_ValidParameters_CreatesArmorWithCorrectProperties()
    {
        var armor = new Armor("plate", "Plate Armor", 65m, 1500, "Heavy metal armor", 8, ArmorType.Heavy);

        Assert.Equal("plate", armor.TemplateId);
        Assert.Equal("Plate Armor", armor.Name);
        Assert.Equal(65m, armor.Weight);
        Assert.Equal(1500, armor.ValueInCopper);
        Assert.Equal("Heavy metal armor", armor.Description);
        Assert.Equal(8, armor.ArmorClassBonus);
        Assert.Equal(ArmorType.Heavy, armor.ArmorType);
    }

    [Fact]
    public void Constructor_LightArmor_CreatesCorrectly()
    {
        var armor = new Armor("leather", "Leather Armor", 10m, 100, "Light protective armor", 1, ArmorType.Light);

        Assert.Equal(ArmorType.Light, armor.ArmorType);
        Assert.Equal(1, armor.ArmorClassBonus);
    }

    [Fact]
    public void Constructor_MediumArmor_CreatesCorrectly()
    {
        var armor = new Armor("chainmail", "Chain Shirt", 20m, 400, "Medium armor", 3, ArmorType.Medium);

        Assert.Equal(ArmorType.Medium, armor.ArmorType);
        Assert.Equal(3, armor.ArmorClassBonus);
    }

    [Fact]
    public void Constructor_HeavyArmor_CreatesCorrectly()
    {
        var armor = new Armor("plate", "Plate Armor", 65m, 1500, "Heavy armor", 8, ArmorType.Heavy);

        Assert.Equal(ArmorType.Heavy, armor.ArmorType);
        Assert.Equal(8, armor.ArmorClassBonus);
    }

    [Fact]
    public void Constructor_Shield_CreatesCorrectly()
    {
        var armor = new Armor("shield", "Shield", 6m, 100, "A protective shield", 2, ArmorType.Shield);

        Assert.Equal(ArmorType.Shield, armor.ArmorType);
        Assert.Equal(2, armor.ArmorClassBonus);
    }

    [Fact]
    public void Constructor_HighArmorClassBonus_Allowed()
    {
        var armor = new Armor("fullplate", "Full Plate", 65m, 2000, "Maximum protection", 10, ArmorType.Heavy);
        Assert.Equal(10, armor.ArmorClassBonus);
    }

    [Fact]
    public void InheritsFromItem_HasBaseProperties()
    {
        var armor = new Armor("plate", "Plate Armor", 65m, 1500, "Heavy armor", 8, ArmorType.Heavy);

        Assert.IsAssignableFrom<Item>(armor);
        Assert.Equal("plate", armor.TemplateId);
        Assert.Equal("Plate Armor", armor.Name);
        Assert.Equal(65m, armor.Weight);
        Assert.Equal(1500, armor.ValueInCopper);
    }

    [Fact]
    public void Constructor_NullDescription_DoesNotThrowAndUsesEmptyString()
    {
        var armor = new Armor("plate", "Plate Armor", 65m, 1500, null!, 8, ArmorType.Heavy);
        Assert.Equal(string.Empty, armor.Description);
    }

    [Fact]
    public void Constructor_EmptyDescription_DoesNotThrow()
    {
        var armor = new Armor("plate", "Plate Armor", 65m, 1500, "", 8, ArmorType.Heavy);
        Assert.Equal(string.Empty, armor.Description);
    }
}

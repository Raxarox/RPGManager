using RPGManager.GameData.Items;

namespace RPGManager.Tests.Items;

public class ItemTest
{
    [Fact]
    public void Constructor_NegativeWeight_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            new TestItem("test", "Test Item", -1m, 100, "Test description"));
    }

    [Fact]
    public void Constructor_NegativeValue_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            new TestItem("test", "Test Item", 1m, -100, "Test description"));
    }

    [Fact]
    public void Constructor_NullTemplateId_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new TestItem(null!, "Test Item", 1m, 100, "Test description"));
    }

    [Fact]
    public void Constructor_NullName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => 
            new TestItem("test", null!, 1m, 100, "Test description"));
    }

    [Fact]
    public void Constructor_NullDescription_DoesNotThrowAndUsesEmptyString()
    {
        var item = new TestItem("test", "Test Item", 1m, 100, null!);
        Assert.Equal(string.Empty, item.Description);
    }

    [Fact]
    public void Constructor_ValidParameters_CreatesItemWithCorrectProperties()
    {
        var item = new TestItem("test_id", "Test Item", 2.5m, 500, "A test item");

        Assert.Equal("test_id", item.TemplateId);
        Assert.Equal("Test Item", item.Name);
        Assert.Equal(2.5m, item.Weight);
        Assert.Equal(500, item.ValueInCopper);
        Assert.Equal("A test item", item.Description);
    }

    [Fact]
    public void Constructor_ZeroWeight_Allowed()
    {
        var item = new TestItem("test", "Test Item", 0m, 100, "Test description");
        Assert.Equal(0m, item.Weight);
    }

    [Fact]
    public void Constructor_ZeroValue_Allowed()
    {
        var item = new TestItem("test", "Test Item", 1m, 0, "Test description");
        Assert.Equal(0, item.ValueInCopper);
    }

    // Helper class for testing the abstract Item class
    private class TestItem(string templateId, string name, decimal weight, int valueInCopper, string description)
        : Item(templateId, name, weight, valueInCopper, description);
}

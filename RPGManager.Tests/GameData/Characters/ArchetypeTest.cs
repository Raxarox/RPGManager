using RPGManager.GameData.Characters;

namespace RPGManager.Tests.GameData.Characters;

public class ArchetypeTest
{
    [Fact]
    public void Constructor_ValidParameters_CreatesArchetype()
    {
        var archetype = new Archetype(
            "champion",
            "Champion",
            "fighter",
            []);

        Assert.Equal("champion", archetype.ArchetypeId);
        Assert.Equal("Champion", archetype.Name);
        Assert.Equal("fighter", archetype.ParentClassId);
        Assert.Empty(archetype.Progression);
    }

    [Fact]
    public void Constructor_WithProgression_PreservesProgression()
    {
        var progression = new List<ClassLevelData>
        {
            new ClassLevelData(3, 2, ["Improved Critical"], [], []),
            new ClassLevelData(7, 3, ["Remarkable Athlete"], [], [])
        };
        var archetype = new Archetype("champion", "Champion", "fighter", progression);

        Assert.Equal(2, archetype.Progression.Count);
        Assert.Equal("Improved Critical", archetype.Progression[0].ClassFeatures[0]);
    }

    [Fact]
    public void Constructor_NullProgression_DefaultsToEmptyList()
    {
        var archetype = new Archetype("champion", "Champion", "fighter", null);

        Assert.Empty(archetype.Progression);
    }

    [Fact]
    public void Constructor_EmptyArchetypeId_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Archetype("", "Champion", "fighter", []));
    }

    [Fact]
    public void Constructor_NullName_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Archetype("champion", null!, "fighter", []));
    }

    [Fact]
    public void Constructor_EmptyName_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new Archetype("champion", null!, "fighter", []));
    }

    [Fact]
    public void Constructor_EmptyParentClassId_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            new Archetype("champion", "Champion", "", []));
    }

    [Fact]
    public void ToString_ReturnsName()
    {
        var archetype = new Archetype("champion", "Champion", "fighter", []);
        Assert.Equal("Champion", archetype.ToString());
    }

    [Fact]
    public void ParentClassId_PreservesValue()
    {
        var archetype = new Archetype("evocation", "Evocation School", "wizard", []);
        Assert.Equal("wizard", archetype.ParentClassId);
    }

    [Fact]
    public void ArchetypeId_PreservesValue()
    {
        var archetype = new Archetype("battle-master", "Battle Master", "fighter", []);
        Assert.Equal("battle-master", archetype.ArchetypeId);
    }
}

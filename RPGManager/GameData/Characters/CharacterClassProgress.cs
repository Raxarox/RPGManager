using System.Text.Json.Serialization;

namespace RPGManager.GameData.Characters;

public class CharacterClassProgress : IEquatable<CharacterClassProgress>
{
    private CharacterClass? _classAsset;
    private Archetype? _archetypeAsset;

    // Preserve ClassId during serialization for asset re-linking
    public string ClassId { get; set; } = string.Empty;

    // Preserve ArchetypeId the same way, for the same reason.
    public string ArchetypeId { get; set; } = string.Empty;

    // Use a backing field or regular property so it can be assigned during Load
    [JsonInclude]
    public CharacterClass? ClassAsset
    {
        get => _classAsset;
        set
        {
            _classAsset = value;
            if (value != null)
            {
                ClassId = value.ClassId;
            }
        }
    }

    [JsonInclude]
    public Archetype? ArchetypeAsset
    {
        get => _archetypeAsset;
        set
        {
            _archetypeAsset = value;
            if (value != null)
            {
                ArchetypeId = value.ArchetypeId;
            }
        }
    }

    public int Level { get; set; }

    [JsonConstructor]
    public CharacterClassProgress(
        CharacterClass? classAsset,
        int level,
        string classId = "",
        Archetype? archetypeAsset = null,
        string archetypeId = "")
    {
        _classAsset = classAsset;
        Level = Math.Max(1, level);
        ClassId = classAsset?.ClassId ?? classId;
        _archetypeAsset = archetypeAsset;
        ArchetypeId = archetypeAsset?.ArchetypeId ?? archetypeId;
    }

    // Attempts to select an archetype for this class progress. Fails if:
    // - this class hasn't reached its archetype choice level yet
    // - the archetype doesn't belong to this class
    // - the archetype isn't in this class's list of valid choices
    public bool TrySelectArchetype(Archetype archetype)
    {
        if (ClassAsset is null)
            return false;

        if (ClassAsset.ArchetypeChoiceLevel <= 0 || Level < ClassAsset.ArchetypeChoiceLevel)
            return false;

        if (archetype.ParentClassId != ClassAsset.ClassId)
            return false;

        if (!ClassAsset.AvailableArchetypeIds.Contains(archetype.ArchetypeId, StringComparer.OrdinalIgnoreCase))
            return false;

        ArchetypeAsset = archetype;
        return true;
    }

    public bool Equals(CharacterClassProgress? other)
    {
        if (other is null) return false;
        return ClassId == other.ClassId && Level == other.Level && ArchetypeId == other.ArchetypeId;
    }

    public override bool Equals(object? obj) => obj is CharacterClassProgress other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(ClassId, Level, ArchetypeId);
}
using System.Text.Json.Serialization;

namespace RPGManager.CharacterClasses;

[JsonDerivedType(typeof(Fighter), "Fighter")]
[JsonDerivedType(typeof(Wizard), "Wizard")]
[JsonDerivedType(typeof(Rogue), "Rogue")]
public abstract class CharacterClass
{
    public abstract string Name { get;  }
    public abstract int HitDieValue { get; }
    public abstract bool IsSpellcaster { get; }

    public override string ToString() => Name;
}
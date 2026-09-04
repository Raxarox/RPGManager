namespace RPGManager.CharacterClasses;

public class Rogue : CharacterClass
{
    public override string Name => "Rogue";
    public override int HitDieValue  => 8;
    public override bool IsSpellcaster => false;
}
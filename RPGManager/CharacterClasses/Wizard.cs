namespace RPGManager.CharacterClasses;

public class Wizard : CharacterClass
{
    public override string Name => "Wizard";
    public override int HitDieValue  => 6;
    public override bool IsSpellcaster => true;
}
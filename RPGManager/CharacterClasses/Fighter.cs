namespace RPGManager.CharacterClasses;

public class Fighter : CharacterClass
{
    public override string Name => "Fighter";
    public override int HitDieValue => 10;
    public override bool IsSpellcaster => false;
}
using UnityEngine;

[CreateAssetMenu(fileName = "DefenseEffect", menuName = "Card Effect/DefenseEffect")]
public class DefenseEffect : Effect
{
    public override void Execute(CharacterBase from,CharacterBase target)
    {
        if(effectType == EffectType.Self)
        {
            from.UpdateDefense(value);
        }

        
        if(effectType == EffectType.Target)
        {
            target.UpdateDefense(value);
        }

        
        
    }
}

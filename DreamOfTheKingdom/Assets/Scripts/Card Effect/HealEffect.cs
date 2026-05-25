using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effect/HealEffect")]
public class HealEffect :  Effect
{
    public override void Execute(CharacterBase from,CharacterBase target)
    {
        
        if(effectType == EffectType.Self)
        {
            from.HealHealth(value);
        }

        
        if(effectType == EffectType.Target)
        {
            target.HealHealth(value);
        }
        
    }
}

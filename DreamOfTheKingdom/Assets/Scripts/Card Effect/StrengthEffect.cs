using UnityEngine;

[CreateAssetMenu(fileName = "StrengthEffect", menuName = "Card Effect/StrengthEffect")]
public class StrengthEffect : Effect
{
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        switch(effectType)
        {
            case EffectType.Self:
                from.SetupStrength(value,true);
                break;
            case EffectType.Target:
                target.SetupStrength(value,false);
                break;
            case EffectType.All:

                break;
                        
        }
    }
}

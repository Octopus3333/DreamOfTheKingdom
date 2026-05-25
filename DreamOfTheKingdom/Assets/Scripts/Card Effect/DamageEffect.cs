using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effect/DamageEffect")]
public class DamageEffect : Effect
{
    public override void Execute(CharacterBase from,CharacterBase target)
    {
        if(target == null) 
        {
            Debug.Log("目标为空");
            return;
        }
        
        switch(effectType)
        {
            case EffectType.Target:
                var damage = (int)math.round(value * from.baseStrength);
                target.TakeDamage(damage);
                Debug.Log($"执行了{damage}点伤害");
                break;
            case EffectType.All:
                foreach(var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<CharacterBase>().TakeDamage(value);
                }
                break;
        }
    }
}

using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public int value;
    public EffectType effectType;

    public abstract void Execute(CharacterBase from,CharacterBase target);
}

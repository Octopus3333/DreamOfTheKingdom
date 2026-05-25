using UnityEngine;

[CreateAssetMenu(fileName = "DrawCardEffect", menuName = "Card Effect/DrawCardEffect")]
public class DrawCardEffect : Effect
{
    public IntEventSO drawCountEvent;
    public override void Execute(CharacterBase from, CharacterBase target)
    {
        drawCountEvent?.RaisEvent(value,this);
    }
}

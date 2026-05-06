using UnityEngine;

[CreateAssetMenu(fileName = "CardDataSO", menuName = "Card/CardDataSO")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public Sprite CardImage;
    public int cost;
    public CardType cardType;

    [TextArea]
    public string description;

    //TODO：执行的实际效果
}
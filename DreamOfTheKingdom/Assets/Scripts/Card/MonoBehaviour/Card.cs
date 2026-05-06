using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText,descriptionText,typeText,cardNameText; 

    public CardDataSO cardData;

    private void Start()
    {
        Init(cardData);
    }

    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = cardData.CardImage;
        costText.text = cardData.cost.ToString();
        descriptionText.text = cardData.description;
        cardNameText.text = cardData.cardName;

        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotFiniteNumberException(),
        };
    }
}

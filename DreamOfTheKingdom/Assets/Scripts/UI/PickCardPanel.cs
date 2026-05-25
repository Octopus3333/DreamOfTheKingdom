using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public CardManager cardManager;
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer;
    private CardDataSO currentCardData;

    private Button confirmButton;

    [Header("事件广播")]
    public ObjectEventSO finishPickCardEvent;

    private List<Button> cardButtons = new();

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");

        confirmButton.clicked += OnConfirmButtonClicked;

        for(int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = cardManager.GetNewCardData();
            //初始化
            InitCard(card,data);
            var cardButton = card.Q<Button>("Card");
            cardContainer.Add(card);
            cardButtons.Add(cardButton);

            cardButton.clicked += () => OnCardClicked(cardButton,data);

        }
    }

    private void OnConfirmButtonClicked()
    {
        cardManager.UnlockCard(currentCardData);
        finishPickCardEvent.RaisEvent(null,this);
    }

    private void OnCardClicked(Button cardButton,CardDataSO data)
    {
        currentCardData = data;
        Debug.Log("选中的卡牌：" + currentCardData.cardName);
        for(int i = 0; i < cardButtons.Count; i++)
        {
            if(cardButtons[i] == cardButton)
                    cardButtons[i].SetEnabled(false);
                else
                    cardButtons[i].SetEnabled(true);
        }

    }

    public void InitCard(VisualElement card,CardDataSO cardData)
    {
        card.dataSource = cardData;

        var cardSpriteElement = card.Q<VisualElement>("CardSprite");
        var cardCost = card.Q<Label>("EnergyCost");
        var cardDescription = card.Q<Label>("CardDescription");
        var cardType = card.Q<Label>("CardType");
        var cardName = card.Q<Label>("CardName");

        cardSpriteElement.style.backgroundImage = new StyleBackground(cardData.CardImage);
        cardName.text = cardData.cardName;
        cardCost.text = cardData.cost.ToString();
        cardDescription.text = cardData.description;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Skill => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotFiniteNumberException(),
        };
    }

    
}

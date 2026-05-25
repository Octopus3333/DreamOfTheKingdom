using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("组件")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText,descriptionText,typeText,cardNameText; 

    public CardDataSO cardData;

    [Header("原始卡牌位置数据")]
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originalLayerOrder;


    public bool isAnimating ;//卡牌是否正在动画中
    public bool isCanUse;//卡牌是否可以被使用

    public Player player;

    [Header("广播事件")]
    public ObjectEventSO discardCardEvent;
    public IntEventSO costEvent;

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
            CardType.Skill => "技能",
            CardType.Abilities => "能力",
            _ => throw new System.NotFiniteNumberException(),
        };

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    /// <summary>
    /// 保存卡片位置和旋转,以便于拖拽卡牌后恢复原位和排序
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void UpdatePositionRotation(Vector3 position, Quaternion rotation)
    {
        originalPosition = position;
        originalRotation = rotation;
        originalLayerOrder = GetComponent<SortingGroup>().sortingOrder;    
    }

    /// <summary>
    /// 鼠标进入卡牌时，恢复卡牌原始位置和排序，使其能完全显示
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isAnimating) return;
        transform.position = originalPosition + Vector3.up;
        transform.rotation = Quaternion.identity;
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    /// <summary>
    /// 鼠标离开卡牌时，恢复卡牌在手牌中之前的位置和排序
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if(isAnimating) return;
        RestCardTransform();
    }

    public void RestCardTransform()
    {
        transform.SetPositionAndRotation(originalPosition,originalRotation);
        GetComponent<SortingGroup>().sortingOrder = originalLayerOrder;
    }

    public void ExecuteEffects(CharacterBase from,CharacterBase target)
    {
        //减少卡牌费用 通知回收事件
        costEvent.RaisEvent(cardData.cost,this);
        discardCardEvent.RaisEvent(this,this);
        //遍历卡牌效果列表，执行每个效果
        foreach(var effect in cardData.effects)
        {
            effect.Execute(from,target);
        }
    }

    public void UpdateCardState()
    {
        isCanUse = cardData.cost <= player.CurrentMana;
        costText.color = isCanUse ? Color.green : Color.red;
    }
}
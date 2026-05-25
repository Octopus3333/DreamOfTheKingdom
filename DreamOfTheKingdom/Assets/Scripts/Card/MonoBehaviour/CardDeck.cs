using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using DG.Tweening;
public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager layoutManager;

    public Vector3 deckPosition;//抽牌堆位置

    private List<CardDataSO> drawDeck = new();//抽牌堆
    private List<CardDataSO> discardDeck = new();//弃牌堆
    private List<Card> handCardObjectList = new();//当前手牌

    [Header("事件广播")]
    public IntEventSO drawCountEvent;
    public IntEventSO discardCountEvent;
    

    //TODO：测试用
    private void Start()
    {
        InitializeDeck();
    }

    /// <summary>
    /// 根据当前卡牌库重建抽牌堆；进入新房间或战斗结束时会调用，须先清空运行时牌堆避免重复累加。
    /// </summary>
    public void InitializeDeck()
    {
        drawDeck.Clear();
        discardDeck.Clear();

        foreach(var entry in cardManager.currentLibrary.cardLibraryList)
        {
            for(int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }
        //洗牌/更新抽牌堆and弃牌堆显示数字
        ShuffleDeck();
    }

    [ContextMenu("测试抽牌")]
    public void Test()
    {
        DrawCard(1);
    }

    /// <summary>
    /// 事件监听函数，进入玩家回合时触发
    /// </summary>
    public void NewturnDrawCards()
    {
        DrawCard(4);
    }

    public void DrawCard(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            CardDataSO cardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            if(drawDeck.Count == 0)
            {
                //洗牌/更新抽牌堆and弃牌堆显示数字
                foreach(var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }

            drawCountEvent.RaisEvent(drawDeck.Count,this);
            
            var card = cardManager.GetCardObject().GetComponent<Card>();
            //根据抽取的卡牌数据初始化卡牌对象
            card.Init(cardData);
            card.transform.position = deckPosition;

            handCardObjectList.Add(card);
            var delay = 0.2f * i;//延迟缩放动画时间
            SetCardLayout(delay);
        }
    }

    /// <summary>
    /// 设置手牌布局
    /// </summary>
    private void SetCardLayout(float delay)
    {
        for(int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];

            CardTransform cardTransform = layoutManager.GetCardTransform(i,handCardObjectList.Count);

            //currentCard.transform.SetPositionAndRotation(cardTransform.pos,cardTransform.rotation);
            
            //卡牌能量判断
            currentCard.UpdateCardState();
            

            currentCard.isAnimating = true;

            currentCard.transform.DOScale(Vector3.one,0.2f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransform.pos,0.5f).onComplete = () => currentCard.isAnimating = false;
                currentCard.transform.DORotateQuaternion(cardTransform.rotation,0.5f);
            };


            //设置卡牌排序
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos,cardTransform.rotation);
        }
    }


    /// <summary>
    /// 洗牌
    /// </summary>
    private void ShuffleDeck()
    {
        discardDeck.Clear();
        //更新UI显示数量
        drawCountEvent.RaisEvent(drawDeck.Count,this);
        discardCountEvent.RaisEvent(discardDeck.Count,this);

        for(int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i,drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }

    /// <summary>
    /// 玩家使用卡牌时，将这张牌回收致弃牌堆
    /// </summary>
    /// <param name="card"></param>
    public void DiscardCard(object obj)
    {
        Card card = obj as Card;
        
        discardDeck.Add(card.cardData);
        handCardObjectList.Remove(card);

        cardManager.DiscardCard(card.gameObject);

        discardCountEvent.RaisEvent(discardDeck.Count,this);

        SetCardLayout(0f);
    }

    /// <summary>
    /// 玩家回合结束时，弃掉所有手牌
    /// </summary>
    public void OnPlayerTurnEnd()
    {
        for(int i = 0; i < handCardObjectList.Count; i++)
        {
            discardDeck.Add(handCardObjectList[i].cardData);
            cardManager.DiscardCard(handCardObjectList[i].gameObject);
        }

        handCardObjectList.Clear();
        discardCountEvent.RaisEvent(discardDeck.Count,this);
    }

    /// <summary>
    /// 战斗结束等时机回收手牌并重建牌堆。
    /// </summary>
    public void ReleaseAllCards(object obj)
    {
        foreach(var card in handCardObjectList)
        {
            cardManager.DiscardCard(card.gameObject);
        }

        handCardObjectList.Clear();
        InitializeDeck();
    }
}

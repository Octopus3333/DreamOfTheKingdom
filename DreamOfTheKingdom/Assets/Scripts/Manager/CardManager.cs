using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList ; //游戏中所有可能出现的卡牌

    public CardLibrarySO newGameCardLibrary;//新游戏时使用的卡牌库
    public CardLibrarySO currentLibrary;//当前游戏中的卡牌库，可以随时从卡牌库中获取卡牌

    private int previousIndex;

    private void Awake()
    {
        InitializeCardDataList();

        foreach(var item in newGameCardLibrary.cardLibraryList)
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }

    /// <summary>
    /// 在场景切换/项目停止时 清空 当前卡牌库
    /// </summary>
    private void OnDisable()
    {
        currentLibrary.cardLibraryList.Clear();
    }    

    #region 获取项目卡牌数据列表
    /// <summary>
    /// 初始化卡牌数据列表
    /// </summary>
    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData",null).Completed += OnCardDataLoaded;
    }

    /// <summary>
    /// 卡牌数据加载完成回调
    /// </summary>
    /// <param name="handle"></param>
    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("No CardData Found!");
        }
    }
    #endregion

    /// <summary>
    /// 获取卡牌
    /// </summary>
    /// <returns></returns>
    public GameObject GetCardObject()
    {
        var cardObj =poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;//获取卡牌时改变其缩放为0
        return cardObj;
    }

    /// <summary>
    /// 回收卡牌
    /// </summary>
    /// <param name="cardObj"></param>
    public void DiscardCard(GameObject cardObj)
    {
        poolTool.ReturnObjectToPool(cardObj);
    }

    public CardDataSO GetNewCardData()
    {
        var randomIndex = 0;
        do{
            randomIndex = Random.Range(0,cardDataList.Count);
        }while(previousIndex == randomIndex);

        previousIndex = randomIndex;
        return cardDataList[randomIndex];
    }

    /// <summary>
    /// 解锁添加新卡牌；库中已有相同 CardData 时仅增加 Amount。
    /// </summary>
    /// <param name="newCardData">要加入牌库的卡牌数据</param>
    public void UnlockCard(CardDataSO newCardData)
    {
        int index = currentLibrary.cardLibraryList.FindIndex(e => e.cardData == newCardData);
        if(index >= 0)
        {
            var entry = currentLibrary.cardLibraryList[index];
            entry.amount++;
            currentLibrary.cardLibraryList[index] = entry;
        }
        else
        {
            currentLibrary.cardLibraryList.Add(new CardLibraryEntry
            {
                cardData = newCardData,
                amount = 1,
            });
        }
    }
}


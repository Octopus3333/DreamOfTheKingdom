using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePlayPanel : MonoBehaviour
{
   [Header("事件广播")]
   public ObjectEventSO playerTurnEndEvent;
   private VisualElement rootElement;
   private Label energyAmountLabel,drawAmountLabel,discardAmountLabel,turnLabel;

   private Button endTurnButton;



   private void OnEnable()
   {
      rootElement = GetComponent<UIDocument>().rootVisualElement;

      //添加元素组件和事件处理
      energyAmountLabel = rootElement.Q<Label>("EnergyAmount");
      drawAmountLabel = rootElement.Q<Label>("DrawAmount");
      discardAmountLabel = rootElement.Q<Label>("DiscardAmount");
      turnLabel = rootElement.Q<Label>("TurnLabel");
      endTurnButton = rootElement.Q<Button>("EndTurn");

      endTurnButton.clicked += OnEndTurnButtonClicked;




      drawAmountLabel.text = "0";
      discardAmountLabel.text = "0";
      energyAmountLabel.text = "0";

      turnLabel.text = "游戏开始";
   }

    private void OnEndTurnButtonClicked()
    {
      playerTurnEndEvent.RaisEvent(null,this);
    }

    public void UpdateDrawDeckAmout(int amount)
   {
      drawAmountLabel.text = amount.ToString();
   }

   public void UpdateDiscardDeckAmout(int amount)
   {
      discardAmountLabel.text = amount.ToString();
   }

   public void UpdateEnergyAmount(int amount)
   {
      energyAmountLabel.text = amount.ToString();
   }

   public void OnEnemyTurnBegin()
   {
      endTurnButton.SetEnabled(false);
      turnLabel.text = "敌人回合";
      turnLabel.style.color = Color.red;
   }

   public void OnPlayerTurnBegin()
   {
      endTurnButton.SetEnabled(true);
      turnLabel.text = "玩家回合";
      turnLabel.style.color = Color.white;
   }
}  

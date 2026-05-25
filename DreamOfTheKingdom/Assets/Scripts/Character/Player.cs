using UnityEngine;

public class Player : CharacterBase
{
    public IntVariable playerMana;
    public int maxMana;

    public int CurrentMana {get => playerMana.currentValue; set => playerMana.SetValue(value);}

    private void OnEnable()
    {
        playerMana.maxValue = maxMana;
        CurrentMana = playerMana.maxValue;//设置初始费用为最大费用
    }

    /// <summary>
    /// 监听事件函数，用于新回合开始时重置费用
    /// </summary>
    public void NewTurn()
    {
        CurrentMana = maxMana;
        Debug.Log("费用已恢复");
    }

    public void UpdateMana(int cost)
    {
        CurrentMana -= cost;
        if(CurrentMana <= 0)
        {
            CurrentMana = 0;
        }
    }

    public void NewGame()
    {
        CurrentHP = MaxHP;
        isDead = false;
        buffRound.currentValue = buffRound.maxValue;
        NewTurn();
    }
}

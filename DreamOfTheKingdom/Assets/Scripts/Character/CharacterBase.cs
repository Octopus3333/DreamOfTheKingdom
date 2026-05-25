using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public int maxHp;
    public IntVariable hp;
    public IntVariable defense;
    public IntVariable buffRound;

    public int CurrentHP{get => hp.currentValue; set => hp.SetValue(value);}
    public int MaxHP {get => hp.maxValue;}
    public bool isDead;

    protected Animator animator;

    public GameObject buff;
    public GameObject debuff;

    //力量相关
    public float baseStrength = 1f;
    private float strengthEffect = 0.5f;

    [Header("事件广播")]
    public ObjectEventSO characterDeadEvent;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
        buffRound.currentValue = buffRound.maxValue;

        ResetDefense();
    }

    protected virtual void Update()
    {
        animator.SetBool("isDead",isDead);
    }

    public virtual void TakeDamage(int damage)
    {
        var currentDamage = (damage - defense.currentValue >= 0) ? (damage - defense.currentValue) : 0;
        var currentDefense =  (damage - defense.currentValue >= 0) ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);

        if(CurrentHP > currentDamage)
        {
            CurrentHP -= currentDamage;
            //Debug.Log("当前HP:" + CurrentHP);
            animator.SetTrigger("hit");
        }
        else
        {
            Debug.Log("当前HP为0,死亡");
            CurrentHP = 0;
            isDead = true;
            characterDeadEvent.RaisEvent(this,this);
        }
    }

    public void UpdateDefense(int amount)
    {
        var value = defense.currentValue + amount;
        defense.SetValue(value);
    }

    public void ResetDefense()
    {
        defense.SetValue(0);
    }

    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        buff.SetActive(true);
    }

    public void SetupStrength(int round , bool isPositive)
    {
        if(isPositive)
        {
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Max(newStrength, 1.5f);
            buff.SetActive(true);
        }
        else
        {
            debuff.SetActive(true);
            baseStrength = 1 - strengthEffect;
        }

        var currentRound = buffRound.currentValue + round;
        if(baseStrength == 1)
            buffRound.SetValue(0);
        else
            buffRound.SetValue(currentRound);
    }

    /// <summary>
    /// 回合开始时结算力量/虚弱剩余回合数，归零后恢复默认攻击力并清除图标。
    /// </summary>
    public void UpdateStrengthRound()
    {
        if(buffRound.currentValue <= 0)
            return;

        buffRound.SetValue(buffRound.currentValue - 1);
        if(buffRound.currentValue > 0)
            return;

        buffRound.SetValue(0);
        baseStrength = 1f;
        buff.SetActive(false);
        debuff.SetActive(false);
    }
}

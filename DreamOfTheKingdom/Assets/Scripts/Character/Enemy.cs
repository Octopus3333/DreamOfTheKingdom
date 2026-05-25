using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase
{
   public EnemyActionDataSO actionDataSO;
   public EnemyAction currentAction;

   protected Player player;



//    protected override void Awake()
//    {
//         base.Awake();
//         player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
//    }

   /// <summary>
   /// 玩家回合开始时：先结算力量/虚弱等层数，再随机本回合意图。
   /// </summary>
   public virtual void OnPlayerTurnBegin()
   {
        UpdateStrengthRound();
        var randomIndex = Random.Range(0,actionDataSO.actions.Count);
        currentAction = actionDataSO.actions[randomIndex];
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
   }

   public virtual void OnEnemyTurnBegin()
   {
        ResetDefense();
        switch(currentAction.effect.effectType)
        {
            case EffectType.Self:
                Skill();
                break;
            case EffectType.Target:
                Attack();
                break;
            case EffectType.All:
                break;
        }
   }

   public virtual void Skill()
   {
        // animator.SetTrigger("skill");
        // currentAction.effect.Execute(this,this);
        StartCoroutine(ProcessDelayAction("skill"));
   }

   public virtual void Attack()
   {
        // animator.SetTrigger("attack");
        // currentAction.effect.Execute(this,player);
        StartCoroutine(ProcessDelayAction("attack"));
   }

   IEnumerator ProcessDelayAction(string actionName)
   {
        animator.SetTrigger(actionName);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime %1.0f > 0.6f 
                                        && !animator.IsInTransition(0)
                                        && animator.GetCurrentAnimatorStateInfo(0).IsName(actionName));
        if(actionName == "attack")
            currentAction.effect.Execute(this,player);
        else
            currentAction.effect.Execute(this,this);
   }
}

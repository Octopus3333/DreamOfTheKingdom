using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;
    private GameObject currentArrow;

    private Card currentCard;
    private bool canMove;
    private bool canExecute;

    private CharacterBase targetCharacter;

    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }
    
    private void OnDisable()
    {
        canMove = false;
        canExecute = false;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!currentCard.isCanUse) return;

        switch(currentCard.cardData.cardType)
        {
            case CardType.Attack:
                currentArrow = Instantiate(arrowPrefab,transform.position,Quaternion.identity);
                break;
            case CardType.Skill:
            case CardType.Abilities:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!currentCard.isCanUse) return;

        if(canMove)
        {
            currentCard.isAnimating = true;
            Vector3 screenPos = new(Input.mousePosition.x,Input.mousePosition.y,10);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            currentCard.transform.position = worldPos;
            canExecute = worldPos.y > 1f;
        }
        else
        {
            if(eventData.pointerEnter == null) 
            {
                Debug.Log("鼠标离开卡牌");
                return;
            }

            if(eventData.pointerEnter.CompareTag("Enemy"))
            {
                Debug.Log("鼠标进入敌人");
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false;
            targetCharacter = null;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!currentCard.isCanUse) return;
        
        if(currentArrow != null)
            Destroy(currentArrow);
            
        if(canExecute)
        {
            Debug.Log("执行卡牌效果");
            currentCard.ExecuteEffects(currentCard.player,targetCharacter);
        }
        else
        {
            Debug.Log("恢复卡牌位置");
            currentCard.RestCardTransform();
            currentCard.isAnimating = false;
        }
    }
}

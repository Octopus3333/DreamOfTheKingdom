using UnityEngine;
using System.Collections.Generic;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;//是否水平排布手牌
    public float maxWidth = 7f;//手牌占有的最大宽度
    public float cardSpacing = 2f;//手牌间间隙

    [Header("弧形参数")]
    public float angleBetweenCards = 7f;
    public float radius = 17f;//圆弧的半径

    public Vector3 centerPoint;//手牌区域中心位置

    [SerializeField]private List<Vector3> cardPositions = new();//手牌的位置
    private List<Quaternion> cardRotations = new();//手牌旋转角度


    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }

    public CardTransform GetCardTransform(int index,int totalCards)
    {
        CalculatePositions(totalCards,isHorizontal);
        //返回手牌位置和旋转角度，创建一个卡牌变换结构体实例
        return new CardTransform(cardPositions[index],cardRotations[index]);
    }

   /// <summary>
   /// 计算手牌位置
   /// </summary>
   /// <param name="numberOfCards"></param>
   /// <param name="horizontal"></param>
    private void CalculatePositions(int numberOfCards,bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        //水平布局
        if(horizontal)
        {
            //计算手牌占有的宽度
            float currentWidth = cardSpacing * (numberOfCards - 1);
            //计算手牌占有的宽度不超过最大宽度
            float totalWidth = Mathf.Min(currentWidth,maxWidth);

            //计算手牌间间隙,手牌越多间隙越小
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            //计算手牌位置
            for(int i = 0; i < numberOfCards; i++)
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);

                var pos = new Vector3(xPos,centerPoint.y,0f);
                var rotation = Quaternion.identity;

                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
        else
        {
            float cardAngle = (numberOfCards - 1) * angleBetweenCards / 2f;

            for(int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition(cardAngle - i * angleBetweenCards);
                var rotation = Quaternion.Euler(0f,0f,cardAngle - i * angleBetweenCards);
                cardPositions.Add(pos);
                cardRotations.Add(rotation);
            }
        }
    }

    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0f
        );
    }
}

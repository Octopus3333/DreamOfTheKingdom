using Unity.Mathematics;

using UnityEngine;

using UnityEngine.UIElements;



public class HealthBarController : MonoBehaviour

{

    private CharacterBase currentCharacter;

    [Header("Elements")]

    public Transform healthBarTransform;



    private UIDocument healthBarDocument;

    private ProgressBar healthBar;



    private VisualElement defenseElement;

    private Label defenseAmountLabel;



    private VisualElement buffElement;

    private Label buffRound;

    [Header("Buff Sprite")]

    public Sprite buffSprite;

    public Sprite debuffSprite;



    private Enemy enemy;

    private VisualElement intentSprite;

    private Label intentAmount;





     private void Awake()

     {

        currentCharacter = GetComponent<CharacterBase>();

        enemy = GetComponent<Enemy>();

     }

    

    private void OnEnable()

    {

        InitHealthBar();

    }



    private void MoveToWorldPosition(VisualElement element , Vector3 worldPosition , Vector2 size)

    {

        Rect rect = RuntimePanelUtils.CameraTransformWorldToPanelRect(element.panel, worldPosition, size, Camera.main);

        element.style.translate = new StyleTranslate(rect.position);

    }



    /// <summary>

    /// 初始化血条 UI；意图图标在 SetIntentElement 前默认隐藏。

    /// </summary>

    public void InitHealthBar()

    {

        healthBarDocument = GetComponent<UIDocument>();

        if (healthBarDocument == null || healthBarDocument.rootVisualElement == null)

        {

            Debug.LogError("HealthBarController：UIDocument 或 rootVisualElement 无效。", this);

            return;

        }



        healthBar = healthBarDocument.rootVisualElement.Q<ProgressBar>("HealthBar");

        if (healthBar == null || currentCharacter == null)

        {

            Debug.LogError("HealthBarController：未找到 HealthBar 或未挂载 CharacterBase。", this);

            return;

        }



        healthBar.highValue = currentCharacter.MaxHP;

        if (healthBarTransform != null)

            MoveToWorldPosition(healthBar, healthBarTransform.position, Vector2.zero);



        defenseElement = healthBar.Q<VisualElement>("Defense");

        defenseAmountLabel = defenseElement != null ? defenseElement.Q<Label>("DefenseAmount") : null;

        if (defenseElement != null)

            defenseElement.style.display = DisplayStyle.None;



        buffElement = healthBar.Q<VisualElement>("Buff");

        buffRound = buffElement != null ? buffElement.Q<Label>("BuffRound") : null;

        if (buffElement != null)

            buffElement.style.display = DisplayStyle.None;



        if (buffRound == null && buffElement != null)

            Debug.LogWarning("HealthBarController：Buff 下需存在 name=\"BuffRound\" 的 Label。", this);



        intentSprite = healthBar.Q<VisualElement>("Intent");

        intentAmount = healthBar.Q<Label>("IntentAmount");

        if (intentSprite != null)

            intentSprite.style.display = DisplayStyle.None;

    }



    private void Update()

    {

        UpdateHealthBar();

    }



    public void UpdateHealthBar()

    {

        if (currentCharacter == null || healthBar == null)

            return;



        if(currentCharacter.isDead)

        {

            healthBar.style.display = DisplayStyle.None;

            return;

        }



        healthBar.style.display = DisplayStyle.Flex;

        healthBar.title = $"{currentCharacter.CurrentHP}/{currentCharacter.MaxHP}";

        // highValue 须在 MaxHP 确定后同步（OnEnable 可能早于 CharacterBase.Start，且 Enemy HP 为共享 SO）
        healthBar.highValue = currentCharacter.MaxHP;
        healthBar.value = currentCharacter.CurrentHP;



        healthBar.RemoveFromClassList("highHealth");

        healthBar.RemoveFromClassList("mediumHealth");

        healthBar.RemoveFromClassList("lowHealth");



        var percentage = (float)currentCharacter.CurrentHP / (float)currentCharacter.MaxHP;



        if(percentage < 0.3)

            healthBar.AddToClassList("lowHealth");

        else if(percentage < 0.6)

            healthBar.AddToClassList("mediumHealth");

        else

            healthBar.AddToClassList("highHealth");



        if (defenseElement != null && defenseAmountLabel != null && currentCharacter.defense != null)

        {

            defenseElement.style.display = currentCharacter.defense.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;

            defenseAmountLabel.text = currentCharacter.defense.currentValue.ToString();

        }



        if (buffElement != null && buffRound != null && currentCharacter.buffRound != null)

        {

            buffElement.style.display = currentCharacter.buffRound.currentValue > 0 ? DisplayStyle.Flex : DisplayStyle.None;

            buffRound.text = currentCharacter.buffRound.currentValue.ToString();

            buffElement.style.backgroundImage = currentCharacter.baseStrength > 1

                ? new StyleBackground(buffSprite)

                : new StyleBackground(debuffSprite);

        }

    }



    #region 敌人意图



    /// <summary>

    /// 玩家回合开始时显示本回合敌人意图。须在 Enemy.OnPlayerTurnBegin 选定 currentAction 之后调用。

    /// </summary>

    public void SetIntentElement()

    {

        if (enemy == null)

            enemy = GetComponent<Enemy>();



        if (enemy == null || enemy.currentAction.effect == null)

        {

            HideIntentElement();

            return;

        }



        if (intentSprite == null || intentAmount == null)

        {

            intentSprite = healthBar?.Q<VisualElement>("Intent");

            intentAmount = healthBar?.Q<Label>("IntentAmount");

        }



        if (intentSprite == null || intentAmount == null)

            return;



        intentSprite.style.display = DisplayStyle.Flex;

        intentSprite.style.backgroundImage = new StyleBackground(enemy.currentAction.intentSprite);



        var value = enemy.currentAction.effect.value;

        if(enemy.currentAction.effect.GetType() == typeof(DamageEffect))

            value = (int)math.round(enemy.currentAction.effect.value * enemy.baseStrength);



        intentAmount.text = value.ToString();

    }



    /// <summary>

    /// 敌人回合结束时隐藏意图图标。

    /// </summary>

    public void HideIntentElement()

    {

        if (intentSprite == null)

            return;



        intentSprite.style.display = DisplayStyle.None;

    }



    #endregion

}



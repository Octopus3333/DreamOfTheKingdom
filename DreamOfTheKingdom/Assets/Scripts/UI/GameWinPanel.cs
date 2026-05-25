using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 战斗/宝箱胜利面板。UI 绑定必须在 OnEnable 中完成：UIDocument 禁用后会重建 VisualTree，Awake 缓存的 Button 会失效。
/// </summary>
public class GameWinPanel : MonoBehaviour
{
    private Button pickCardButton;
    private Button backToMapButton;

    [Header("事件广播")]
    public ObjectEventSO loadMapEvent;
    public ObjectEventSO pickCardEvent;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        pickCardButton = root.Q<Button>("PickCardButton");
        backToMapButton = root.Q<Button>("BackToMapButton");

        pickCardButton.clicked += OnPickCardButtonClicked;
        backToMapButton.clicked += OnBackToMapButtonClicked;

        pickCardButton.style.display = DisplayStyle.Flex;
        pickCardButton.SetEnabled(true);
        backToMapButton.SetEnabled(true);
    }

    private void OnDisable()
    {
        if (pickCardButton != null)
            pickCardButton.clicked -= OnPickCardButtonClicked;
        if (backToMapButton != null)
            backToMapButton.clicked -= OnBackToMapButtonClicked;

        pickCardButton = null;
        backToMapButton = null;
    }

    private void OnPickCardButtonClicked()
    {
        pickCardEvent.RaisEvent(null, this);
    }

    private void OnBackToMapButtonClicked()
    {
        loadMapEvent.RaisEvent(null, this);
    }

    public void OnFinishPickCardEvent(object data)
    {
        if (pickCardButton != null)
            pickCardButton.style.display = DisplayStyle.None;
    }
}

using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverPanel : MonoBehaviour
{
    public Button button;
    public ObjectEventSO loadMenuEvent;

    private void OnEnable()
    {
        GetComponent<UIDocument>().rootVisualElement.Q<Button>("BackToStartButton").clicked += BackToStart;
    }

    private void BackToStart()
    {
        loadMenuEvent.RaisEvent(null, this);
    }
}

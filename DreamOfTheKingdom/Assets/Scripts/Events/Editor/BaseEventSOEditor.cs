using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if(baseEventSO == null)
            baseEventSO = target as BaseEventSO<T>;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("订阅数量:" + GetListeners().Count);

        foreach(var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString());
        }
    }

    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> listeners = new();

        if(baseEventSO.OnEventRaised == null  || baseEventSO == null)
            return listeners;

        var subscribers = baseEventSO.OnEventRaised.GetInvocationList();
        foreach (var subscriber in subscribers)
        {
           var obj = subscriber.Target as MonoBehaviour;
            if(!listeners.Contains(obj))
                listeners.Add(obj);
        }

        return listeners;
    }
}

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 泛型事件监听器类，用于监听和处理特定类型的事件
/// </summary>
/// <typeparam name="T">事件数据类型</typeparam>
public class BaseEventListener<T>: MonoBehaviour
{
    
    public BaseEventSO<T> eventSO;
    public UnityEvent<T> response;

    private void OnEnable()
    {
        // 如果事件SO不为空，则订阅事件
        if(eventSO != null)
            eventSO.OnEventRaised += OnEventRaised;
    }

    private void OnDisable()
    {
        // 如果事件SO不为空，则取消订阅事件
        if(eventSO != null)
            eventSO.OnEventRaised -= OnEventRaised;
    }

    /// <summary>
    /// 当事件被触发时的处理方法
    /// </summary>
    /// <param name="value">事件传递的数据值</param>
    private void OnEventRaised(T value)
    {
        // 调用响应事件并传递数据值
        response.Invoke(value);
    }
}

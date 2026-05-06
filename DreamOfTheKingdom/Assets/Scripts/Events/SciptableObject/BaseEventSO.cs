using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 泛型基类事件ScriptableObject，用于创建可被脚本化对象触发的事件
/// </summary>
/// <typeparam name="T">事件参数类型</typeparam>
public class BaseEventSO<T> : ScriptableObject
{
    /// <summary>
    /// 事件描述信息，使用TextArea特性可以在Inspector中以文本框形式显示
    /// </summary>
    [TextArea]
    public string description;

    public UnityAction<T> OnEventRaised;

    //记录下最后一个发出广播的对象
    public string lastSender;

    /// <summary>
    /// 触发事件的方法
    /// </summary>
    /// <param name="value">事件触发时传递的参数值</param>
    public void RaisEvent(T value, Object sender)
    {
        // 使用空条件运算符安全地调用委托，如果委托不为null则触发
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }
}

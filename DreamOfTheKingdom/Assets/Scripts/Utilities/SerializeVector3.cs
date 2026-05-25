using UnityEngine;

/// <summary>
/// 用于 ScriptableObject / 可序列化类型中的 Vector3 数据，必须是纯数据类型。
/// 若继承 MonoBehaviour，嵌套在 MapLayoutSO 中时 Unity 无法按普通字段持久化坐标，会导致地图连线保存丢失。
/// </summary>
[System.Serializable]
public class SerializeVector3
{
    public float x, y, z;

    /// <summary>无参构造：供 Unity 反序列化使用。</summary>
    public SerializeVector3() { }

    /// <summary>
    /// 构造函数：用于将 Vector3 写入可持久化字段。
    /// </summary>
    /// <param name="position">世界或本地空间坐标</param>
    public SerializeVector3(Vector3 position)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }
    
    /// <summary>
    /// 将当前对象转换为三维向量(Vector3)
    /// </summary>
    /// <returns>返回一个新的Vector3对象，使用当前对象的x、y、z坐标值</returns>
    public Vector3 ToVector3()
    {
        // 创建并返回一个新的Vector3实例
        // 使用当前对象的x、y、z属性作为向量的三个分量
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

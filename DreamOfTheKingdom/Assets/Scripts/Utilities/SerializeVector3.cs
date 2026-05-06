using UnityEngine;

[System.Serializable]
public class SerializeVector3 : MonoBehaviour
{
    public float x, y, z;

    /// <summary>
    /// 构造函数：用于将Vector3对象序列化为可持久化的格式
    /// </summary>
    /// <param name="position">需要序列化的三维向量对象</param>
    public SerializeVector3(Vector3 position)
    {
        // 将传入Vector3对象的x分量赋值给当前实例的x属性
        x = position.x;
        // 将传入Vector3对象的y分量赋值给当前实例的y属性
        y = position.y;
        // 将传入Vector3对象的z分量赋值给当前实例的z属性
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

    public Vector2 ToVector2()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-100)]
public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;

    private ObjectPool<GameObject> Pool;

    private void Awake()
    {
        //初始化对象池
        Pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab,transform),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        ) ;

        PreFillPool(7);
    }


    /// <summary>
    /// 预填充对象池
    /// </summary>
    /// <param name="count"></param>
    private void PreFillPool(int count)
    {
        var preFillArray = new GameObject[count];
        for (int i= 0;i<count;i++)
        {
            preFillArray[i] = Pool.Get();
        }

        foreach(var item in preFillArray)
        {
            Pool.Release(item);
        }
    }
    
    /// <summary>
    /// 从对象池中获取对象（对外）
    /// </summary>
    /// <returns></returns>
    public GameObject GetObjectFromPool()
    {
        return Pool.Get();
    }

    /// <summary>
    /// 将对象返回给对象池（对外）
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnObjectToPool(GameObject obj)
    {
        Pool.Release(obj);
    }
}
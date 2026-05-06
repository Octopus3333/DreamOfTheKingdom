using UnityEngine;
using UnityEngine.Pool;

public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;

    private ObjectPool<GameObject> Pool;

    private void Start()
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

    public GameObject GetObjectFromPool()
    {
        return Pool.Get();
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        Pool.Release(obj);
    }
}
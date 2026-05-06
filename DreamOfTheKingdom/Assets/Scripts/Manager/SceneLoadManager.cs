using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;//当前加载的场景
    public AssetReference map;

    private Vector2Int currentRoomVector;

    [Header("广播")]
    public ObjectEventSO afterLoadRoomEvent;

    /// <summary>
    /// 在房间加载事件中监听
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        if(data is Room)
        {
            Room currentRoom = data as Room;
            var currentData = currentRoom.roomData;
            currentRoomVector = new Vector2Int(currentRoom.column, currentRoom.line);

            currentScene = currentData.sceneToLoad;
        }

        //先卸载地图场景
        await UnloadSceneTask();
        //加载房间场景
        await LoadSceneTask();

        afterLoadRoomEvent.RaisEvent(currentRoomVector, this);
    }


    /// <summary>
    /// 异步加载场景的任务方法
    /// 使用协程方式异步加载场景，并在加载完成后激活该场景
    /// </summary>
    private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;

        if(s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    /// <summary>
    /// 异步卸载当前场景的任务方法
    /// 使用Awaitable类型实现异步操作，确保场景卸载过程不会阻塞主线程
    /// </summary>
    private async Awaitable UnloadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    /// <summary>
    /// 异步加载地图场景的方法
    /// </summary>
    public async void LoadMap()
    {
        //先卸载房间场景
        await UnloadSceneTask();

        currentScene = map; //将当前场景设置为地图场景
        //加载地图场景
        await LoadSceneTask();
    }
}

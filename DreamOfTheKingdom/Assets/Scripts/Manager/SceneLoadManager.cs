using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public FadePanel fadePanel;
    private AssetReference currentScene;//当前加载的场景
    public AssetReference map;
    public AssetReference menu;

    public AssetReference intro;

    private Vector2Int currentRoomVector;

    private Room currentRoom;

    [Header("广播")]
    public ObjectEventSO afterLoadRoomEvent;
    public ObjectEventSO updateRoomEvent;

    private void Awake()
    {
        currentRoomVector = Vector2Int.one * -1;
        //LoadMenu();
        LoadIntro();
    }

    /// <summary>
    /// 在房间加载事件中监听
    /// </summary>
    /// <param name="data"></param>
    public async void OnLoadRoomEvent(object data)
    {
        if(data is Room)
        {
            currentRoom = data as Room;
            var currentData = currentRoom.roomData;
            currentRoomVector = new Vector2Int(currentRoom.column, currentRoom.line);

            currentScene = currentData.sceneToLoad;
        }

        //先卸载地图场景
        await UnloadSceneTask();
        //加载房间场景
        await LoadSceneTask();

        afterLoadRoomEvent.RaisEvent(currentRoom, this);
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
            fadePanel.FadeOut(0.2f);
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }

    /// <summary>
    /// 异步卸载当前场景的任务方法
    /// 使用Awaitable类型实现异步操作，确保场景卸载过程不会阻塞主线程
    /// </summary>
    private async Awaitable UnloadSceneTask()
    {
        fadePanel.FadeIn(0.4f);
        await Awaitable.WaitForSecondsAsync(0.45f);
        await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene()));
    }

    /// <summary>
    /// 异步加载地图场景的方法
    /// </summary>
    public async void LoadMap()
    {
        //先卸载房间场景
        await UnloadSceneTask();
        if(currentRoomVector != Vector2.one * -1)
        {
            updateRoomEvent.RaisEvent(currentRoomVector, this);
        }

        currentScene = map; //将当前场景设置为地图场景
        //加载地图场景
        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        if(currentScene != null)
            await UnloadSceneTask();
            
        currentScene = menu;
        await LoadSceneTask();
    }

    public async void LoadIntro()
    {
        if(currentScene != null)
            await UnloadSceneTask();
            
        currentScene = intro;
        await LoadSceneTask();
    }
}

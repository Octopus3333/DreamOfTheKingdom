using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;
    
    public List<Enemy> aliveEnemyList;

    [Header("事件广播")]
    public ObjectEventSO gameWinEvent;
    public ObjectEventSO gameOverEvent;


    /// <summary>
    /// 更新房间的事件监听函数,加载地图
    /// </summary>
    /// <param name="roomVector"></param>
    public void UpdateMapLayout(object value)
    {
        var roomVector = (Vector2Int)value;
        if(mapLayout.mapRoomDataList.Count == 0)
            return;
        var currentRoom = mapLayout.mapRoomDataList.Find(r => r.colum == roomVector.x && r.line == roomVector.y);

        currentRoom.roomState = RoomState.Visited;
        //更新相邻房间的数据

        var sameColumnRooms = mapLayout.mapRoomDataList.FindAll(r => r.colum == roomVector.x);

        foreach (var room in sameColumnRooms)
        {
            if(room.line != roomVector.y)
            {
                room.roomState = RoomState.Locked;
            }
        } 

        foreach (var link in currentRoom.linkTo)
        {
            var linkedRoom = mapLayout.mapRoomDataList.Find(r => r.colum == link.x && r.line == link.y);
            linkedRoom.roomState = RoomState.Attainable;
        } 

        aliveEnemyList.Clear();
    }

    public void OnRoomLoadedEvent(object obj)
    {
        var enemies = FindObjectsByType<Enemy>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        foreach(var enemy in enemies)
        {
            aliveEnemyList.Add(enemy);
        }
    }

    /// <summary>
    /// 角色死亡事件
    /// </summary>
    /// <param name="character"></param>
    public void OnCharacterDeadEvent(object character)
    {
        if(character is Player)
        {
            //发出失败通知
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        if(character is Boss)
        {
            //发出获胜通知
            StartCoroutine(EventDelayAction(gameOverEvent));
        }
        else if(character is Enemy)
        {
            aliveEnemyList.Remove(character as Enemy);

            if(aliveEnemyList.Count == 0)
            {
                //发出获胜通知
                StartCoroutine(EventDelayAction(gameWinEvent));
            }
        }

        
    }

    IEnumerator EventDelayAction(ObjectEventSO eventSO)
    {
        yield return new WaitForSeconds(1.5f);
        eventSO.RaisEvent(null,this);
    }

    public void OnNewGameEvent()
    {
        mapLayout.mapRoomDataList.Clear();
        mapLayout.linePositionList.Clear();
    }
}

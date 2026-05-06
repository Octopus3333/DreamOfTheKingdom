using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;//列
    public int line;//行
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;
    public List<Vector2Int> linkeTo = new();

    [Header(header:"广播")]
    public ObjectEventSO loadRoomEvent;
    
    private void Awake() 
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    private void OnMouseDown()
    {
         //测试
        Debug.Log("该房间是："+ roomData.roomType);
        //处理点击事件
        if(roomState == RoomState.Attainable)
            loadRoomEvent.RaisEvent(this, this);
    }

    /// <summary>
    /// 外部创建房间时调用配置房间
    /// </summary>
    /// <param name="column"></param>
    /// <param name="line"></param>
    /// <param name="roomData"></param>
    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;

        spriteRenderer.sprite = roomData.roomIcon;
        spriteRenderer.color = roomState switch
        {
            RoomState.Locked => new Color(0.5f, 0.5f, 0.5f, 1f),
            RoomState.Visited => new Color(0.5f, 0.8f, 0.5f, 0.5f),
            RoomState.Attainable => Color.white,
            _ => throw new System.NotImplementedException(),
        };
    }
}

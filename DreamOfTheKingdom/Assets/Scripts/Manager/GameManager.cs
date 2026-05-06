using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("地图布局")]
    public MapLayoutSO mapLayout;


    /// <summary>
    /// 更新房间的事件监听函数
    /// </summary>
        /// <param name="roomVector"></param>
    public void UpdateMapLayout(object value)
    {
        var roomVector = (Vector2Int)value;
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
    }
}

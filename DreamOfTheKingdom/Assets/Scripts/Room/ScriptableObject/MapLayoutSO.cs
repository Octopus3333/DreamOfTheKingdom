using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLayoutSO", menuName = "Map/MapLayoutSO")]
/// <summary>
/// 地图布局数据
/// </summary>
public class MapLayoutSO : ScriptableObject
{
    //地图房间数据列表
    public List<MapRoomData> mapRoomDataList = new();
    //地图连线数据列表
    public List<LinePosition> linePositionList = new();
}

[System.Serializable]
/// <summary>
/// 地图房间数据
/// </summary>
public class MapRoomData
{
    public float posX, posY;
    public int colum,line;
    public RoomDataSO roomData;
    public RoomState roomState;

    //地图房间连接数据列表（与运行时 Room 解耦，保存时需拷贝一份）
    public List<Vector2Int> linkTo = new();

}

[System.Serializable]
/// <summary>
/// 地图连线数据
/// </summary>
public class LinePosition
{
    public SerializeVector3 startPos,endPos;
}

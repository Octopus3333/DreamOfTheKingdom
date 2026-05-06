using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("地图配置表")]
    public MapConfigSO mapConfig;
    [Header("地图布局")]
    public MapLayoutSO mapLayout;

    [Header("房间预制体")]
    public Room roomPrefab;

    public LineRenderer linePrefab;

    private float screenHeight;
    private float screenWidth;
    private float columnWidth;
    private Vector3 generatorePoint;

    public float border;

    private List<Room> rooms = new();
    private List<LineRenderer> lines = new();

    public List<RoomDataSO> roomDataList = new();
    //用字典来存储对应的房间类型和数据
    private Dictionary<RoomType,RoomDataSO> roomDataDict = new();


    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2f;
        screenWidth = screenHeight * Camera.main.aspect;

        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);//每一列之间的宽度

        foreach (var roomData in roomDataList)
        {
            roomDataDict.Add(roomData.roomType, roomData);
        }
    }

    // private void Start()
    // {
    //     CreateMap();
    // }

    private void OnEnable()
    {
        if(mapLayout.mapRoomDataList.Count>0)
            LoadMap();
        else
            CreateMap();
    }


    public void CreateMap()
    {
        //创建前一列房间列表
        List<Room> previousColumnRooms = new();

        for (int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];
            var amount = UnityEngine.Random.Range(blueprint.min, blueprint.max);//随机生成房间数量
            var startHeight = screenHeight / 2 - screenHeight / (amount + 1);//初始房间的y位置
            generatorePoint = new Vector3(-screenWidth / 2 + border + columnWidth * column, startHeight, 0);//初始房间的位置
            var newPosition = generatorePoint;
            var roomGapY = screenHeight / (amount + 1);//房间之间的高度间隔

            //创建当前房间列表
            List<Room> currentColumnRooms = new();

            //循环当前列的所有房间数量生成房间
            for (int i = 0; i < amount; i++)
            {
                //检测房间位置，如果是最后一个房间（boss），就固定位置不添加随机偏移
                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border * 2;
                }
                //如果不是最后一列，也不是第一列，添加随机偏移
                else if (column != 0)
                {
                    newPosition.x = generatorePoint.x + UnityEngine.Random.Range(-border / 2, border / 2);
                }

                newPosition.y = startHeight - roomGapY * i; //更新房间位置

                //生成房间
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity, transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);

                //设置只有第一列房间可以进入其他房间锁
                if(column == 0)
                    room.roomState = RoomState.Attainable;
                else
                    room.roomState = RoomState.Locked;
                

                room.SetupRoom(column, i, GetRoomData(newType));

                

                rooms.Add(room);
                //将房间添加到当前列房间列表
                currentColumnRooms.Add(room);
            }

            //判断当前列是否为第一列，如果不是则连接到上一列
            if (previousColumnRooms.Count > 0)
            {
                //创建两个列表的连线
                CreateConnections(previousColumnRooms,currentColumnRooms);

            }

            previousColumnRooms = currentColumnRooms;//上一列为当前列
        }

        SaveMap();
    }

    //创建连线的方法
    private void CreateConnections(List<Room> column1, List<Room> column2)
    {
        //为了确保第二列的房间都被连接上了，要创建一个不会重复的列表来存储已经连接过的房间
        HashSet<Room> connectedRooms = new();

        //遍历第一列的房间，连接到第二列的随机房间
        foreach (var room in column1)
        {
            var targetRoom = ConnectToRandomRoom(room, column2,false);
            //将连接过的房间添加到列表中
            connectedRooms.Add(targetRoom);
        }

        //遍历第二列的房间，检查是否有未连接的房间，如果有则连接到第一列的随机房间，确保所有房间都连接上
        foreach (var room in column2)
        {
            if (!connectedRooms.Contains(room))
            {
                ConnectToRandomRoom(room, column1,true);
            }
        }

    }

    private Room ConnectToRandomRoom(Room room, List<Room> column2,bool check)
    {
        Room targetRoom;

        targetRoom = column2[UnityEngine.Random.Range(minInclusive: 0, column2.Count)];

        if(check)
        {
            targetRoom.linkeTo.Add(new(room.column, room.line)); 
        }
        else
        {
            room.linkeTo.Add(new(targetRoom.column, targetRoom.line));
        }
        

        //创建房间之间的连线
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);

        return targetRoom;
    }


    [ContextMenu("ReGenerateRoom")]
    public void ReGenerateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        rooms.Clear();
        lines.Clear();

        CreateMap();
    }

    /// <summary>
    /// 根据房间类型获取对应的房间数据
    /// </summary>
    /// <param name="roomType">要获取对应数据的房间类型</param>
    /// <returns>返回对应的房间数据对象</returns>
    private RoomDataSO GetRoomData(RoomType roomType)
    {
        // 从 房间数据字典 中 根据 房间类型 获取对应的房间数据
        return roomDataDict[roomType];
    }

    /// <summary>
    /// 根据给定的房间类型标志，随机选择一个房间类型
    /// </summary>
    /// <param name="flags">房间类型的组合标志，使用逗号分隔</param>
    /// <returns>返回随机选择的单个房间类型</returns>
    private RoomType GetRandomRoomType(RoomType flags)
    {
        // 将房间类型标志字符串按逗号分割成数组
        string[] options = flags.ToString().Split(',');
        // 从数组中随机选择一个元素
        string randomOption = options[UnityEngine.Random.Range(0,options.Length)];

        // 将选中的字符串转成RoomType枚举值
        RoomType roomtype = (RoomType)Enum.Parse(typeof(RoomType), randomOption);

        // 返回随机选择的房间类型
        return roomtype;
    }

    private void SaveMap()
    {
        mapLayout.mapRoomDataList = new();
        
        //添加所有已生成的房间
        for(int i=0; i < rooms.Count;i++)
        {
            var room = new MapRoomData()
            {
                posX = rooms[i].transform.position.x,
                posY = rooms[i].transform.position.y,
                colum = rooms[i].column,
                line = rooms[i].line,
                roomData = rooms[i].roomData,
                roomState = rooms[i].roomState,
                linkTo = rooms[i].linkeTo,
            };

            mapLayout.mapRoomDataList.Add(room);
        }

        mapLayout.linePositionList = new();
        //添加所有连线
        for(int i=0; i<lines.Count;i++)
        {
            var line = new LinePosition()
            {
              startPos= new SerializeVector3(lines[i].GetPosition(0)),
              endPos = new SerializeVector3(lines[i].GetPosition(1)) , 
            };

            mapLayout.linePositionList.Add(line);
        }
    }

    private void LoadMap()
    {
        //读取房间数据生成房间
        for (int i=0;i<mapLayout.mapRoomDataList.Count;i++)
        {
            var newPos = new Vector3(mapLayout.mapRoomDataList[i].posX, mapLayout.mapRoomDataList[i].posY, 0);
            var newRoom = Instantiate(roomPrefab,newPos,Quaternion.identity,transform);
            newRoom.roomState = mapLayout.mapRoomDataList[i].roomState;
            newRoom.SetupRoom(mapLayout.mapRoomDataList[i].colum,mapLayout.mapRoomDataList[i].line,mapLayout.mapRoomDataList[i].roomData);
            newRoom.linkeTo = mapLayout.mapRoomDataList[i].linkTo;
            rooms.Add(newRoom);
        }

        //读取连线
        for(int i = 0;i<mapLayout.linePositionList.Count;i++)
        {
            var line = Instantiate(linePrefab, transform);
            line.SetPosition(0, mapLayout.linePositionList[i].startPos.ToVector3());
            line.SetPosition(1, mapLayout.linePositionList[i].endPos.ToVector3());
            lines.Add(line);
        }
    }

}

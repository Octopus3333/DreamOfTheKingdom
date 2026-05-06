using System;

[Flags]
public enum RoomType
{
    MinorEnemy = 1,//普通敌人
    EliteEnemy = 2,//精英敌人
    Shop = 4, //商店
    Treasure = 8, //宝箱
    RestRoom = 16, //休息室
    Boss = 32 //Boss
}

public enum RoomState
{
    Locked,//锁定
    Visited,//已访问
    Attainable,//可访问
}

public enum CardType
{
    Attack,
    Defense,
    Abilities
}
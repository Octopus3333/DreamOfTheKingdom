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
    Attack,//攻击
    Skill,//技能
    Abilities,//能力
}

public enum EffectType
{
    Self,//自己
    Target,//对单
    All,//对群
}
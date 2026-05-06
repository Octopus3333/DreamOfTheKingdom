using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapConfigSO", menuName = "Map/MapConfigSO")]
public class MapConfigSO : ScriptableObject
{
    public List<RoomBlueprint> roomBlueprints;
}

[System.Serializable]
public class RoomBlueprint
{
    public int min, max;//该类型房间的最小最大数量
    public RoomType roomType;//房间类型
}
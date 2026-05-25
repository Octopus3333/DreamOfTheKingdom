using UnityEngine;

public struct CardTransform
{
    public Vector3 pos;
    public Quaternion rotation;

    public CardTransform(Vector3 vector3,Quaternion quaternion)
    {
        this.pos = vector3;
        this.rotation = quaternion;
    }
}

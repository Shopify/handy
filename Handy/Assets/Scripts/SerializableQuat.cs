using System;

[Serializable]
public class SerializableQuat
{
    public float x;
    public float y;
    public float z;
    public float w;

    public static SerializableQuat FromQuaternion(UnityEngine.Quaternion quat)
    {
        var ret = new SerializableQuat();
        ret.x = quat.x;
        ret.y = quat.y;
        ret.z = quat.z;
        ret.w = quat.w;
        return ret;
    }
}

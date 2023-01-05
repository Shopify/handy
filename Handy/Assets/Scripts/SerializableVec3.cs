using System;

// A serializable Vector3
[Serializable]
public class SerializableVec3
{
    public float x;
    public float y;
    public float z;

    public static SerializableVec3 FromVector3(UnityEngine.Vector3 vec)
    {
        var ret = new SerializableVec3();
        ret.x = vec.x;
        ret.y = vec.y;
        ret.z = vec.z;
        return ret;
    }
}

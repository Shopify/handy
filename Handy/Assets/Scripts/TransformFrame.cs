using System;
using UnityEngine;

[Serializable]
public class TransformFrame
{
    public float timestamp;
    public SerializableVec3 position;
    public SerializableQuat rotation;
    public SerializableVec3 scale;

    public static TransformFrame FromTransform(float timestamp, Transform t) {
        var ret = new TransformFrame();
        ret.timestamp = timestamp;
        ret.position = SerializableVec3.FromVector3(t.localPosition);
        ret.rotation = SerializableQuat.FromQuaternion(t.localRotation);
        ret.scale = SerializableVec3.FromVector3(t.localScale);
        return ret;
    }

    public void CopyToTransform(Transform t)
    {
        t.localPosition = new Vector3(position.x, position.y, position.z);
        t.localRotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        t.localScale = new Vector3(scale.x, scale.y, scale.z);
    }

    public static TransformFrame FromFlattened(float[] f) {
        var ret = new TransformFrame();
        ret.timestamp = f[0];
        ret.position = new SerializableVec3();
        ret.position.x = f[1];
        ret.position.y = f[2];
        ret.position.z = f[3];
        ret.rotation = new SerializableQuat();
        ret.rotation.x = f[4];
        ret.rotation.y = f[5];
        ret.rotation.z = f[6];
        ret.rotation.w = f[7];
        ret.scale = new SerializableVec3();
        ret.scale.x = f[8];
        ret.scale.y = f[9];
        ret.scale.z = f[10];
        return ret;
    }

    public float[] Flattened()
    {
        return new float[11] {
            timestamp,
            position.x,
            position.y,
            position.z,
            rotation.x,
            rotation.y,
            rotation.z,
            rotation.w,
            scale.x,
            scale.y,
            scale.z,
        };
    }
}

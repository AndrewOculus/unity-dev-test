using System;
using UnityEngine;

[Serializable]
public class SerializableModel
{
    public float[] x;
    public float[] y;
    public float[] z;
    [NonSerialized]
    private Vector3 point = Vector3.zero;
    public Vector3 GetVector(int idx){
        point.Set(x[idx], y[idx], z[idx]);
        return point;
    }
}

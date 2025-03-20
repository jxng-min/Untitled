using UnityEngine;

[System.Serializable]
public class SVector3 
{
    public float x, y, z;

    public SVector3(Vector3 vector)
    {
        this.x = vector.x;
        this.y = vector.y;
        this.z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

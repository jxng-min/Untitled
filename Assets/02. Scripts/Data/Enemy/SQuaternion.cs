using UnityEngine;

[System.Serializable]
public class SQuaternion
{
    public float x,y,z,w;

    public SQuaternion(Quaternion Q)
    {
        this.x = Q.x;
        this.y = Q.y;
        this.z = Q.z;
        this.w = Q.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}

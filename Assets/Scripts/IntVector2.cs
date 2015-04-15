[System.Serializable]
public struct IntVector2
{
    public int x, z;

    public IntVector2(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    }
}

public static class IntVector2Extensions
{
    public static UnityEngine.Vector3 ToWorldspace(this IntVector2 current, IntVector2 maxSize)
    {
        // converts 0,0 => -9.5,-9.5 when max is 20,20
        var result = new UnityEngine.Vector3(
            current.x - maxSize.x / 2 + .5f,
            0,
            current.z - maxSize.z / 2 + .5f
        );

        //UnityEngine.Debug.Log(current.x + "," + current.z + "ToWorldspace" + result);
        return result;
    }
}
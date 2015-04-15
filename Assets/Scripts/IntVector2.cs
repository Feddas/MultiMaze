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
    public static UnityEngine.Vector3 ToCellcenter(this IntVector2 current, IntVector2 maxSize)
    {
        // converts 0,0 => -9.5,-9.5 when max is 20,20
        var result = new UnityEngine.Vector3(
            current.x - lengthOffset(maxSize.x),
            1,
            current.z - lengthOffset(maxSize.z)
        );

        //UnityEngine.Debug.Log("for cell (" + current.x + "," + current.z + ") player placed at " + result);
        return result;
    }

    private static float lengthOffset(int max)
    {
        // gameboard is centered around 0,0
        float unitsFromCenter = max / 2;

        // if the board is an even number of units in length, add 0.5 because a wall is at 0,0. if it's an odd length it's a cell center.
        float odd = (max % 2 == 0) ? .5f : 0;

        return unitsFromCenter - odd;
    }
}
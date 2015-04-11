using UnityEngine;
using System.Collections;

public abstract class MazeCellEdge : MonoBehaviour
{

    public MazeCell cell;
    /// <summary> cell that this cell connects with </summary>
    public MazeCell otherCell;

    /// <summary> this cells orientation </summary>
    public MazeDirection direction;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        this.cell = cell;
        this.otherCell = otherCell;
        this.direction = direction;
        cell.SetEdge(direction, this);
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = direction.ToRotation(); // TODO: only rotate if wall
    }
}

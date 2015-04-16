using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Growing Tree algorithm
/// created from tutorial http://catlikecoding.com/unity/tutorials/maze/ / http://www.reddit.com/r/gamedev/comments/29qlrj/i_wrote_a_maze_generating_tutorial_for_c_unity_45/
/// 
/// Alternate Maze Alorithms:
/// http://www.cgl.uwaterloo.ca/~csk/projects/mazes/
/// http://bl.ocks.org/mbostock/11357811
/// </summary>
public class Maze : MonoBehaviour
{
    public IntVector2 size;

    public MazeCell cellPrefab;

    public MazePassage passagePrefab;
    public MazeWall wallPrefab;
    public GameObject mazeFinish;

    private MazeCell[,] cells;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static IntVector2 RandomCoordinates(IntVector2 maxSize)
    {
        return new IntVector2(
            Random.Range(0, maxSize.x),
            Random.Range(0, maxSize.z));
    }

    /// <summary>
    /// true when is within (20x20) size bounds
    /// </summary>
    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x
            && coordinate.z >= 0 && coordinate.z < size.z;
    }

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }
    public MazeCell[] GetCells()
    {
        return cells.Cast<MazeCell>().ToArray();
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        DoFirstGenerationStep(activeCells, RandomCoordinates(size));
    }
    private void DoFirstGenerationStep(List<MazeCell> activeCells, IntVector2 startingCell)
    {
        activeCells.Add(CreateCell(startingCell));
    }

    // each cell tries to move a random MazeDirection until it collides
    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        // get last active cell
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized) // all its edges have been moved to
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }

        // try to move random direction from cell
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();

        if (ContainsCoordinates(coordinates))// && GetCell(coordinates) == null)
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null) // connect a new cell
            {
                neighbor = CreateCell(coordinates);
                trackDistance(currentCell, neighbor);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else // wall off already connectedcell, avoid loops
            {
                CreateWall(currentCell, neighbor, direction);
            }
        }
        else // edge of the maze size
        {
            CreateWall(currentCell, null, direction);
        }
    }

    private void trackDistance(MazeCell parent, MazeCell newCell)
    {
        newCell.DistanceFromFirstCell = parent.DistanceFromFirstCell + 1;
    }
    private void addChild(MazeCell toCell, GameObject childToAdd)
    {
        GameObject child = Instantiate(childToAdd) as GameObject;
        child.name = childToAdd.name;
        child.transform.parent = toCell.gameObject.transform;
        child.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }

    public IntVector2[] Generate(IntVector2 startCoordinate, int numberPlayers)
    {
        cells = new MazeCell[size.x, size.z];

        // cells where a side may be open to add a new cell
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells, startCoordinate);
        while (activeCells.Count > 0)
        {
            DoNextGenerationStep(activeCells);
        }

        //Debug.Log("Max cell " + maxDistanceCell.x + "," + maxDistanceCell.z + " is " + maxDistance);
        addChild(cells[startCoordinate.x, startCoordinate.z], mazeFinish);
        return GetPostionsAtMaxDistance(numberPlayers);
    }

    public IntVector2[] GetPostionsAtMaxDistance(int howMany)
    {
        IntVector2[] startLocations = new IntVector2[howMany];

        var uniqueSpots = GetCells()
                .GroupBy(e => e.DistanceFromFirstCell)
                .Where(group => group.Count() == howMany)
                .Select(group => new { Distance = group.Key, Count = group.Count(), Cells = group.ToList() })
                .ToList();

        if (uniqueSpots.Count < 1) // requirement doesn't exist, players have to start ontop of each other.
            startLocations = GetPostionsAtMaxDistance(howMany - 1);
        else
        {
            var maxValue = uniqueSpots.Max(x => x.Distance);
            startLocations = uniqueSpots.First(x => x.Distance == maxValue).Cells.Select(c => c.coordinates).ToArray();
        } 

        return startLocations;
    }

    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(
            coordinates.x - size.x * 0.5f + 0.5f,
            0f,
            coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }
}

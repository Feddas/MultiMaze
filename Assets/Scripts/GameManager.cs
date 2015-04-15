using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private IntVector2 startCoordinates;

    private void Start()
    {
        BeginGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public Maze mazePrefab;

    private Maze mazeInstance;

    private void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        startCoordinates = Maze.RandomCoordinates(mazeInstance.size);
        StartCoroutine(mazeInstance.Generate(startCoordinates));
        player.transform.position = startCoordinates.ToWorldspace(mazeInstance.size);
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        BeginGame();
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text FinishText;

    public static GameManager Instance { get; set; }
    
    public GameObject player;
    public Rigidbody playerRigidbody;

    private IntVector2 startCoordinates;

    private void Start()
    {
        BeginGame();
        Instance = this;
        playerRigidbody = player.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButtonDown("Jump"))
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
        //StartCoroutine();
        mazeInstance.Generate(startCoordinates);
        player.transform.position = startCoordinates.ToCellcenter(mazeInstance.size);
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        FinishText.text = "";
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        Time.timeScale = 1;
        BeginGame();
    }

    public void Winner(string player)
    {
        FinishText.text = player + " won!";
        Time.timeScale = 0;
    }
}
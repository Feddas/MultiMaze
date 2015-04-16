using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text FinishText;

    public static GameManager Instance { get; set; }

    public GameObject BallPrefab;
    public Material[] playerColors;
    private GameObject[] players;
    private Rigidbody[] playerRigidbody;

    public Animator UiInstructions;

    private IntVector2 startCoordinates;

    void Start()
    {
        createPlayers();
        BeginGame();
        Instance = this;
    }

    private void createPlayers()
    {
        var numberPlayers = playerColors.Length;
        players = new GameObject[numberPlayers];
        playerRigidbody = new Rigidbody[numberPlayers];
        for (int i = 0; i < numberPlayers; i++)
        {
            players[i] = Instantiate(BallPrefab);
            players[i].name = playerColors[i].name + " Ball";
            players[i].GetComponent<UnityStandardAssets.Vehicles.Ball.BallUserControl>().SetBallMaterial(playerColors[i]);
            playerRigidbody[i] = players[i].GetComponent<Rigidbody>();
        }
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
        var startSpots = mazeInstance.Generate(startCoordinates, playerColors.Length);

        for (int i = 0; i < playerColors.Length; i++)
        {
            players[i].transform.position
                = startSpots[i % startSpots.Length]
                  .ToCellcenter(mazeInstance.size);
        }

        BeginMenu();
    }

    public void BeginMenu()
    {
        UiInstructions.SetTrigger("FadeOut");
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);
        FinishText.text = "";

        foreach (var rigidbody in playerRigidbody)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        Time.timeScale = 1;
        BeginGame();
    }

    public void Winner(string player)
    {
        FinishText.text = player + " won!";
        Time.timeScale = 0;
    }
}
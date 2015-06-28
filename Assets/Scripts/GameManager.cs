using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum GameState
{
    Start,
    Playing,
    Win,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public TextFader GameText;
    public GameObject BallPrefab;
    public Material[] playerColors;

    private GameObject[] players;
    private Rigidbody[] playerRigidbody;

    public Animator UiInstructions;
    public Text RestartText;

    private IntVector2 startCoordinates;
    private float timeHoldingScreenCenter;
    private int timeHoldingStepCompleted;
    private GameState gameState;

    void Start()
    {
        RestartText.text = "Hold here for a new maze";
        createPlayers();
        BeginGame();
        Instance = this;

        this.Delay(0, () => GameText.ShowText("First color to the Checkered Flag wins the game!"));
        UiInstructions.SetTrigger("FadeOut"); // Remove tutorial overlay
        gameState = GameState.Playing;
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
            if (gameState == GameState.Win) // player is viewing the win screen
            {
                RestartGame();
            }
            else
            {
                timeHoldingScreenCenter = Time.time;
            }
        }
        else if (UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            // reset for next countdown
            timeHoldingScreenCenter = 0;
            timeHoldingStepCompleted = 0;
            GameText.ShowText("");
        }
        else if (timeHoldingScreenCenter > float.Epsilon)
        {
            showCountdown();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.Win) // exit the win screen and return to the game
            {
                RestartGame();
            }
            else // go back to the settings page
            {
                Application.LoadLevel("Settings");
            }
        }
    }

    private void showCountdown()
    {
        float timeDelta = Time.time - timeHoldingScreenCenter;
        //Debug.Log(timeDelta + " step " + timeHoldingStepCompleted);
        if (timeDelta > 0 && timeDelta < 1 && timeHoldingStepCompleted == 0)
        {
            GameText.ShowText("New Maze in 3 seconds!");
            timeHoldingStepCompleted++;
        }
        else if (timeDelta > 1 && timeDelta < 2 && timeHoldingStepCompleted == 1)
        {
            GameText.ShowText("New Maze in 2 seconds!");
            timeHoldingStepCompleted++;
        }
        else if (timeDelta > 2 && timeDelta < 3 && timeHoldingStepCompleted == 2)
        {
            GameText.ShowText("New Maze in 1 second!");
            timeHoldingStepCompleted++;
        }
        else if (timeDelta > 3 && timeDelta < 4 && timeHoldingStepCompleted == 3)
        {
            RestartGame();
            timeHoldingStepCompleted++;
        }
        else if (timeDelta > 4 && timeDelta < 5 && timeHoldingStepCompleted == 4)
        {
            GameText.ShowText("Exiting in 3 seconds!");
            timeHoldingStepCompleted++;
        }
        else if (timeDelta > 5 && timeDelta < 6 && timeHoldingStepCompleted == 5)
        {
            GameText.ShowText("Exiting in 2 seconds!");
            timeHoldingStepCompleted++;
        }
        else if (timeDelta > 6 && timeDelta < 7 && timeHoldingStepCompleted == 6)
        {
            GameText.ShowText("Exiting in 1 second!");
            timeHoldingStepCompleted++;
        }
        else if (timeDelta > 7 && timeDelta < 8 && timeHoldingStepCompleted == 7)
        {
            Application.Quit();
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
            // move players into position
            players[i].transform.position
                = startSpots[i % startSpots.Length]
                  .ToCellcenter(mazeInstance.size);

            // insures the ball's are ready for a new game. Right now, this just clears ball trails.
            StartCoroutine(players[i].GetComponent<UnityStandardAssets.Vehicles.Ball.Ball>().ResetBall());
        }

        // Shows the tutorial overlay for every new maze
        // UiInstructions.SetTrigger("FadeOut");
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(mazeInstance.gameObject);

        foreach (var rigidbody in playerRigidbody)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        gameState = GameState.Playing;
        Time.timeScale = 1;
        BeginGame();
    }

    public void Winner(string player)
    {
        GameText.ShowText(player + " won!");

        RestartText.text = "Tap here for a new maze";
        UiInstructions.SetTrigger("FadeOut");

        gameState = GameState.Win;

        // Don't let the win text fade away until a new game is begun
        this.Delay(2, () =>
        {
            Time.timeScale = 0;
        });
    }
}
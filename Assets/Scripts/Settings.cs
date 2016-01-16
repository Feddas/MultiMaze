using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour
{
    void Start() { }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ModeTorque()
    {
        PlayerPref.SetInt(PlayerPrefEnum.ControlMode.ToString(), (int)ControlModeEnum.Torque);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MazeGrowingTree", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void ModeAttract()
    {
        PlayerPref.SetInt(PlayerPrefEnum.ControlMode.ToString(), (int)ControlModeEnum.Attract);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MazeGrowingTree", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void ModeTrace()
    {
        PlayerPref.SetInt(PlayerPrefEnum.ControlMode.ToString(), (int)ControlModeEnum.Trace);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MazeGrowingTree", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}

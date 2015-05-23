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
        PlayerPrefs.SetInt(PlayerPrefEnum.ControlMode.ToString(), (int)ControlModeEnum.Torque);
        Application.LoadLevel("MazeGrowingTree");
    }

    public void ModeDrag()
    {
        PlayerPrefs.SetInt(PlayerPrefEnum.ControlMode.ToString(), (int)ControlModeEnum.Drag);
        Application.LoadLevel("MazeGrowingTree");
    }
}

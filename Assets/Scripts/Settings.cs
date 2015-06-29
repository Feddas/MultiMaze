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
        Application.LoadLevel("MazeGrowingTree");
    }

    public void ModeDrag()
    {
        PlayerPref.SetInt(PlayerPrefEnum.ControlMode.ToString(), (int)ControlModeEnum.Drag);
        Application.LoadLevel("MazeGrowingTree");
    }
}

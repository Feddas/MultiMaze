using UnityEngine;
using System.Collections;

public class MazeFinish : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Winner(other.name);
        }
    }
}

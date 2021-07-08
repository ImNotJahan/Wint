using UnityEngine.SceneManagement;
using UnityEngine;

public class TeleportToDungeon : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(2);
        }
    }
}

using UnityEngine.SceneManagement;
using UnityEngine;

public class TelportToDungeon : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(1);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Singleplayer()
    {
        SceneManager.LoadScene(1);
    }

    public void Mods()
    {
        System.Diagnostics.Process.Start("explorer.exe", "/select," + @"C:\");
    }
}

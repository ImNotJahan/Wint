using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Singleplayer()
    {
        GameManager.instance.LoadGame();
    }

    public void Mods()
    {
        System.Diagnostics.Process.Start("explorer.exe", "/select," + @"C:\");
    }
}

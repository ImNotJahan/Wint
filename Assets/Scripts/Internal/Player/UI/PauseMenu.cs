using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;

    private void Start()
    {
        menu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(IniFiles.Keybinds.pause))
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);
        MouseLook.disabled = menu.activeSelf;

        Cursor.lockState = menu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = menu.activeSelf;

        if (!PhotonNetwork.InRoom) Time.timeScale = menu.activeSelf ? 0 : 1;
    }

    public void Save()
    {
        CharacterStats.currentPlayerInstance.Save();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

using UnityEngine;

public class Startup : MonoBehaviour
{
    private void Awake()
    {
        IniFiles.Keybinds.LoadKeybinds();
    }
}

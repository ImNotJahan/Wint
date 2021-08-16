using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeybindingHandler : MonoBehaviour
{
    [SerializeField] Transform inputMenu = null;
    [SerializeField] GameObject keybindInput = null;

    public void LoadKeybinds()
    {
        Transform keybindInputInstance = Instantiate(keybindInput).transform;
        keybindInputInstance.GetChild(0).GetComponent<Text>().text = "Forward";
        keybindInputInstance.GetChild(1).GetComponent<Text>().text = IniFiles.Keybinds.forward;
    }
}

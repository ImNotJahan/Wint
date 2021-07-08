using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text text;
    public RoomInfo info;

    public void Setup(RoomInfo info)
    {
        this.info = info;
        text.text = info.Name;
    }

    public void OnClick()
    {
        Launcher.instance.JoinRoom(info);
    }
}

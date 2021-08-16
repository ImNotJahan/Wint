using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField roomNameInput;
    public Text roomText;

    public Transform roomItems;
    public GameObject roomItem;

    public Text playersInRoom;

    public GameObject room;
    public GameObject find;
    public GameObject startGameButton;

    public InputField tempUsernameInput;

    public static Launcher instance;

    void Start()
    {
        instance = this;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinCallback(string id)
    {
        PhotonNetwork.JoinRoom(id);
    }

    public void SetUsername()
    {
        if(!string.IsNullOrEmpty(tempUsernameInput.text)) PhotonNetwork.NickName = tempUsernameInput.text;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = "Player." + RandomString(4);
    }

    public void CreateRoom()
    {
        string roomName = roomNameInput.text;
        if (string.IsNullOrEmpty(roomName))
        {
            roomName = RandomString(10);
        }
        PhotonNetwork.CreateRoom(roomName);
    }

    private static System.Random random = new System.Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public override void OnJoinedRoom()
    {
        roomText.text = PhotonNetwork.CurrentRoom.Name;
        refreshPlayerlist();

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        DiscordController.CheckPresence("In a room - " + PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.Name,
            PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(2);
        DiscordController.CheckPresence("Slaying Minotaurs");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(Transform trans in roomItems)
        {
            Destroy(trans.gameObject);
        }

        for(int k = 0; k < roomList.Count(); k++)
        {
            if (roomList[k].RemovedFromList) continue;
            Instantiate(roomItem, roomItems).GetComponent<RoomItem>().Setup(roomList[k]);
        }
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        find.SetActive(false);
        room.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        refreshPlayerlist();
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        refreshPlayerlist();
    }

    void refreshPlayerlist()
    {
        DiscordController.CheckPresence("In a room - " + PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.Name,
            PhotonNetwork.CurrentRoom.PlayerCount);

        playersInRoom.text = "";

        for (int k = 0; k < PhotonNetwork.PlayerList.Length; k++)
        {
            playersInRoom.text += PhotonNetwork.PlayerList[k].NickName + "\n";
        }
    }
}

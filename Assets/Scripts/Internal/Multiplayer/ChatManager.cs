using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    InputField chatInput = null;
    Text chatText = null;
    GameObject chat = null;

    [SerializeField] PhotonView pv = null;
    [SerializeField] PlayerMovementScript movementScript = null;
    [SerializeField] MouseLook mouseLook = null;

    private void Awake()
    {
        chat = RoomManager.instance.chat;
        chatText = chat.transform.GetChild(0).GetComponent<Text>();
        chatInput = chat.transform.GetChild(1).GetComponent<InputField>();

        chat.SetActive(false);
    }

    private void Update()
    {
        if(pv.IsMine || !PhotonNetwork.InRoom)
        {
            if(Input.GetKeyUp(IniFiles.Keybinds.chat))
            {
                if (chatInput.isFocused && chatInput.text.Length > 0)
                {
                    pv.RPC("SendChat", RpcTarget.All, chatInput.text);
                    StartCoroutine(HideChat());

                    chatInput.text = "";

                    movementScript.disabled = false;
                    mouseLook.disabled = false;
                }
                else
                {
                    chat.SetActive(true);
                    chatInput.Select();

                    movementScript.disabled = true;
                    mouseLook.disabled = true;
                }
            }
        }
    }

    [PunRPC]
    private void SendChat(string message)
    {
        chatText.text += "\n" + pv.Owner.NickName + ": " + message;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            chat.SetActive(true);
            StartCoroutine(HideChat());
        }
    }

    IEnumerator HideChat()
    {
        yield return new WaitForSeconds(5);
        if (!chatInput.isFocused)
        {
            chat.SetActive(false);

            movementScript.disabled = false;
            mouseLook.disabled = false;
        }
    }
}
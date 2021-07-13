using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject playerCamera = null;
    [SerializeField] CharacterController playerCharacterController = null;
    [SerializeField] Focus focus = null;
    [SerializeField] PlayerMovementScript playerMovementScript = null;
    [SerializeField] TextMeshPro text = null;

    public GameObject cooldownBar = null;

    private MouseLook mouseLook;
    private Interact interact;
    private PhotonView pv;
    private CharacterCombat combat;

    void Awake()
    {
        pv = GetComponent<PhotonView>();

        if (PhotonNetwork.InRoom) text.text = pv.Owner.NickName;

        if (pv.IsMine || !PhotonNetwork.InRoom)
        {
            playerCamera.SetActive(true);
            playerCharacterController.enabled = true;
            focus.enabled = true;
            playerMovementScript.enabled = true;

            mouseLook = playerCamera.GetComponent<MouseLook>();
            interact = playerCamera.GetComponent<Interact>();
            combat = playerCharacterController.gameObject.GetComponent<CharacterCombat>();

            mouseLook.status = RoomManager.instance.status;
            mouseLook.styles = RoomManager.instance.styles;

            interact.uihandler = RoomManager.instance.uiHandler;

            CharacterStats.currentPlayerInstance = combat.myStats;
        }
    }

    private void Start()
    {
        combat.cooldownBar = cooldownBar;
    }
}
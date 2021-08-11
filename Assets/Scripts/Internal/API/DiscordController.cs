using System;
using UnityEngine;

[Serializable]
public class DiscordJoinEvent : UnityEngine.Events.UnityEvent<string> { }

[Serializable]
public class DiscordSpectateEvent : UnityEngine.Events.UnityEvent<string> { }

[Serializable]
public class DiscordJoinRequestEvent : UnityEngine.Events.UnityEvent<DiscordRpc.DiscordUser> { }

public class DiscordController : MonoBehaviour
{
    public static int timestamp = 0;
    public DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
    public string applicationId = "852535107906437140";
    public DiscordRpc.DiscordUser joinRequest;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;
    public UnityEngine.Events.UnityEvent hasResponded;
    public DiscordJoinEvent onJoin;
    public DiscordJoinEvent onSpectate;
    public DiscordJoinRequestEvent onJoinRequest;
    public static DiscordController instance;

    DiscordRpc.EventHandlers handlers;

    public void RequestRespondYes()
    {
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.Yes);
        hasResponded.Invoke();
    }

    public void RequestRespondNo()
    {
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.No);
        hasResponded.Invoke();
    }

    DiscordRpc.DiscordUser user;
    public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
    {
        onConnect.Invoke();
        CheckPresence("Choosing what to do");

        user = connectedUser;
        MenuLoaded();
    }

    public void MenuLoaded()
    {
        if (!string.IsNullOrEmpty(user.username))
        {
            //GameObject.Find("Canvas/Multiplayer/UsernameInput").transform.GetChild(1)
                //.GetComponent<Text>().text = user.username;
        }
    }

    public static void CheckPresence(string action, string partyId = "", int amountOfPeople = 1)
    {
        DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();

        presence.state = amountOfPeople == 1 ? "Solo" : "In a party";
        presence.details = action;
        presence.startTimestamp = timestamp;
        presence.endTimestamp = 0;
        presence.largeImageText = "Level 1";
        presence.largeImageKey = "icon";

        if(partyId != "")
        {
            presence.joinSecret = partyId;
            presence.partyId = "scret";
            presence.matchSecret = "motch";
            presence.partySize = amountOfPeople;
            presence.partyMax = 10;
            presence.instance = true;
        }

        DiscordRpc.UpdatePresence(presence);
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        Debug.Log(message);
    }

    public void JoinCallback(string secret)
    {

        onJoin.Invoke(secret);
    }

    public void SpectateCallback(string secret)
    {
        onSpectate.Invoke(secret);
    }

    public void RequestCallback(ref DiscordRpc.DiscordUser request)
    {
        joinRequest = request;
        onJoinRequest.Invoke(request);
    }

    void Start()
    {
        timestamp = DateTime.UtcNow.Second;
        instance = this;
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();
    }

    void OnEnable()
    {
        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback += ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        handlers.joinCallback += JoinCallback;
        handlers.spectateCallback += SpectateCallback;
        handlers.requestCallback += RequestCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, "");
    }

    void OnDisable()
    {
        DiscordRpc.Shutdown();
    }

    void OnDestroy()
    {

    }
}

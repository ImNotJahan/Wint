using System;
using UnityEngine;
using UnityEngine.UI;

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
    public string optionalSteamId;
    public DiscordRpc.DiscordUser joinRequest;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;
    public UnityEngine.Events.UnityEvent hasResponded;
    public DiscordJoinEvent onJoin;
    public DiscordJoinEvent onSpectate;
    public DiscordJoinRequestEvent onJoinRequest;

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
        CheckPresence("Slaying Minotaurs");

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

    public static void CheckPresence(string state)
    {
        int amountOfPlayers = 1;
        DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();

        presence.state = state;
        presence.details = amountOfPlayers == 1 ? "Solo" : "In a party";
        presence.startTimestamp = timestamp;
        presence.endTimestamp = 0;
        presence.smallImageText = "ImNotJahan";
        presence.largeImageText = "Level 1";
        presence.largeImageKey = "game_icon";
        presence.partyId = "ae488379-351d-4a4f-ad32-2b9b01c91657";
        presence.partySize = amountOfPlayers;
        presence.partyMax = 5;
        presence.joinSecret = "MTI4NzM0OjFpMmhuZToxMjMxMjM= ";

        DiscordRpc.UpdatePresence(presence);
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
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
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    void OnDisable()
    {
        DiscordRpc.Shutdown();
    }

    void OnDestroy()
    {

    }
}

using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
    public GameObject playerPrefab;

    public UIHandler uiHandler;
    public GameObject status;
    public GameObject styles;

    public GameObject chat;

    public GameObject cooldownBar;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        if (!PhotonNetwork.InRoom)
        {
            GameObject player = Instantiate(playerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
            player.GetComponent<PlayerManager>().cooldownBar = cooldownBar;
            ChunkGenerator.viewer = player.transform.GetChild(0);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoader;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoader;
    }

    void OnSceneLoader(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1 && PhotonNetwork.InRoom)
        {
            PhotonNetwork.Instantiate(@"Prefabs\Player", new Vector3(Random.Range(-10, 10), 2,
                Random.Range(-10, 10)), Quaternion.identity);
        }
    }
}

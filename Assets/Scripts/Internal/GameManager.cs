using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject loading = null;
    [SerializeField] private Image bar = null;

    [SerializeField] private Text objText = null;
    [SerializeField] private Transform obj = null;

    [SerializeField] private DiscordController controller = null;

    private void Awake()
    {
        instance = this;
        objText.text = obj.GetComponent<PickableItem>().item.description;
        loading.SetActive(false);

        scenesLoading.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive));

        
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    public void LoadGame()
    {
        scenesLoading.Add(SceneManager.UnloadSceneAsync(1));
        loading.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive));

        StartCoroutine(LoadProgress());
    }

    IEnumerator LoadProgress()
    {
        foreach(AsyncOperation scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                float progress = 0;
                foreach(AsyncOperation operation in scenesLoading)
                {
                    progress += operation.progress;
                }

                bar.fillAmount = progress / scenesLoading.Count;
                yield return null;
            }
        }

        loading.SetActive(false);
    }

    IEnumerator LoadProgressMenu()
    {
        while (!scenesLoading[0].isDone)
        {
            yield return null;
        }

        controller.MenuLoaded();
    }
}
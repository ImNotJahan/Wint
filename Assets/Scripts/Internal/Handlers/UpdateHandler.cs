using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

public class VersionData
{
    public string version;
    public string[] displayInfo = new string[2];
    public string[] changedBundles;
    public int changedScripts;
}

public class UpdateHandler : MonoBehaviour
{
    public GameObject updateButton;
    private VersionData data;

    void Awake()
    {
        StartCoroutine(GetRequest("https://www.jahanrashidi.com/wint/version.json"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                UnityEngine.Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                data = JsonUtility.FromJson<VersionData>(webRequest.downloadHandler.text);

                /*Debug.Log("Latest version: " + data.version + ", current version: " + Application.version);
                Debug.Log("Update name: " + data.displayInfo[0] + ", update description: "+ data.displayInfo[1]);
                Debug.Log("Changed bundles:" + data.changedBundles);*/

                if(data.version != Application.version)
                {
                    updateButton.SetActive(true);
                }
            }
        }
    }

    public void UpdateGame()
    {
        if(data.changedScripts == 1)
        {
            Process.Start(Application.dataPath + "/../Update.bat");
        }
    }
}

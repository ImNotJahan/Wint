using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static MultidimensionalArray;

public class UIHandler : MonoBehaviour
{
    private UnityEvent onEsc = new UnityEvent();
    private UnityAction[] setResponseId = new UnityAction[] { response0, response1, response2 };

    [SerializeField] private Text questAppearedText = null;

    static void response0()
    {
        NPCBase.responseId2 = 0;
    }

    static void response1()
    {
        NPCBase.responseId2 = 1;
    }

    static void response2()
    {
        NPCBase.responseId2 = 2;
    }

    private void Start()
    {
        dialogBox.SetActive(false);
        for (int k = 0; k < dialogButtons.Length; k++)
        {
            dialogButtons[k].SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            onEsc.Invoke();
        }
    }

    public GameObject dialogBox;
    public GameObject[] dialogButtons;
    public void displayMessage(string message, string title, Arr responses, UnityAction call)
    {
        dialogBox.SetActive(true);
        dialogBox.transform.GetChild(0).GetComponent<Text>().text = title;
        dialogBox.transform.GetChild(1).GetComponent<Text>().text = message;

        for (int k = 0; k < responses.Length(); k++)
        {
            dialogButtons[k].SetActive(true);
            dialogButtons[k].transform.GetChild(0).GetComponent<Text>().text = responses[k];
            dialogButtons[k].GetComponent<Button>().onClick.AddListener(call);
        }

        onEsc.AddListener(closeMessage);
    }

    public void displayMessage(string message, string title, Arr responses, UnityAction call, NPCBase npc, int id)
    {
        dialogBox.SetActive(true);
        dialogBox.transform.GetChild(0).GetComponent<Text>().text = title;
        dialogBox.transform.GetChild(1).GetComponent<Text>().text = message;

        npc.responseId = id;

        for (int k = 0; k < dialogButtons.Length; k++)
        {
            dialogButtons[k].SetActive(false);
        }

        for (int k = 0; k < responses.Length(); k++)
        {
            dialogButtons[k].SetActive(true);
            dialogButtons[k].transform.GetChild(0).GetComponent<Text>().text = responses[k];
            dialogButtons[k].GetComponent<Button>().onClick.RemoveAllListeners();
            dialogButtons[k].GetComponent<Button>().onClick.AddListener(setResponseId[k]);
            dialogButtons[k].GetComponent<Button>().onClick.AddListener(call);
        }

        onEsc.AddListener(closeMessage);
    }

    private void closeMessage()
    {
        dialogBox.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;

        onEsc.RemoveListener(closeMessage);
    }

    public void questAdded(string title)
    {
        questAppearedText.text = "QUEST UNLOCKED:\n" + title;
        StartCoroutine(hideAfter(questAppearedText.gameObject, 3));
    }

    IEnumerator hideAfter(GameObject gameObj, float seconds)
    {
        gameObj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        gameObj.SetActive(false);
    }
}
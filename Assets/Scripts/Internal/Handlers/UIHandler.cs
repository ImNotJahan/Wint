using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static MultidimensionalArray;

public class UIHandler : MonoBehaviour
{
    private UnityEvent onEsc = new UnityEvent();
    public static ParamEvent onMenuChange = new ParamEvent();

    [SerializeField] private Text questAppearedText = null;

    public int menuIndex = 0;
    public static GameObject currentMenu = null; //This is so other menus can close one another when opening

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
        graduallyLoadText(message.Length);

        for (int k = 0; k < responses.Length(); k++)
        {
            dialogButtons[k].SetActive(true);
            dialogButtons[k].transform.GetChild(0).GetComponent<Text>().text = responses[k];
            dialogButtons[k].GetComponent<Button>().onClick.AddListener(call);
        }

        onEsc.AddListener(closeMessage);
    }

    IEnumerator graduallyLoadText(int amountOfCharacters)
    {
        for(int k = 0; k < amountOfCharacters; k++)
        {
            dialogBox.transform.GetChild(1).GetComponent<Text>().resizeTextMaxSize = k;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void displayMessage(string message, string title, Arr responses, UnityAction call, NPCBase npc, int id)
    {
        menuIndex = 1;

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
            int responseId = k;

            dialogButtons[k].SetActive(true);
            dialogButtons[k].transform.GetChild(0).GetComponent<Text>().text = responses[k];
            dialogButtons[k].GetComponent<Button>().onClick.RemoveAllListeners();
            dialogButtons[k].GetComponent<Button>().onClick.AddListener(
                () => { NPCBase.responseId2 = responseId; });
            dialogButtons[k].GetComponent<Button>().onClick.AddListener(call);
        }

        onEsc.AddListener(closeMessage);
    }

    private void closeMessage()
    {
        dialogBox.SetActive(false);

        foreach(GameObject button in dialogButtons)
        {
            button.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        menuIndex = 0;

        onEsc.RemoveListener(closeMessage);
    }

    public void questAdded(string title)
    {
        questAppearedText.text = "QUEST UNLOCKED:\n" + title.ToUpper();
        StartCoroutine(hideAfter(questAppearedText.gameObject, 3));
    }

    IEnumerator hideAfter(GameObject gameObj, float seconds)
    {
        gameObj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        gameObj.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    UnityEvent onClick = new UnityEvent();

    private void Start()
    {
        dialogBox.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            onClick.Invoke();
        }
    }

    public GameObject dialogBox;
    public void displayMessage(string message, string title, int id)
    {
        dialogBox.SetActive(true);
        dialogBox.transform.GetChild(0).GetComponent<Text>().text = title;
        dialogBox.transform.GetChild(1).GetComponent<Text>().text = message;

        onClick.AddListener(closeMessage);
    }

    private void closeMessage()
    {
        dialogBox.SetActive(false);

        onClick.RemoveListener(closeMessage);

        //move this to interact
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
}
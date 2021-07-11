using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    [SerializeField] GameObject quest = null;
    [SerializeField] GameObject questPrefab = null;
    [SerializeField] Transform questList = null;

    [SerializeField] Text title = null;
    [SerializeField] Text description = null;

    CharacterStats stats;

    private void Start()
    {
        quest.SetActive(false);
        stats = CharacterStats.currentPlayerInstance;
    }

    private void Update()
    {
        if (Input.GetKeyUp(IniFiles.Keybinds.log))
        {
            if (!quest.activeSelf)
            {
                foreach (Transform item in questList)
                {
                    Destroy(item.gameObject);
                }

                foreach(Quest quest in stats.log)
                {
                    GameObject questItem = Instantiate(questPrefab, questList);
                    questItem.transform.GetChild(0).GetComponent<Text>().text = quest.title;

                    QuestItem questItemComponent = questItem.GetComponent<QuestItem>();
                    questItemComponent.title = quest.title;
                    questItemComponent.description = quest.description;

                    questItemComponent.questTitle = title;
                    questItemComponent.questDescription = description;

                    questItem.GetComponent<Button>().onClick.AddListener(questItemComponent.DisplayItem);
                }

                title.text = "";
                description.text = "";
            }

            quest.SetActive(!quest.activeSelf);

            MouseLook.disabled = quest.activeSelf;

            Cursor.lockState = quest.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = quest.activeSelf;
        }
    }
}
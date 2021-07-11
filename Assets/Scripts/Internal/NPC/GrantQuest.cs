using UnityEngine;

[RequireComponent(typeof(NPCBase))]
public class GrantQuest : MonoBehaviour
{
    public string dialog;
    public Quest quest;
    public UIHandler uihandler;

    void Start()
    {
        GetComponent<NPCBase>().onDialog.AddListener(OnDialog);
    }

    private void OnDialog(string[] arg)
    {
        if(arg[0] == dialog)
        {
            CharacterStats.currentPlayerInstance.log.Add(quest);
            uihandler.questAdded(quest.title);
        }
    }
}

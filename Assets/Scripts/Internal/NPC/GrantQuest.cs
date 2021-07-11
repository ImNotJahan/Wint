using UnityEngine;

[RequireComponent(typeof(NPCBase))]
public class GrantQuest : MonoBehaviour
{
    public string dialog;
    public Quest quest;

    void Start()
    {
        GetComponent<NPCBase>().onDialog.AddListener(OnDialog);
    }

    private void OnDialog(string[] arg)
    {
        if(arg[0] == dialog)
        {
            CharacterStats.currentPlayerInstance.log.Add(quest);
        }
    }
}

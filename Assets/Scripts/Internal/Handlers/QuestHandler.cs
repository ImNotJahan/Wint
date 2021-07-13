using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    public Quest[] quests = new Quest[] { };

    void Start()
    {
        CharacterStats.currentPlayerInstance.log.Add(quests[0]);
    }

    private void OnDeath(string[] args)
    {

    }
}

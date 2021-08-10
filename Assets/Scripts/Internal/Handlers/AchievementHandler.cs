using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AchievementHandler : MonoBehaviour
{
    public static ParamEvent entityDeath = new ParamEvent();
    public static ParamEvent achievementAdded = new ParamEvent();

    public GameObject achievementPopup;
    public Text achievementText;
    public Image achievementImage;

    private void Awake()
    {
        entityDeath.AddListener(EntityDeath);
        achievementAdded.AddListener((string[] args) => { StartCoroutine(AchievementAdded(args[0])); } );
    }

    private void EntityDeath(string[] args)
    {
        if(args.Length > 0)
        {
            switch (args[0])
            {
                case "minotaur":
                    CharacterStats.currentPlayerInstance.achievements.Add("The Meaning of Adventure");
                    achievementAdded.Invoke(new string[] { "The Meaning of Adventure" });
                    break;
            }
        }
    }

    private IEnumerator AchievementAdded(string title)
    {
        string achievementId = title.ToLower();
        achievementId = Regex.Replace(achievementId, "[^a-z ]", string.Empty);
        achievementId = achievementId.Replace(" ", "_");

        achievementText.text = title;
        achievementPopup.SetActive(true);

        yield return new WaitForSeconds(4);

        achievementPopup.SetActive(false);
    }
}

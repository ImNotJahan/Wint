using UnityEngine;
using UnityEngine.UI;

public class LocalHealthbar : MonoBehaviour
{
    public Image healthbar;
    private CharacterStats stats;

    private void Start()
    {
        stats = CharacterStats.currentPlayerInstance;
        stats.onTakeDamage.AddListener(onTakeDamage);

        UIHandler.onMenuChange.AddListener(OnMenuChange);
    }

    private void onTakeDamage(string[] args)
    {
        healthbar.fillAmount = (float)stats.health / stats.maxHealth; 
    }

    private void OnMenuChange(string[] args)
    {
        if (args.Length > 0) healthbar.transform.parent.gameObject.SetActive(args[0] == "off");
    }
}

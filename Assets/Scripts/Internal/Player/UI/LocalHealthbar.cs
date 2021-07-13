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
    }

    private void onTakeDamage(string[] args)
    {
        healthbar.fillAmount = stats.health / stats.maxHealth; 
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public GameObject healthbar;
    public Transform target;
    public CharacterCombat combat;
    Image slider;

    private void Start()
    {
        combat.myStats.onTakeDamage.AddListener(updateHealth);

        Transform inst = Instantiate(healthbar, target).transform;
        inst.localScale = inst.localScale / target.parent.localScale.x;

        slider = inst.GetChild(0).GetComponent<Image>();
        slider.fillAmount = combat.myStats.maxHealth / combat.myStats.health;
    }

    void updateHealth(string[] args)
    {
        slider.fillAmount = (float)combat.myStats.health / combat.myStats.maxHealth;
    }
}

using UnityEngine;

public class Fist : MonoBehaviour
{
    public int damage = 10;
    public GameObject bloodEffect;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.root == transform.root)
            return;

        CharacterStats targetStats = collision.transform.GetComponent<CharacterCombat>().myStats;
        if (targetStats != null)
        {
            targetStats.TakeDamage(damage);
        }
    }
}

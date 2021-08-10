using UnityEngine;

public class BreakableItem : MonoBehaviour
{
    public int health = 50;
    public Element madeOf = Element.Wood;
    public GameObject drops;

    public void Attack(int damage)
    {
        health -= damage;

        GetComponent<AudioSource>().Play(0);

        if (health < 1)
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Instantiate(drops, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
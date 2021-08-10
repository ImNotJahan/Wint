using System.Collections;
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
        Instantiate(drops, transform.position, transform.rotation).SetActive(true);

        GetComponent<Renderer>().enabled = false;
        GetComponents<Collider>()[0].enabled = false;
        GetComponents<Collider>()[1].enabled = false;

        StartCoroutine(DestroyAfterAudio());
    }

    IEnumerator DestroyAfterAudio()
    {
        while (GetComponent<AudioSource>().isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
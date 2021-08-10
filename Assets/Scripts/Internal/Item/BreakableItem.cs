using System.Collections;
using UnityEngine;

public class BreakableItem : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<AudioSource>().enabled = false;
    }

    public int health = 50;
    public Element madeOf = Element.Wood;
    public GameObject drops;

    public void Attack(int damage)
    {
        health -= damage;

        GetComponent<AudioSource>().enabled = true;
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
        GetComponent<Collider>().enabled = false;

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
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu()]
public class Quest : ScriptableObject
{
    public UnityEvent onCompletion = new UnityEvent();
    public string title;
    public string description;
    [HideInInspector] public Item[] reward = new Item[0];

    public Quest(string title, string description)
    {
        this.title = title;
        this.description = description;
    }

    public Quest(string title, string description, Item[] reward)
    {
        this.title = title;
        this.description = description;

        this.reward = reward;
    }

    public Quest(string title, string description, UnityEvent _event)
    {
        this.title = title;
        this.description = description;

        _event.AddListener(Complete);
    }

    public Quest(string title, string description, Item[] reward, UnityEvent _event)
    {
        this.title = title;
        this.description = description;

        this.reward = reward;

        _event.AddListener(Complete);
    }

    public void Complete()
    {
        foreach(Item item in reward)
        {
            CharacterStats.currentPlayerInstance.inventory.Add(item);
        }
        onCompletion.Invoke();
    }
}

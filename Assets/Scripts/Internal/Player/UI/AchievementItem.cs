using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchievementItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string title = "";
    public string description = "";
    public bool unlocked = false;

    private GameObject hover;
    private Text hoverTitle;
    private Text hoverDescription;

    private void Start()
    {
        hover = gameObject.transform.GetChild(0).gameObject;
        hoverTitle = hover.transform.GetChild(0).GetComponent<Text>();
        hoverDescription = hover.transform.GetChild(1).GetComponent<Text>();

        hoverTitle.text = title;

        hover.SetActive(false);

        if (unlocked) Unlock();
        else AchievementHandler.achievementAdded.AddListener(AchievementAdded);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        hover.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        hover.SetActive(false);
    }

    private void AchievementAdded(string[] args)
    {
        if (args.Length > 0 && args[0] == title) Unlock();
    }

    public void Unlock()
    {
        hoverDescription.text = description;

        string achievementId = title.ToLower();
        achievementId = Regex.Replace(achievementId, "[^a-z ]", string.Empty);
        achievementId = achievementId.Replace(" ", "_");

        GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/achievements/" + achievementId);

        unlocked = true;
    }
}

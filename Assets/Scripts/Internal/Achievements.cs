using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public Dictionary<string[], bool> achievements = new Dictionary<string[], bool>();

    private void Awake()
    {
        AddAchievement("The Meaning of Adventure", "Kill a minotaur");
        AddAchievement("Art of the Deal", "Buy the napkin for ten million dollars");
        AddAchievement("7.6 Million Years", "Reach level 42");
        AddAchievement("I've Heard it Both Ways", "Convince a hired companion to pay you for their work");
        AddAchievement("The Crocodile Hunter", "Tame a creature");
        AddAchievement("Is it That Bad?", "Kill yourself");
    }

    private void AddAchievement(string title, string description)
    {
        achievements.Add(new string[] { title, description }, false);
    }
}

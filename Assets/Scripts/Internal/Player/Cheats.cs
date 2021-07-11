using UnityEngine;

public class Cheats : MonoBehaviour
{
    public static void Check(string text)
    {
        string[] cheatParams = text.Split(' ');

        switch (cheatParams[0])
        {
            case "!give":
                if (cheatParams.Length > 1 && ItemList.items.ContainsKey(cheatParams[1]))
                {
                    CharacterStats.currentPlayerInstance.inventory.Add(ItemList.items[cheatParams[1]]);
                }
                break;
        }
    }
}

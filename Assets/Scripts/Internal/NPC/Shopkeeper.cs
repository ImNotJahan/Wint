using UnityEngine;

public class Shopkeeper : NPCBase
{
    /**
     * This array contains the (if any) buyable item at (responseId + responseId)
     * responseId + responseId2 is equal to the actual response from the player, probably should refactor for more clairity
     * **/
    [SerializeField] private Item[] shopItems = null;
    [SerializeField] private Item[] shopCosts = null;

    public new void HandleClick()
    {
        base.HandleClick();

        int id = responseId + responseId2;

        if(shopItems.Length > id && shopItems[id] != null)
        {
            var inventory = PlayerMovementScript.instance.characterStats.inventory;
            if (inventory.Contains(shopCosts[id]))
            {
                inventory.Remove(shopCosts[id]);
                inventory.Add(shopItems[id]);
            }
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject inventory = null;
    [SerializeField] GameObject invDetails = null;
    [SerializeField] GameObject invPrefab = null;
    [SerializeField] Transform invList = null;

    [SerializeField] CharacterCombat stats = null;

    private void Update()
    {
        if (Input.GetKeyUp(IniFiles.Keybinds.inventory))
        {
            if (!inventory.activeSelf)
            {
                inventory.SetActive(true);
                
                foreach(Item item in stats.myStats.inventory)
                {
                    GameObject invItem = Instantiate(invPrefab, invList);
                    invItem.transform.GetChild(0).GetComponent<Text>().text = item.item_id;
                }
            }
            else inventory.SetActive(false);
        }
    }
}
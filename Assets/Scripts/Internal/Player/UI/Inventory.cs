using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject inventory = null;
    [SerializeField] GameObject invPrefab = null;
    [SerializeField] Transform invList = null;

    [SerializeField] Text title = null;
    [SerializeField] Text description = null;

    CharacterStats stats;

    private void Start()
    {
        inventory.SetActive(false);
        stats = CharacterStats.currentPlayerInstance;
    }

    private void Update()
    {
        if (Input.GetKeyUp(IniFiles.Keybinds.inventory))
        {
            if (!inventory.activeSelf)
            {
                foreach (Transform item in invList)
                {
                    Destroy(item.gameObject);
                }

                Dictionary<string, int> invCache = new Dictionary<string, int>();

                foreach (Item item in stats.inventory)
                {
                    if (invCache.ContainsKey(item.item_id)) invCache[item.item_id]++;
                    else invCache[item.item_id] = 1;

                    GameObject invItem = Instantiate(invPrefab, invList);
                    invItem.transform.GetChild(0).GetComponent<Text>().text = item.item_id;

                    InventoryItem invItemComponent = invItem.GetComponent<InventoryItem>();
                    invItemComponent.title = item.item_id;
                    invItemComponent.description = item.description;
                    invItemComponent.count = invCache[item.item_id];

                    invItemComponent.invTitle = title;
                    invItemComponent.invDescription = description;

                    invItem.GetComponent<Button>().onClick.AddListener(invItemComponent.DisplayItem);
                }

                title.text = "";
                description.text = "";
            }

            inventory.SetActive(!inventory.activeSelf);

            MouseLook.disabled = inventory.activeSelf;

            Cursor.lockState = inventory.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = inventory.activeSelf;
        }
    }
}
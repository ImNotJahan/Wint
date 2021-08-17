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
    [SerializeField] Button drop = null;
    [SerializeField] Button equip = null;

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
                    if (invCache.ContainsKey(item.itemName)) invCache[item.itemName]++;
                    else invCache[item.itemName] = 1;

                    GameObject invItem = Instantiate(invPrefab, invList);
                    invItem.transform.GetChild(0).GetComponent<Text>().text = item.itemName;

                    string itemDescription = item.description;
                    Weapon weapon = item.gameItem.GetComponent<Weapon>();

                    if (item.gameItem.GetComponent<Weapon>() != null)
                    {
                        itemDescription += "\n";
                        itemDescription += "Damage: " + weapon.baseDamage + "\n";
                        itemDescription += "Variation: " + weapon.variation + "\n";
                        itemDescription += "Attack speed: " + weapon.attackSpeed + "\n";

                        equip.gameObject.SetActive(true);
                    } else equip.gameObject.SetActive(false);

                    InventoryItem invItemComponent = invItem.GetComponent<InventoryItem>();
                    invItemComponent.title = item.itemName;
                    invItemComponent.description = itemDescription;
                    invItemComponent.count = invCache[item.itemName];

                    invItemComponent.itemPrefab = item.gameItem;
                    invItemComponent.itemSelf = item;

                    invItemComponent.invTitle = title;
                    invItemComponent.invDescription = description;

                    drop.GetComponent<Button>().onClick.RemoveAllListeners();
                    equip.GetComponent<Button>().onClick.RemoveAllListeners();

                    drop.GetComponent<Button>().onClick.AddListener(invItemComponent.Drop);
                    equip.GetComponent<Button>().onClick.AddListener(invItemComponent.Equip);

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
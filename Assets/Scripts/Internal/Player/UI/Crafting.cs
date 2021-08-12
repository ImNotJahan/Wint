using UnityEngine;
using UnityEngine.UI;

namespace Crafting
{
    public class Crafting : MonoBehaviour
    {
        [SerializeField] GameObject inventory = null;
        [SerializeField] GameObject invPrefab = null;
        [SerializeField] Transform invList = null;

        [SerializeField] Text title = null;
        [SerializeField] Text ingredients = null;
        [SerializeField] CraftButton button = null;

        CharacterStats stats;

        private void Start()
        {
            inventory.SetActive(false);
            stats = CharacterStats.currentPlayerInstance;
        }

        private void Update()
        {
            if (Input.GetKeyUp(IniFiles.Keybinds.crafting))
            {
                if (!inventory.activeSelf)
                {
                    foreach (Transform item in invList)
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (Recipe recipe in stats.knownRecipes)
                    {
                        GameObject invItem = Instantiate(invPrefab, invList);
                        invItem.transform.GetChild(0).GetComponent<Text>().text = recipe.result.item_id;

                        CraftItem invItemComponent = invItem.GetComponent<CraftItem>();
                        invItemComponent.title = recipe.result;
                        invItemComponent.count = recipe.result.count;
                        invItemComponent.ingredients = recipe.ingredients;

                        invItemComponent.craftIngredients = ingredients;
                        invItemComponent.craftTitle = title;
                        invItemComponent.button = button;

                        invItem.GetComponent<Button>().onClick.AddListener(invItemComponent.DisplayItem);
                    }

                    title.text = "";
                }

                inventory.SetActive(!inventory.activeSelf);

                MouseLook.disabled = inventory.activeSelf;

                Cursor.lockState = inventory.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = inventory.activeSelf;
            }
        }
    }
}
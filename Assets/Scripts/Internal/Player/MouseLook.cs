using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Interact))]
public class MouseLook : MonoBehaviour
{
    [Range(50f, 1000f)]
    public float mouseSensitivity = 100;
    public Transform playerBody;

    public string defaultStatus;

    private float xRotation = 0f;

    public GameObject status;
    public GameObject styles;

    public static bool disabled = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!disabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * Time.timeScale;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * Time.timeScale;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerBody.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

     /*else if (Input.GetKeyUp(KeyCode.V))
        {
            if (!status.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;

                StringBuilder statusString = new StringBuilder("", 100);
                statusString.AppendFormat(defaultStatus, PlayerData.strength, PlayerData.endurance,
                    PlayerData.dexterity, PlayerData.agility, PlayerData.magic);

                status.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = statusString.ToString().Replace("\\n", "\n");
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
            }
            status.SetActive(!status.activeSelf);
        }*/
            if (Input.GetKeyDown(IniFiles.Keybinds.style))
            {
                styles.SetActive(true);
            }
            else if (Input.GetKeyUp(IniFiles.Keybinds.style))
            {
                styles.SetActive(false);
            }
        }
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
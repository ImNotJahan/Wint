using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class MouseLook : MonoBehaviour
{
    [Range(50f, 1000f)]
    public float mouseSensitivity = 100;
    public Transform playerBody;

    public string defaultStatus;

    private float xRotation = 0f;

    public GameObject status;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime * Time.timeScale;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime * Time.timeScale;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerBody.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (Input.GetKeyUp(KeyCode.E))
        {
            int layerMask = 1 << 8;

            layerMask = ~layerMask;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                transform.GetComponent<Interact>().ProcessInteract(hit.transform);
            }
        } else if (Input.GetKeyUp(KeyCode.V))
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
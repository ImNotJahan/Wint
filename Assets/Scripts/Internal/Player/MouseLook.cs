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
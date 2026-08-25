using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 1000;
    GameObject player;
    Gravity gravity;
    float xRotation;
    float yRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = GameObject.Find("Player");
        gravity = FindFirstObjectByType<Gravity>();
    }
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        player.transform.rotation = Quaternion.Euler(gravity.gravityRotation) * Quaternion.Euler(0, yRotation, 0);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -89, 89);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}

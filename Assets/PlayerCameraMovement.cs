using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        // Rotate the camera based on mouse input
        transform.parent.Rotate(Vector3.up * mouseX);
        transform.parent.Rotate(Vector3.left * mouseY);
        // Clamp the vertical rotation to prevent flipping
        Vector3 eulerAngles = transform.parent.eulerAngles;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -90f, 90f);
        transform.parent.eulerAngles = eulerAngles;
        // Optional: Reset the camera rotation with right-click
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, 0); // Reset to default rotation
        }        
    }
}

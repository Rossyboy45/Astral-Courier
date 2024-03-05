using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerCamera : MonoBehaviour
{
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityY;
    float horizontal;
    float vertical;
    [SerializeField] Rigidbody rb;
    

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        //input gathering
        float mouseX = horizontal * sensitivityX;
        float mouseY = vertical * sensitivityY;
        xRotation -= mouseY;
        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -1.3f, 1.25f);
        
        //rotate camera and body
        transform.rotation = quaternion.Euler(xRotation, yRotation, 0);
    }

    private void FixedUpdate()
    {
        rb.MoveRotation(quaternion.Euler(0, yRotation, 0));
    }

    public void Look(InputAction.CallbackContext context)
    {
            horizontal = context.ReadValue<Vector2>().x;
            vertical = context.ReadValue<Vector2>().y;
    }
}

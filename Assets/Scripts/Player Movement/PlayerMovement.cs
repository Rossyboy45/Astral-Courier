using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] LayerMask ground;

    //movement
    float xInput;
    float zInput;
    Vector3 moveDirection;
    Rigidbody rb;

    //Friction
    float playerHeight;
    bool grounded;
    float drag = 5;
    float moveSpeed = 20;

    //jumpuing
    float jumpForce = 8f;
    float AirMove = 0.3f;
    bool canJump = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, ground);
        moveDirection = orientation.forward * zInput + orientation.right * xInput;
        
        if (grounded == true) 
        {
            rb.drag = drag;
            canJump = true;
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        }
        else
        {
            rb.drag = 0;
            rb.AddForce(moveDirection.normalized * moveSpeed * AirMove, ForceMode.Force);
        }

        SpeedControl();
    }

    private void SpeedControl()
    {
        Vector3 maxSpeed = new Vector3(rb.velocity.x, 0f ,rb.velocity.z);

        if (maxSpeed.magnitude > moveSpeed)
        {
            Vector3 limit = maxSpeed.normalized * moveSpeed;
            rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
        }
    }

    //capture players inputs
    public void Move(InputAction.CallbackContext context)
    { 
            xInput = context.ReadValue<Vector2>().x;
            zInput = context.ReadValue<Vector2>().y;   
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && canJump == true)
        {
            rb.velocity = new Vector3 (rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }
}

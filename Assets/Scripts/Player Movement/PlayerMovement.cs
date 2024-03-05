using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] Transform groundPosition;
    [SerializeField] LayerMask ground;
    [SerializeField] Collider normalCollider;
    [SerializeField] Collider crouchCollider;
    [SerializeField] Rigidbody rb;

    //movement
    float xInput;
    float zInput;
    Vector3 moveDirection;
    

    //Friction
    bool grounded;
    float drag = 5f;
    float moveSpeed= 15;

    //jumpuing
    float jumpForce = 8f;
    float AirMove = 0.3f;
    bool canJump = true;

    //Crouch
    float crouchSpeed = 7.5f;
    float walkSpeed = 15f;

    //slopeHandle
    float maxSlopeAngle = 40;
    RaycastHit slopeHit;
    bool exitSlope;

    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        grounded = Physics.Raycast(groundPosition.position, Vector3.down, 0.25f, ground);
        moveDirection = orientation.forward * zInput + orientation.right * xInput;

        if (OnSlope() && !exitSlope)
        {
            rb.AddForce(MoveDirection() * moveSpeed * 10, ForceMode.Force);
            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

        }
        
        if (grounded == true) 
        {
            rb.drag = drag;
            canJump = true;
            exitSlope = false;
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.drag = 0;
            rb.AddForce(moveDirection.normalized * moveSpeed * AirMove, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
        SpeedControl();
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(groundPosition.position,Vector3.down,out slopeHit, 0.25f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 MoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection,slopeHit.normal).normalized;
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        else 
        {
            Vector3 maxSpeed = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (maxSpeed.magnitude > moveSpeed)
            {
                Vector3 limit = maxSpeed.normalized * moveSpeed;
                rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
            }
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
            exitSlope = true;
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            crouchCollider.enabled = true;
            normalCollider.enabled = false;
            moveSpeed = crouchSpeed;
        }
        else
        {
            normalCollider.enabled = true;
            crouchCollider.enabled = false;
            moveSpeed = walkSpeed;
        }
    }

}

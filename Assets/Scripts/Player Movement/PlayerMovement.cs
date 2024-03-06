using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] Transform groundPosition;
    [SerializeField] LayerMask ground;
    [SerializeField] Rigidbody rb;

    //movement
    public float xInput;
    public float zInput;
    Vector3 moveDirection;

    //Friction
    bool grounded;
    float drag = 5f;
    public float maxYSpeed;

    //jumpuing
    float jumpForce = 8f;
    float AirMove = 0.8f;
    bool canJump = true;

    //slopeHandle
    float maxSlopeAngle = 40;
    RaycastHit slopeHit;
    bool exitSlope;

    //slide
    public bool isSliding = false;
    float slideSpeed = 25;
    float walkSpeed = 15;
    public float moveSpeed;
    public float desiredMoveSpeed;

    //dash
    public bool isDashing = false;
    float dashSpeed = 40;



    // Start is called before the first frame update
    void Start()
    {
        rb.freezeRotation = true;
        moveSpeed = walkSpeed;
        desiredMoveSpeed = walkSpeed;
    }

    private void FixedUpdate()
    {

        grounded = Physics.Raycast(groundPosition.position, Vector3.down, 0.25f, ground);
        moveDirection = orientation.forward * zInput + orientation.right * xInput;

        if (isDashing == true)
        {
            StopAllCoroutines();
            moveSpeed = dashSpeed;
            desiredMoveSpeed = dashSpeed;
        }
        else
        {
            desiredMoveSpeed = walkSpeed;
        }

        if (OnSlope())
        {
            if (!exitSlope)
            {
                rb.AddForce(MoveDirection(moveDirection) * moveSpeed * 10, ForceMode.Force);
                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }
        }

        if (isSliding == true)
        { 
            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                StopAllCoroutines();
                moveSpeed = slideSpeed;
                desiredMoveSpeed = slideSpeed;
            }
            else
            {
                desiredMoveSpeed = walkSpeed;
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

        if (Mathf.Abs(moveSpeed - desiredMoveSpeed) > 4f && rb.velocity.magnitude > 1 && isDashing == false)
        {
            StopAllCoroutines();
            StartCoroutine(SLMS());
        }
        
        
        if (rb.velocity.magnitude < 1)
        {
            moveSpeed = desiredMoveSpeed;
        }

    }

    private IEnumerator SLMS()
    {
        float time = 0.4f;
        float diffrence = Mathf.Abs (moveSpeed - desiredMoveSpeed);
        float startValue = moveSpeed;

        while (time < diffrence)
        {
            moveSpeed = Mathf.Lerp (startValue,desiredMoveSpeed, time/diffrence);
            time += Time.deltaTime;
            yield return null;
        }

        desiredMoveSpeed = moveSpeed;
        
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

    public Vector3 MoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction,slopeHit.normal).normalized;
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
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

        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
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

}

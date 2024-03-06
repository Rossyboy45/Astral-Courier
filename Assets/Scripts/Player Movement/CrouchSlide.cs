using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchSlide : MonoBehaviour
{
    [SerializeField] Collider normalCollider;
    [SerializeField] Collider slideCollider;
    [SerializeField] Transform orientation;
    [SerializeField] Rigidbody rb;
    PlayerMovement pm;

    float slideForce = 1000;
    Vector3 inputDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (pm.isSliding == true)
        {
            slideMovement();
        }
        else
        {
            if (pm.xInput != 0 || pm.zInput != 0)
            {
                inputDirection = orientation.forward * pm.zInput + orientation.right * pm.xInput;
            }
            else
            {
                inputDirection = orientation.forward;
            }
        }
    }

    public void Slide(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pm.isSliding = true;
            slideCollider.enabled = true;
            normalCollider.enabled = false;
        }
        else
        {
            pm.isSliding = false;
            normalCollider.enabled = true;
            slideCollider.enabled = false;
        }
    }

    private void slideMovement()
    {
        if(!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        }
        else
        {
            rb.AddForce(pm.MoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        
    }
}

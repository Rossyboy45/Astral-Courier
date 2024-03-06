using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dashing : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerCam;
    [SerializeField] Rigidbody rb;
    PlayerMovement pm;

    float dashForce = 20;
    float dashUpwardsForce = 0.5f;
    float dashDuration = 0.25f;
    float maxDashYSpeed = 15;

    public float dashCharge = 100;
    float dashRegen = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        dashCharge = Mathf.MoveTowards(dashCharge, 100, dashRegen * Time.deltaTime);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && dashCharge > 49)
        {
            pm.isDashing = true;
            pm.maxYSpeed = maxDashYSpeed;
            dashCharge -= 50;
            Dash();
        }
    }

    private void Dash()
    {
        Transform forwardT = playerCam;
        Vector3 direction = GetDirection(forwardT);

        Vector3 forceToApply = direction * dashForce + direction * dashUpwardsForce;
        rb.AddForce(forceToApply, ForceMode.Impulse);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        pm.isDashing=false;
        pm.maxYSpeed = 0;
    }

    private Vector3 GetDirection(Transform forwardT)
    {
        Vector3 direction = new Vector3();

        if (pm.xInput != 0 || pm.zInput != 0)
        {
            direction = forwardT.forward * pm.zInput + forwardT.right *pm.xInput;
            
        }
        else
        {
            direction = forwardT.forward;
        }
        return direction.normalized;
    }

}

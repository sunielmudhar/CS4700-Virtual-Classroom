using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticipantController : MonoBehaviour
{
    // This script will handle all of the participant's movement and transformation

    [SerializeField] float fl_MouseSensitivity, fl_MovementSpeed, fl_SmoothTime;
    [SerializeField] GameObject cameraHolder;

    float fl_VerticalLookRotation;
    bool bl_CanMove = true;
    Vector3 smoothMovementVelocity;
    Vector3 movementAmount;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (bl_CanMove)
        {
            ParticipantMovement();
            ParticipantPanning();
        }
    }

    public void InActivity(int index)
    {
        if (index == 1)
        {
            bl_CanMove = false;
        }
        else
        {
            bl_CanMove = true;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(movementAmount) * Time.fixedDeltaTime);
    }

    public void ParticipantMovement()
    {
        Vector3 movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        movementAmount = Vector3.SmoothDamp(movementAmount, movementDirection, ref smoothMovementVelocity, fl_SmoothTime);
    } 

    public void ParticipantPanning()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * fl_MouseSensitivity);

        fl_VerticalLookRotation += Input.GetAxisRaw("Mouse Y") * fl_MouseSensitivity;
        fl_VerticalLookRotation = Mathf.Clamp(fl_VerticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * fl_VerticalLookRotation;
    }
}

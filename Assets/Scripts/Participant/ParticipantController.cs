using UnityEngine;
using Photon.Pun;

public class ParticipantController : MonoBehaviour
{
    // This script will handle all of the participant's movement and transformation
    /*Created with help of https://www.youtube.com/watch?v=AZRdwnBJcfg&t */

    [SerializeField] float fl_MouseSensitivity, fl_MovementSpeed, fl_SmoothTime;
    [SerializeField] GameObject cameraHolder;

    private PhotonView PV;

    float fl_VerticalLookRotation;
    [SerializeField] private bool bl_InActivity = false, bl_CanMove = true;
    Vector3 smoothMovementVelocity;
    Vector3 movementAmount;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (PV.IsMine && bl_CanMove)
        {
            ParticipantMovement();
            ParticipantPanning();
        }
    }

    public void InActivity(int index)
    {
        if (index == 1)
        {
            bl_InActivity = true;
            SetMovementState(0);
        }
        else
        {
            bl_InActivity = false;
            SetMovementState(1);
        }
    }

    public void SetMovementState(int index)
    {
        if (index == 1)
            bl_CanMove = true;
        else
            bl_CanMove = false;
    }

    public bool GetActivityState()
    {
        return bl_InActivity;
    }

    void FixedUpdate()
    {
        if (bl_CanMove)
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

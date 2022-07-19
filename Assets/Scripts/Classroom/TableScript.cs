using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour
{
    //NEED TO TARGET THE INSTANTIATED PREFAB!

    /*
    public Transform participant, chairPoint;
    public Vector3 sittingPlace;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Sitting();
        }        
    }

    public void Sitting()
    {
        //When this function is activate stop the participant from moving
        //participant.GetComponent<BoxCollider>().enabled = false;
        participant.GetComponent<Rigidbody>().useGravity = false;
        participant.GetComponent<ParticipantController>().InActivity(0);

        Debug.Log("Sitting");

        //Move the participant to the seat
        participant.position = Vector3.Lerp(participant.position, chairPoint.position + sittingPlace, 1);
        participant.rotation = Quaternion.Slerp(participant.rotation, chairPoint.rotation, 1);

        //Add code for animation will look something like the following:
        //participant.GetComponentInChildren<Animator>.SetBool("Sitting", true);
    } 
    */
}

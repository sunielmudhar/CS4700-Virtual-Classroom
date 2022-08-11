using UnityEngine;

public class TableScript : MonoBehaviour
{   
    public Transform[] chairPoint;
    public Vector3 sittingPlace;
    public GameObject participant;

    public void Sitting()
    {
        participant = GameObject.Find("CurrentParticipant");

        if (participant.tag.Equals("Student"))
        {
            for (int i = 0; i == chairPoint.Length; i++)
            {
                if (!chairPoint[i].GetComponent<InteractionPointManager>().IsOccupied())
                {
                    participant.GetComponent<ParticipantController>().InActivity(1);

                    //Move the participant to the seat
                    participant.transform.position = Vector3.Lerp(participant.transform.position, chairPoint[i].position + sittingPlace, 1);
                    participant.transform.rotation = Quaternion.Slerp(participant.transform.rotation, chairPoint[i].rotation, 1);

                    chairPoint[i].GetComponent<InteractionPointManager>().Occupy(true);
                }
            }
        }
    }

    public void StandUp()
    {
        participant = GameObject.Find("CurrentParticipant");

        if (participant.tag.Equals("Student"))
        {
            participant.GetComponent<ParticipantController>().InActivity(0);
        }
    }
}

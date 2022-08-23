using UnityEngine;

public class TableScript : MonoBehaviour
{   
    public Transform[] chairPoint;
    public Vector3 sittingPlace;

    public void Sitting(GameObject participant)
    {
        if (participant.tag.Equals("Student"))
        {
            foreach(Transform chair in chairPoint)
            {
                if (!chair.GetComponent<InteractionPointManager>().IsOccupied())
                {
                    participant.GetComponent<ParticipantController>().InActivity(1);

                    //Move the participant to the seat
                    participant.transform.position = Vector3.Lerp(participant.transform.position, chair.position + sittingPlace, 1);
                    participant.transform.rotation = Quaternion.Slerp(participant.transform.rotation, chair.rotation, 1);

                    chair.GetComponent<InteractionPointManager>().Occupy(true);

                    break;  //Break once a chair is found
                }
            }
        }
    }

    public void StandUp(GameObject participant)
    {
        if (participant.tag.Equals("Student"))
        {
            participant.GetComponent<ParticipantController>().InActivity(0);

            foreach (Transform chair in chairPoint)
            {
                chair.GetComponent<InteractionPointManager>().Occupy(false);
            }
        }
    }
}

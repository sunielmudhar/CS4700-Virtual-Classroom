using UnityEngine;

public class TableScript : MonoBehaviour
{   
    public Transform[] chairPoint;
    public Vector3 sittingPlace;

    public int Position(GameObject participant, int sentChairPoint)
    {
        int index = -1;

        if (sentChairPoint == -1)
        {
            foreach (Transform chair in chairPoint)
            {
                index++;

                if (!chair.GetComponent<InteractionPointManager>().IsOccupied())
                {
                    SitDown(participant, chair);
                    break;  //Break once a chair is found
                }
            }
        }
        else
        {
            Transform chair = chairPoint[sentChairPoint];
            SitDown(participant, chair);
        }

        return index;
    }

    public void SitDown(GameObject participant, Transform chair)
    {

        participant.GetComponent<ParticipantController>().InActivity(1);

        //Move the participant to the seat
        participant.transform.position = Vector3.Lerp(participant.transform.position, chair.position + sittingPlace, 1);
        participant.transform.rotation = Quaternion.Slerp(participant.transform.rotation, chair.rotation, 1);

        chair.GetComponent<InteractionPointManager>().Occupy(true);

    }

    public void StandUp(GameObject participant)
    {
        participant.GetComponent<ParticipantController>().InActivity(0);

        foreach (Transform chair in chairPoint)
        {
            chair.GetComponent<InteractionPointManager>().Occupy(false);
        }
    }
}

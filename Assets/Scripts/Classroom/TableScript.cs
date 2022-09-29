using UnityEngine;

public class TableScript : MonoBehaviour
{   
    public Transform[] chairPoint;
    public Vector3 sittingPlace;

    /// <summary>
    /// Seat the participant (Can be student or teacher)
    /// If a specific chair point has been sent, then position the participant at that chair point
    /// If no spcific chair point has been supplied, then find the next available chair for the participant to sit in
    /// else statement is mainly used to update other student positions (RPC from line 320 of Teacher Panel script)
    /// </summary>
    
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
        foreach (Transform chair in chairPoint)
        {
            FreeChair(chair);
        }
        
        participant.GetComponent<ParticipantController>().InActivity(0);
    }

    //Need to call this method from the foreach loop as cannot modify variables within an iteration
    public void FreeChair(Transform chair)
    {
        chair.GetComponent<InteractionPointManager>().Occupy(false);
    }
}

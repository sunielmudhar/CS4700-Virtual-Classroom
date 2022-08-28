using UnityEngine;
using Photon.Pun;
using System.IO;

public class ActivityManager : MonoBehaviour
{
    [SerializeField] private GameObject whiteboard, markingManager;
    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void StartActivity(string activityName, int numberOfGroups)
    {
        if (activityName.Equals("whiteboard"))
        {
            PV.RPC("ActivityInitialiser", RpcTarget.All, activityName, true, 0);
        }
        else if (activityName.Equals("whiteboardMarking"))
        {
            PV.RPC("ActivityInitialiser", RpcTarget.Others, activityName, null, numberOfGroups);
        }
    }

    public void EndActivity(string activityName)
    {
        if (activityName.Equals("whiteboard"))
        {
            ActivityInitialiser(activityName, false, 0);
        }
    }

    [PunRPC]
    public void ActivityInitialiser(string activityName, bool state, int numberOfGroups)
    {
        if (activityName.Equals("whiteboard"))
        {
            if (state)
            {
                whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(true);
            }
            else if (!state)
            {
                whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(false);
            }
        }
        else if (activityName.Equals("whiteboardMarking"))
        {
            markingManager.GetComponent<MarkingManager>().StartMarkingActivity("whiteboard", numberOfGroups);
        }
    }
}

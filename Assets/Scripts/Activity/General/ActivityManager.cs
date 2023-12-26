using UnityEngine;
using Photon.Pun;
using TMPro;

public class ActivityManager : MonoBehaviour
{
    /// <summary>
    /// This script file is used to start and end each activity
    /// Takes instructions from the teacher panel and reflects any changes across all clients connected to the classroom
    /// </summary>

    [SerializeField] private GameObject whiteboard, markingManager, activityManager;
    [SerializeField] private Canvas activityManagerCanvas;
    [SerializeField] private TextMeshProUGUI txt_ActivityText;
    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void StartActivity(string activityName, string activityText, int numberOfGroups, float timerValue)
    {
        PV.RPC("ActivityInitialiser", RpcTarget.All, activityName, true, 0);
        PV.RPC("AssetInitialiser", RpcTarget.All, activityText, timerValue);
    }

    public void EndActivity(string activityName)
    {
        ActivityInitialiser(activityName, false, 0);
        activityManager.GetComponent<Timer>().InitialiseTimer(0f, false);
    }

    //Initialise each activity, by making the relevant canvases visible or initialising the relevant states of the starting functions
    [PunRPC]
    public void ActivityInitialiser(string activityName, bool state, int numberOfGroups)
    {
        switch (activityName)
        {
            case "whiteboard":
                if (state)
                {
                    whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(true);
                }
                else
                {
                    whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(false);
                }
                break;
            case "whiteboardMarking":
                if (state)
                {
                    markingManager.GetComponent<MarkingManager>().StartMarkingActivity("whiteboard", numberOfGroups);
                }
                else
                {
                    markingManager.GetComponent<MarkingManager>().EndMarkingActivity("endWhiteboard");
                }
                break;
            case "peerAssessment":
                if (state)
                {
                    markingManager.GetComponent<MarkingManager>().StartMarkingActivity("peerAssessment", numberOfGroups);
                }
                else
                {
                    markingManager.GetComponent<MarkingManager>().EndMarkingActivity("endPeerAssessment");
                }
                break;
            default:
                Debug.Log("Defaulted to no activity case");
                break;
        }
    }

    //Set the activity text and time
    [PunRPC]
    public void AssetInitialiser(string activityText, float timerValue)
    {
        txt_ActivityText.text = activityText;
        activityManager.GetComponent<Timer>().InitialiseTimer(timerValue, true);
    }
}

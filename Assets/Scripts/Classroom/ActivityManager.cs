using UnityEngine;
using Photon.Pun;
using System.IO;

public class ActivityManager : MonoBehaviour
{
    private GameObject whiteboardActivity;
    private GameObject participant;

    void Start()
    {
        participant = GameObject.Find("Participant(Clone)");
    }

    public void StartActivity(string activityName, int numberOfGroups)
    {
        if (activityName.Equals("whiteboard"))
        {
            for(int i = 1; i <= numberOfGroups; i++)
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefabs/Activities", "Whiteboard"), new Vector3(0f, 0f, 0f), Quaternion.identity);
                SetVisibility(activityName);
            }
        }
    }

    public void SetVisibility(string activityName)
    {

        whiteboardActivity = GameObject.Find("Whiteboard(Clone)");

        if (activityName.Equals("whiteboard"))
        {
            whiteboardActivity.GetComponentInChildren<WhiteboardManager>().SetActive(true, participant);
        }

    }

    public void EndActivity(string activityName)
    {

        if (activityName.Equals("whiteboard"))
        {
            whiteboardActivity.GetComponentInChildren<WhiteboardManager>().SetActive(false, participant);
        }
    }
    
}

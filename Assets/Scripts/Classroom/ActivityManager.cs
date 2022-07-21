using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ActivityManager : MonoBehaviour
{
    [SerializeField] GameObject whiteboardActivity;

    private GameObject whiteboardActivityCanvas;
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
        whiteboardActivityCanvas = GameObject.Find("Whiteboard(Clone)");

        if (PhotonNetwork.LocalPlayer.IsLocal && activityName.Equals("whiteboard") && participant.GetComponent<Participant>().CheckData("type").Equals("Student"))
        {
            whiteboardActivityCanvas.gameObject.SetActive(true);
            participant.GetComponent<ParticipantController>().InActivity(1);
        }
        else
        {
            whiteboardActivityCanvas.gameObject.SetActive(false);
        }
    }

    public void EndActivity(string activityName)
    {
        whiteboardActivityCanvas = GameObject.Find("Whiteboard(Clone)");

        if (PhotonNetwork.LocalPlayer.IsLocal && activityName.Equals("whiteboard") && participant.GetComponent<Participant>().CheckData("type").Equals("Student"))
        {
            whiteboardActivity.gameObject.SetActive(false);
            participant.GetComponent<ParticipantController>().InActivity(0);
        }
    }
    
}

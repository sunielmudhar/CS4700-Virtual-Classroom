using UnityEngine;
using Photon.Pun;
using System.IO;

public class ActivityManager : MonoBehaviour
{
    private GameObject whiteboard, participant;
    void Start()
    {
        participant = GameObject.Find("CurrentParticipant");
    }

    public void StartActivity(string activityName, int numberOfGroups)
    {
        if (activityName.Equals("whiteboard"))
        {
            for(int i = 1; i <= numberOfGroups; i++)
            {
                whiteboard = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Activities", "Whiteboard"), new Vector3(0f, 0f, 0f), Quaternion.identity);
                SetVisibility(activityName, true);
            }
        }
    }

    public void EndActivity(string activityName)
    {
        if (activityName.Equals("whiteboard"))
        {
            SetVisibility(activityName, false);
        }
    }

    public void SetVisibility(string activityName, bool state)
    {
        if (activityName.Equals("whiteboard"))
        {
            if (state)
            {
                whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(true, participant);
            }
            else if (!state)
            {
                foreach (GameObject whiteboard in TeacherPannel.GetList("whiteboardList"))
                {
                    whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(false, participant);
                }
            }
        }
    }
}

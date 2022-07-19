using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeacherPannel : MonoBehaviour
{

    private GameObject participant;
    [SerializeField] public Canvas teacherPanelCanvas;
    [SerializeField] public GameObject activityManager;

    bool bl_ActivityManagerOpen = false;

    void Start()
    {
        participant = GameObject.Find("Participant(Clone)");
        teacherPanelCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsLocal && Input.GetKey(KeyCode.Tab) && participant.GetComponent<Participant>().CheckData("type").Equals("Teacher") && !bl_ActivityManagerOpen)
        {
            ManageActivityPanel();
        }

    }

    public void ManageActivityPanel()
    {
        teacherPanelCanvas.gameObject.SetActive(true);
        participant.GetComponent<ParticipantController>().InActivity(1);
    }

    public void CloseActivityPanel()
    {
        teacherPanelCanvas.gameObject.SetActive(false);
        participant.GetComponent<ParticipantController>().InActivity(0);
    }

    public void StartWhiteBoard()
    {
        activityManager.GetComponent<ActivityManager>().StartActivity("whiteboard");
    }
}

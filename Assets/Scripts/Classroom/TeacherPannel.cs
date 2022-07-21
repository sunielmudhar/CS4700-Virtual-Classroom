using UnityEngine;
using Photon.Pun;
using System;
using TMPro;

public class TeacherPannel : MonoBehaviour
{

    private GameObject participant;
    private PhotonView PV;
    private string str_CurrentTask;
    int int_NumberOfGroups;
    [SerializeField] TMP_InputField intxt_NumberOfGroups;
    [SerializeField] public Canvas teacherPanelCanvas;
    [SerializeField] public GameObject activityManager, btn_StartWBA, btn_EndWBA;

    bool bl_ActivityManagerOpen = false, bl_GroupSet;

    void Start()
    {
        PV = GetComponent<PhotonView>();
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

    [PunRPC]
    public void StartWhiteBoard()
    {
        int_NumberOfGroups = int.Parse(intxt_NumberOfGroups.text);

        btn_StartWBA.gameObject.SetActive(false);
        btn_EndWBA.gameObject.SetActive(true);
        activityManager.GetComponent<ActivityManager>().StartActivity("whiteboard", int_NumberOfGroups);
        str_CurrentTask = "whiteboard";
    }

    [PunRPC]
    public void EndWhiteBoard()
    {
        btn_StartWBA.gameObject.SetActive(true);
        btn_EndWBA.gameObject.SetActive(false);
        activityManager.GetComponent<ActivityManager>().EndActivity("whiteboard");
        str_CurrentTask = "endwhiteboard";
    }
}

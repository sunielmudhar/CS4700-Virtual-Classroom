using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using System.Collections.Generic;

public class TeacherPannel : MonoBehaviour
{
    private GameObject participant;
    private PhotonView PV;
    private string str_CurrentTask;

    List<GameObject> roomGameObjectsList = new List<GameObject>();
    List<GameObject> studentsList = new List<GameObject>();
    List<GameObject> whiteboardsList = new List<GameObject>();
    Scene classroom;

    [SerializeField] public int int_NumberOfGroups, int_CurrentGroupID = 0;
    [SerializeField] public TMP_InputField intxt_NumberOfGroups;
    [SerializeField] public Canvas teacherPanelCanvas;
    [SerializeField] public GameObject activityManager, btn_StartWBA, btn_EndWBA;

    bool bl_ActivityManagerOpen = false, bl_GroupSet;

    void Start()
    {
        classroom = SceneManager.GetActiveScene();
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

    public void PopulateLists()
    {
        classroom.GetRootGameObjects(roomGameObjectsList);

        foreach (GameObject gameObject in roomGameObjectsList)
        {
            if (gameObject.tag.Equals("Student"))
            {
                if (!studentsList.Contains(gameObject))
                {
                    studentsList.Add(gameObject);
                    Debug.Log("Student added");
                }
            }
            else if (gameObject.tag.Equals("Whiteboard"))
            {
                if (!whiteboardsList.Contains(gameObject))
                {
                    whiteboardsList.Add(gameObject);
                }
            }
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
        PopulateLists();
        CreateGroups();
        str_CurrentTask = "whiteboard";
    }

    [PunRPC]
    public void EndWhiteBoard()
    {
        btn_StartWBA.gameObject.SetActive(true);
        btn_EndWBA.gameObject.SetActive(false);
        activityManager.GetComponent<ActivityManager>().EndActivity("whiteboard");
        str_CurrentTask = "EndWhiteBoard";
    }

    public void CreateGroups()
    {
 
        foreach (GameObject student in studentsList)
        {
            student.GetComponent<Participant>().UpdateData("SetGroupID", GetGroupNumber());
        }

        foreach (GameObject whiteboard in whiteboardsList)
        {
            whiteboard.GetComponent<WhiteboardManager>().UpdateData("SetGroupID", GetGroupNumber());
        }

    }

    public int GetGroupNumber()
    {
        int_CurrentGroupID++;

        if (int_CurrentGroupID > int_NumberOfGroups)
        {
            int_CurrentGroupID = 1;
        }

        return int_CurrentGroupID;
    }

    public void UpdateAllClients()
    {
        if (str_CurrentTask.Equals("EndWhiteBoard"))
        {
            if (PV.IsMine)
            {
                PV.RPC("EndWhiteBoard", RpcTarget.All);
            }
        }
    }

}

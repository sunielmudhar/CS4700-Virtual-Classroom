using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System;

public class TeacherPannel : MonoBehaviour
{
    private GameObject participant;
    private PhotonView PV;
    private string str_CurrentTask;
    Scene classroom;

    private static List<GameObject> roomGameObjectsList = new List<GameObject>();
    private static List<GameObject> studentsList = new List<GameObject>();
    private static List<GameObject> whiteboardsList = new List<GameObject>();   //Potentially remove

    [SerializeField] public int int_NumberOfGroups, int_CurrentGroupID = 0;
    [SerializeField] public TMP_InputField intxt_NumberOfGroups, intxt_Activity;
    [SerializeField] public Canvas teacherPanelCanvas;
    [SerializeField] public GameObject activityManager, btn_StartWBA, btn_EndWBA;
    [SerializeField] public GameObject tableGroup_1, tableGroup_2, tableGroup_3, tableGroup_4;

    bool bl_ActivityManagerOpen = false, bl_GroupSet = false;

    void Start()
    {
        classroom = SceneManager.GetActiveScene();
        PV = GetComponent<PhotonView>();
        participant = GameObject.Find("CurrentParticipant");
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

    [PunRPC]
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
                }
            }
            else if (gameObject.tag.Equals("Whiteboard"))
            {
                if (!whiteboardsList.Contains(gameObject))
                {
                    Debug.Log("Whiteboard");
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

    public void StartWhiteBoard()
    {
        int_NumberOfGroups = int.Parse(intxt_NumberOfGroups.text);

        if (int_NumberOfGroups > 4 || int_NumberOfGroups == 0 || int_NumberOfGroups < 0) //Limiting the max number of groups due to the classroom size and tables in the scene
        {
            Debug.LogError("Please enter a number between 1 and 4");
        }
        else if (int_NumberOfGroups == 1 && studentsList.Count > 8)
        {
            Debug.LogError("Only 8 students are allowed in a group");
        }
        else
        {
            str_CurrentTask = "whiteboard";
            btn_StartWBA.gameObject.SetActive(false);
            btn_EndWBA.gameObject.SetActive(true);
            PopulateLists();
            CreateGroups();
            UpdateAllClients();
            activityManager.GetComponent<ActivityManager>().StartActivity("whiteboard", int_NumberOfGroups);
        }
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
                PV.RPC("EndWhiteBoard", RpcTarget.Others);
                PV.RPC("PositionStudents", RpcTarget.All, "stand");
            }
        }
        else if (str_CurrentTask.Equals("whiteboard"))
        {
            if (PV.IsMine)
            {
                PV.RPC("PopulateLists", RpcTarget.Others);
                PV.RPC("PositionStudents", RpcTarget.All, "sit");
            }
        }
    }

    [PunRPC]
    public void PositionStudents(string position)
    {
        if (position.Equals("sit"))
        {
            foreach (GameObject student in studentsList)
            {
                if (student.GetComponent<GroupData>().GetGroupID() == 1)
                {
                    tableGroup_1.GetComponent<TableScript>().Sitting(student);
                }
                else if (student.GetComponent<GroupData>().GetGroupID() == 2)
                {
                    tableGroup_2.GetComponent<TableScript>().Sitting(student);
                }
                else if (student.GetComponent<GroupData>().GetGroupID() == 3)
                {
                    tableGroup_3.GetComponent<TableScript>().Sitting(student);
                }
                else if (student.GetComponent<GroupData>().GetGroupID() == 4)
                {
                    tableGroup_4.GetComponent<TableScript>().Sitting(student);
                }
            }
        }
        else if (position.Equals("stand"))
        {
            foreach (GameObject student in studentsList)
            {
                tableGroup_1.GetComponent<TableScript>().StandUp(student);
            }
        }
    }

    //Might no longer need
    public static List<GameObject> GetList(string listType)
    {
        if (listType.Equals("whiteboardList"))
        {
            return whiteboardsList;
        }
        else
        {
            return null;
        }
    }

    public static void ModifyList(string listType, GameObject gameObject)
    {
        if (listType.Equals("whiteboard"))
        {
            whiteboardsList.Remove(gameObject);
        }
        else if (listType.Equals("whiteboardClear"))
        {
            whiteboardsList.Clear();
        }
    }
}

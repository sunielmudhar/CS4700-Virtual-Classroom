using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System;

public class TeacherPannel : MonoBehaviour
{
    private GameObject participant, table;
    private PhotonView PV;
    private string str_CurrentTask;

    private static List<GameObject> roomGameObjectsList = new List<GameObject>();
    private static List<GameObject> studentsList = new List<GameObject>();
    private static List<GameObject> whiteboardsList = new List<GameObject>();
    Scene classroom;

    [SerializeField] public int int_NumberOfGroups, int_CurrentGroupID = 0;
    [SerializeField] public TMP_InputField intxt_NumberOfGroups, intxt_Activity;
    [SerializeField] public Canvas teacherPanelCanvas;
    [SerializeField] public GameObject activityManager, btn_StartWBA, btn_EndWBA, mainCamera, drawingCamera;
    [SerializeField] public GameObject[] tableGroup_1, tableGroup_2, tableGroup_3, tableGroup_4;

    bool bl_ActivityManagerOpen = false, bl_GroupSet;

    void Start()
    {
        classroom = SceneManager.GetActiveScene();
        PV = GetComponent<PhotonView>();
        participant = GameObject.Find("CurrentParticipant");
        teacherPanelCanvas.gameObject.SetActive(false);
        mainCamera = GameObject.Find("CameraHolder");
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
        else
        {
            btn_StartWBA.gameObject.SetActive(false);
            btn_EndWBA.gameObject.SetActive(true);
            activityManager.GetComponent<ActivityManager>().StartActivity("whiteboard", int_NumberOfGroups);
            PopulateLists();
            CreateGroups();
            str_CurrentTask = "whiteboard";
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
                PV.RPC("PopulateLists", RpcTarget.All);
                PV.RPC("EndWhiteBoard", RpcTarget.All);
                PV.RPC("PositionStudents", RpcTarget.All, "stand");
            }
        }
        else if (str_CurrentTask.Equals("whiteboard"))
        {
            if (PV.IsMine)
            {
                PV.RPC("PositionStudents", RpcTarget.All, "sit");
            }
        }
    }

    [PunRPC]
    public void PositionStudents(string position)
    {
        table = GameObject.Find("Table");

        if (position.Equals("sit"))
        {
            table.GetComponent<TableScript>().Sitting();
            mainCamera.SetActive(false);
            drawingCamera.SetActive(true);
            //drawingCamera.tag = "MainCamera";
        }
        else if (position.Equals("stand"))
        {
            table.GetComponent<TableScript>().StandUp();
            mainCamera.SetActive(true);
            drawingCamera.SetActive(false);
            drawingCamera.tag = "Drawing Camera";
        }
    }

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
}

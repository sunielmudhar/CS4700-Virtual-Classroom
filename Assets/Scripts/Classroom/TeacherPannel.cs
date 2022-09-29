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
    private string str_CurrentTask = "";
    Scene classroom;

    //REMOVE ALL WHITEBOARD LISTS

    private static List<GameObject> roomGameObjectsList = new List<GameObject>();
    private static List<GameObject> studentsList = new List<GameObject>();
    private static List<string[]> markList = new List<string[]>();

    [SerializeField] public int int_NumberOfGroups, int_CurrentGroupID = 0;
    [SerializeField] public TMP_InputField intxt_NumberOfGroups, intxt_Activity, intxt_TimerValue;
    [SerializeField] public Canvas teacherPanelCanvas, joinTableCanvas;
    [SerializeField] public GameObject activityManager, notificationsManager, btn_StartWBA, btn_EndWBA, btn_StartWM, btn_EndWM, btn_StartPM, btn_EndPM;
    [SerializeField] public GameObject[] tableGroup;

    bool bl_ActivityManagerOpen = false;

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
        if (PhotonNetwork.LocalPlayer.IsLocal && Input.GetKeyDown(KeyCode.Tab) && participant.GetComponent<Participant>().CheckData("type").Equals("Teacher") && !bl_ActivityManagerOpen)
        {
            ManagePanels("teacherPanel_open");
            bl_ActivityManagerOpen = true;
        }
        else if (PhotonNetwork.LocalPlayer.IsLocal && Input.GetKeyDown(KeyCode.Tab) && participant.GetComponent<Participant>().CheckData("type").Equals("Teacher") && bl_ActivityManagerOpen)
        {
            ManagePanels("teacherPanel_close");
            bl_ActivityManagerOpen = false;
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
        }
    }

    public void ManagePanels(string instruction)
    {
        string panelName = instruction.Split('_')[0];
        string action = instruction.Split('_')[1];
        bool state = false;

        if (action.Equals("open"))
        {
            state = true;
            CheckMovementState(action);
        }
        else if (action.Equals("close"))
        {
            state = false;
            CheckMovementState(action);
        }

        if (panelName.Equals("teacherPanel"))
        {
            teacherPanelCanvas.gameObject.SetActive(state);
        }
        else if (panelName.Equals("notificationsPanel"))
        {
            notificationsManager.SetActive(state);
        }
        else if (panelName.Equals("joinTablePanel"))
        {
            joinTableCanvas.gameObject.SetActive(state);
        }
        else if (panelName.Equals("all"))
        {
            teacherPanelCanvas.gameObject.SetActive(state);
            notificationsManager.SetActive(state);
            joinTableCanvas.gameObject.SetActive(state);
            bl_ActivityManagerOpen = state;
        }
    }

    public void CheckMovementState(string action)
    {
        if (!participant.GetComponent<ParticipantController>().GetActivityState())
        {
            if (action.Equals("open"))
            {
                participant.GetComponent<ParticipantController>().SetMovementState(0);
            }
            else if (action.Equals("close"))
            {
                participant.GetComponent<ParticipantController>().SetMovementState(1);
            }
        }
    }

    public void StartActivity(string btnType)
    {
        if (intxt_TimerValue.text.Equals(""))
        {
            Debug.Log("Please enter a valid time in hours");
        }

        if (btnType.Equals("whiteboard"))
        {
            int_NumberOfGroups = int.Parse(intxt_NumberOfGroups.text);

            if (int_NumberOfGroups > 4 || int_NumberOfGroups == 0 || int_NumberOfGroups < 0) //Limiting the max number of groups due to the classroom size and tables in the scene
            {
                Debug.LogError("Please enter a number between 1 and 4");
            }
            else if (int_NumberOfGroups == 1 && studentsList.Count > 7)
            {
                Debug.LogError("Only 6 students are allowed in a group");
            }
            else
            {
                str_CurrentTask = "whiteboard";
                btn_StartWBA.gameObject.SetActive(false);
                btn_EndWBA.gameObject.SetActive(true);
                PopulateLists();
                CreateGroups();
                PositionStudents("sit");
            }
        }
        else if (btnType.Equals("whiteboardMarking"))
        {
            if (str_CurrentTask.Equals("EndWhiteboard"))    //Check if whiteboard task has ended
            {
                btn_StartWBA.gameObject.SetActive(false);
                btn_StartWM.gameObject.SetActive(false);
                btn_StartPM.gameObject.SetActive(false);
                btn_EndWM.gameObject.SetActive(true);
                str_CurrentTask = "MarkingWhiteBoard";
            }
            else
            {
                Debug.LogError("Unable to start whiteboard marking task without first starting whiteboard task");
            }
        }
        else if (btnType.Equals("peerAssessment"))
        {
            if (str_CurrentTask.Equals("EndWhiteboard") || str_CurrentTask.Equals("EndWhiteboardMarking"))
            {
                btn_StartWBA.gameObject.SetActive(false);
                btn_StartWM.gameObject.SetActive(false);
                btn_StartPM.gameObject.SetActive(false);
                btn_EndPM.gameObject.SetActive(true);
                str_CurrentTask = "PeerMarking";
            }
            else
            {
                Debug.LogError("Unable to start peer marking task without first starting whiteboard task");
            }
        }

        activityManager.GetComponent<ActivityManager>().StartActivity(btnType, intxt_Activity.text, int_NumberOfGroups, float.Parse(intxt_TimerValue.text));
        UpdateAllClients();
    }

    public void TimerEnded()
    {
        string actType;

        Debug.Log("TimerEnded function has been triggered");

        if (str_CurrentTask.Equals("whiteboard"))
        {
            actType = "whiteboard";
        }
        else if (str_CurrentTask.Equals("MarkingWhiteBoard"))
        {
            actType = "whiteboardMarking";
        }
        else if (str_CurrentTask.Equals("PeerMarking"))
        {
            actType = "peerAssessment";
        }
        else
        {
            actType = null;
        }

        EndActivity(actType);
    }

    [PunRPC]
    public void EndActivity(string btnType)
    {
        if (btnType.Equals("whiteboard"))
        {
            btn_StartWBA.gameObject.SetActive(true);
            btn_StartWM.gameObject.SetActive(true);
            btn_StartPM.gameObject.SetActive(true);
            btn_EndWBA.gameObject.SetActive(false);
            str_CurrentTask = "EndWhiteboard";
        }
        else if (btnType.Equals("whiteboardMarking"))
        {
            btn_StartWBA.gameObject.SetActive(true);
            btn_StartWM.gameObject.SetActive(true);
            btn_StartPM.gameObject.SetActive(true);
            btn_EndWM.gameObject.SetActive(false);
            str_CurrentTask = "EndWhiteboardMarking";
        }
        else if (btnType.Equals("peerAssessment"))
        {
            btn_StartWBA.gameObject.SetActive(true);
            btn_StartWM.gameObject.SetActive(true);
            btn_StartPM.gameObject.SetActive(true);
            btn_EndPM.gameObject.SetActive(false);
            str_CurrentTask = "EndPeerMarking";
        }

        activityManager.GetComponent<ActivityManager>().EndActivity(btnType);
        UpdateAllClients();
    }

    public void ExportToCSV()
    {
        this.GetComponent<CSVMaker>().CreateMarksCSV(markList, str_CurrentTask);
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
        if (str_CurrentTask.Equals("EndWhiteboard"))
        {
            if (PV.IsMine)
            {
                PV.RPC("EndActivity", RpcTarget.Others, "whiteboard");
                PV.RPC("PositionStudents", RpcTarget.Others, "stand");
            }
        }
        else if (str_CurrentTask.Equals("whiteboard"))
        {
            if (PV.IsMine)
            {
                PV.RPC("PopulateLists", RpcTarget.Others);
                //PV.RPC("PositionStudents", RpcTarget.Others, "sit");
            }
        }
        else if (str_CurrentTask.Equals("EndWhiteboardMarking"))
        {
            if (PV.IsMine)
            {
                PV.RPC("EndActivity", RpcTarget.Others, "whiteboardMarking");
            }
        }
        else if (str_CurrentTask.Equals("EndPeerMarking"))
        {
            if (PV.IsMine)
            {
                PV.RPC("EndActivity", RpcTarget.Others, "peerAssessment");
            }
        }
    }

    [PunRPC]
    public void PositionStudents(string position)
    {
        int index = -1;

        foreach (GameObject student in studentsList)
        {
            Debug.Log(student.GetComponent<Participant>().CheckData("name") + " sit");

            if (position.Equals("sit") && student.tag.Equals("Student"))
            {
                if (student.GetComponent<GroupData>().GetGroupID() == 1)
                {
                    index = tableGroup[0].GetComponent<TableScript>().Position(student, -1);
                }
                else if (student.GetComponent<GroupData>().GetGroupID() == 2)
                {
                    index = tableGroup[1].GetComponent<TableScript>().Position(student, -1);
                }
                else if (student.GetComponent<GroupData>().GetGroupID() == 3)
                {
                    index = tableGroup[2].GetComponent<TableScript>().Position(student, -1);
                }
                else if (student.GetComponent<GroupData>().GetGroupID() == 4)
                {
                    index = tableGroup[3].GetComponent<TableScript>().Position(student, -1);
                }
            }
            else if (position.Equals("stand"))
            {
                tableGroup[0].GetComponent<TableScript>().StandUp(student); //Just need to trigger 1 table group not all of them
            }

            PV.RPC("PositionParticipant", RpcTarget.Others, position, student.GetComponent<Participant>().CheckData("refCode"), index);
        }
    }

    [PunRPC]
    public void PositionParticipant(string position, string participantRefCode, int chair)
    {
        if (participantRefCode.Equals(participant.GetComponent<Participant>().CheckData("refCode")))
        {
            if (position.Equals("sit"))
            {
                if (participant.GetComponent<GroupData>().GetGroupID() == 1)
                {
                    tableGroup[0].GetComponent<TableScript>().Position(participant, chair);
                }
                else if (participant.GetComponent<GroupData>().GetGroupID() == 2)
                {
                    tableGroup[1].GetComponent<TableScript>().Position(participant, chair);
                }
                else if (participant.GetComponent<GroupData>().GetGroupID() == 3)
                {
                    tableGroup[2].GetComponent<TableScript>().Position(participant, chair);
                }
                else if (participant.GetComponent<GroupData>().GetGroupID() == 4)
                {
                    tableGroup[3].GetComponent<TableScript>().Position(participant, chair);
                }
            }
            else if (position.Equals("stand"))
            {
                tableGroup[0].GetComponent<TableScript>().StandUp(participant);
            }
        }
    }

    public static void ModifyList(string listType, string[] submittedStudentMark, string participantRef)
    {
        if (listType.Equals("AddMarksPeer") || listType.Equals("AddMarksGroup"))
        {
            markList.Add(submittedStudentMark);
            Debug.Log("Marks Submitted");
        }
        else if (listType.Equals("RemoveStudent"))
        {
            GameObject studentToRemove = null;

            foreach (GameObject student in studentsList)
            {
                if (student.GetComponent<Participant>().CheckData("refCode").Equals(participantRef))
                {
                    studentToRemove = student;
                    break;
                }
            }

            studentsList.Remove(studentToRemove);
        }
    }

    public static List<GameObject> GetList(string listType)
    {
        if (listType.Equals("studentList"))
        {
            return studentsList;
        }
        else
        {
            return null;
        }
    }
}

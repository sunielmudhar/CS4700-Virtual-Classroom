using UnityEngine;

public class JoinTableScript : MonoBehaviour
{
    [SerializeField] GameObject participant, teacherPanel, whiteboardManager;

    void Start()
    {
        participant = GameObject.Find("CurrentParticipant");
        teacherPanel = GameObject.Find("Teacher Panel");
    }

    public void ChangeParticipantType(string action)
    {
        /*if(action.Equals("join"))
            participant.GetComponent<Participant>().ChangeType("student");
        else if (action.Equals("leave"))
            participant.GetComponent<Participant>().ChangeType("teacher");*/
    }

    public void OnClickJoinTable(string tableNumber)
    {
        ChangeParticipantType("join");

        if (tableNumber.Equals("1"))
        {
            participant.GetComponent<GroupData>().SetGroupID(1);
        }
        else if (tableNumber.Equals("2"))
        {
            participant.GetComponent<GroupData>().SetGroupID(2);
        }
        else if (tableNumber.Equals("3"))
        {
            participant.GetComponent<GroupData>().SetGroupID(3);
        }
        else if (tableNumber.Equals("4"))
        {
            participant.GetComponent<GroupData>().SetGroupID(4);
        }

        teacherPanel.GetComponent<TeacherPannel>().ManagePanels("all_close");   //Close all panels, this also fixes the issue with resetting the bl_CanMove status back to 0
        teacherPanel.GetComponent<TeacherPannel>().PositionParticipant("sit", participant.GetComponent<Participant>().CheckData("name"), -1);

        if (whiteboardManager = null)
        {
            Debug.LogError("Whiteboard not found, please start a whiteboard task to activate");
        }
        else
        {
            whiteboardManager.GetComponent<WhiteboardManager>().SetCamera(true);
        }
    }

    public void OnClickLeaveTable()
    {
        ChangeParticipantType("leave");
        teacherPanel.GetComponent<TeacherPannel>().PositionParticipant("stand", participant.GetComponent<Participant>().CheckData("name"), -1);
        whiteboardManager.GetComponent<WhiteboardManager>().SetCamera(false);
    }
}

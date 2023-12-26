using System.Threading;
using UnityEngine;

public class JoinTableScript : MonoBehaviour
{
    /// <summary>
    /// Following script allows a teacher to join a table, by assigning a group number
    /// and then seating the teacher to the specific table with that group number
    /// </summary>

    [SerializeField] GameObject participant, teacherPanel, whiteboardManager;

    void Start()
    {
        participant = GameObject.Find("CurrentParticipant");
        teacherPanel = GameObject.Find("Teacher Panel");
    }

    //Function is attached to a table button, and takes a string input which will be used to assign the teachers group number
    public void OnClickJoinTable(string tableNumber)
    {
        participant.GetComponent<GroupData>().SetGroupID(int.Parse(tableNumber));
        teacherPanel.GetComponent<TeacherPannel>().ManagePanels("all_close");   //Close all panels, this also fixes the issue with resetting the bl_CanMove status back to 0
        teacherPanel.GetComponent<TeacherPannel>().PositionParticipant("sit", participant.GetComponent<Participant>().CheckData("refCode"), -1);

        //Check if the whiteboard manager has been found
        if (whiteboardManager == null)
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
        teacherPanel.GetComponent<TeacherPannel>().PositionParticipant("stand", participant.GetComponent<Participant>().CheckData("refCode"), -1);
        whiteboardManager.GetComponent<WhiteboardManager>().SetCamera(false);
    }
}

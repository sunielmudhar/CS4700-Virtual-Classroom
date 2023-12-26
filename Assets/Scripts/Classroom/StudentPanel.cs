using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentPanel : MonoBehaviour
{
    /// <summary>
    /// Student panel functionality, assigns the manage panel function to buttons
    /// and depending on the type of button pressed, it manipulates the relevant panel/canvases
    /// </summary>

    [SerializeField] GameObject participant;
    [SerializeField] Canvas markingCanvas, activityCanvas, studentPanelCanvas;

    private bool bl_PanelOpen = false;

    void Start()
    {
        participant = GameObject.Find("CurrentParticipant");
    }

    void Update()
    {
        if (participant.tag.Equals("Student") && Input.GetKeyDown(KeyCode.Tab) && !bl_PanelOpen)
        {
            ManagePanels("studentPanel_open");
            bl_PanelOpen = true;
        }
        else if (participant.tag.Equals("Student") && Input.GetKeyDown(KeyCode.Tab) && bl_PanelOpen)
        {
            ManagePanels("studentPanel_close");
            bl_PanelOpen = false;
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

        switch (panelName)
        {
            case "studentPanel":
                studentPanelCanvas.gameObject.SetActive(state);
                break;
            case "activityPanel":
                activityCanvas.gameObject.SetActive(state);
                break;
            case "markingPanel":
                markingCanvas.gameObject.SetActive(state);
                break;
            default:    //Maybe need to have a final case for all...
                studentPanelCanvas.gameObject.SetActive(state);
                activityCanvas.gameObject.SetActive(state);
                markingCanvas.gameObject.SetActive(state);
                bl_PanelOpen = false;
                break;
        }
    }

    //Check if the participant is in an activity, if not then stop them from moving while in a panel and allow them to move when outside
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
}

using UnityEngine;
using Photon.Pun;

public class HelpScript : MonoBehaviour
{
    /// <summary>
    /// Following script is called when a student is looking to request help
    /// Does this by sending a PUN RPC to all participants (incase of multiple teachers)
    /// SendRequest() then recieves the sent data and displays it to the teacher(s)
    /// </summary>

    [SerializeField] private GameObject participant, teacherPanel;
    PhotonView PV;

    void Start()
    {
        participant = GameObject.Find("CurrentParticipant");
        PV = GetComponent<PhotonView>();
    }

    public void OnPressHelp()
    {
        if (participant.tag.Equals("Student"))
        {
            PV.RPC("SendRequest", RpcTarget.All, participant.GetComponent<Participant>().CheckData("name"), participant.GetComponent<GroupData>().GetGroupID());    //Targeting all incase more than 1 teacher
        }
    }

    [PunRPC]
    public void SendRequest(string participantName, int groupNumber)
    {
        if (participant.tag.Equals("Teacher"))
        {
            teacherPanel.GetComponent<NotificationsScript>().RecieveNotification(participantName, groupNumber);
        }
    }    
}

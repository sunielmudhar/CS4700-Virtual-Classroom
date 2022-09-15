using UnityEngine;
using Photon.Pun;

public class HelpScript : MonoBehaviour
{
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

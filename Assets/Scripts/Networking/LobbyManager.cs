using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI txt_RoomCode, txt_HostName;
    [SerializeField] Participant manageParticipants;

    /*If teacher then can see the disconnect button for each other participant, else it is invisible*/

    void Start()
    {
        txt_RoomCode.text = PhotonNetwork.CurrentRoom.Name;
        GetParticipantList();
        manageParticipants.GetParameters();
        txt_HostName.text = "Host: " + PhotonNetwork.MasterClient.NickName.Split('_')[1];
    }

    public void OnClickDisconnect()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu");
    }

    public void GetParticipantList()
    {

    }

    public void ManageParticipants()    //This will allow the host to kick players that are currently in the lobby
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.CloseConnection();
        }
        else
        {
            return;
        }
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [SerializeField] public TextMeshProUGUI txt_RoomCode, txt_HostName;
    [SerializeField] public GameObject participant;
    [SerializeField] public Canvas hostCanvas;

    /*If teacher then can see the disconnect button for each other participant, else it is invisible*/

    void Start()
    {
        txt_RoomCode.text = PhotonNetwork.CurrentRoom.Name;
        txt_HostName.text = "Host: " + PhotonNetwork.MasterClient.NickName.Split('_')[1];

        if (PhotonNetwork.NickName.Split('_')[0].Equals("Teacher"))
        {
            hostCanvas.gameObject.SetActive(true);
        }

        Debug.Log(PhotonNetwork.NickName);
    }

    public void OnClickDisconnect()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu");
    }

    public void OnClickStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Classroom");
        }
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

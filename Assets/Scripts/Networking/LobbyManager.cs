using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using System.IO;

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [SerializeField] public TextMeshProUGUI txt_RoomCode, txt_HostName;
    [SerializeField] public GameObject participant, participantUIPrefab;
    [SerializeField] public Canvas hostCanvas;
    [SerializeField] public Transform participantListCanvas;

    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();

        txt_RoomCode.text = PhotonNetwork.CurrentRoom.Name;
        txt_HostName.text = "Host: " + PhotonNetwork.MasterClient.NickName.Split('_')[1];

        //Check if the current user is a teacher, if yes then display the host canvas
        if (PhotonNetwork.NickName.Split('_')[0].Equals("Teacher"))
        {
            hostCanvas.gameObject.SetActive(true);
        }

        string participantName = PhotonNetwork.NickName.Split('_')[1];

        PV.RPC("SetParticipantList", RpcTarget.MasterClient, participantName);  //Joining participant sends their name to the host to add to participant list
    }

    public static void OnClickDisconnect()
    {
        Debug.Log("Disconnecting");
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu");
    }

    public void OnClickStart()
    {
        Debug.Log("Starting classroom");

        //Only the host can initiate the scene change
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Classroom");
        }
    }

    [PunRPC]
    public void SetParticipantList(string participantName)
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefabs/UI", "participantUIAsset"), new Vector3(0,0,0), Quaternion.identity);
        PV.RPC("ParticipantListHelper", RpcTarget.AllBuffered, participantName);    //Update other participants to show the list changes
    }

    [PunRPC]
    public void ParticipantListHelper(string participantName)
    {
        GameObject participantUIAsset = GameObject.Find("ParticipantUIAsset(Clone)");
        participantUIAsset.transform.SetParent(participantListCanvas);
        participantUIAsset.GetComponentInChildren<TextMeshProUGUI>().text = participantName;
        participantUIAsset.name = participantName + " UI Asset";
    }
}

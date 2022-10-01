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

    /*If teacher then can see the disconnect button for each other participant, else it is invisible*/

    void Start()
    {
        PV = GetComponent<PhotonView>();

        txt_RoomCode.text = PhotonNetwork.CurrentRoom.Name;
        txt_HostName.text = "Host: " + PhotonNetwork.MasterClient.NickName.Split('_')[1];

        if (PhotonNetwork.NickName.Split('_')[0].Equals("Teacher"))
        {
            hostCanvas.gameObject.SetActive(true);
        }

        Debug.Log(PhotonNetwork.NickName);

        string participantName = PhotonNetwork.NickName.Split('_')[1];

        PV.RPC("SetParticipantList", RpcTarget.MasterClient, participantName);
    }

    public static void OnClickDisconnect()
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

    [PunRPC]
    public void SetParticipantList(string participantName)
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefabs/UI", "participantUIAsset"), new Vector3(0,0,0), Quaternion.identity);
        PV.RPC("ParticipantListHelper", RpcTarget.AllBuffered, participantName);
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

using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour
{
    PhotonView PV;
    GameObject participant;
    [SerializeField] Canvas menuCanvas;
    [SerializeField] bool bl_IsMenuOpen = false;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        participant = GameObject.Find("CurrentParticipant");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !bl_IsMenuOpen)
        {
            menuCanvas.gameObject.SetActive(true);
            bl_IsMenuOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && bl_IsMenuOpen)
        {
            menuCanvas.gameObject.SetActive(false);
            bl_IsMenuOpen = false;
        }
    }

    public void OnClickDisconnectButtonMenu()
    {
        if (participant.tag.Equals("Student"))
            PV.RPC("UpdateStudentList", RpcTarget.All, participant.GetComponent<Participant>().CheckData("refCode"));

        DisconnectParticipant();
    }

    public void DisconnectParticipant()
    {
        StartCoroutine(Disconnect());
    }
    IEnumerator Disconnect()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;
        PhotonNetwork.LoadLevel("Menu");
    }

    [PunRPC]
    public void UpdateStudentList(string participantRef)
    {
        TeacherPannel.ModifyList("RemoveStudent", null, participantRef);
        Debug.Log(participantRef + " removed from list");
    }
}

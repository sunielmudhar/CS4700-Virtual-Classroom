using UnityEngine;
using Photon.Pun;
using TMPro;

public class LobbyManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI txt_RoomCode;

    void Start()
    {
        txt_RoomCode.text = PhotonNetwork.CurrentRoom.Name;
    }

}

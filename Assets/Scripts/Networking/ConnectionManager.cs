using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ConnectionManager : MonoBehaviourPunCallbacks
{

    [SerializeField] public TMP_InputField intxt_InputName;
    [SerializeField] public GameObject btn_CreateRoom;
    [SerializeField] public GameObject btn_JoinRoom;
    [SerializeField] public TextMeshProUGUI txt_StatusText;
    [SerializeField] public TMP_InputField intxt_NumberOfParticipants;
    [SerializeField] private TextMeshProUGUI txt_RoomCode;
    [SerializeField] ReadCSV_ForbiddenNames GetForbiddenNames;


    //On start connect to the Photon servers
    private void Start() 
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /*
     *Following functions will redirect users to the right scene, and depending on which button they press, will idenfity them as either a teacher or student
     *Manipulates the nickname as the teacher can then add more teachers to a class, which will manipulate their permissions by adding "Teacher_" to their photon name     
    */

    public void OnClickCreate()
    {
        OnClickCheck();
        PhotonNetwork.NickName = "Teacher_" + PhotonNetwork.NickName;
        PhotonNetwork.CreateRoom(txt_RoomCode.text, new Photon.Realtime.RoomOptions() {MaxPlayers = byte.Parse(intxt_NumberOfParticipants.text)});
    }

    public void OnClickJoin()
    {
        OnClickCheck();
        PhotonNetwork.NickName = "Student_" + PhotonNetwork.NickName;
    }

    public void OnClickCheck() {

        //Check to see if the username is valid
        if (GetForbiddenNames.CheckName(intxt_InputName.text))
        {
            PhotonNetwork.NickName = intxt_InputName.text;
            txt_StatusText.gameObject.SetActive(false);
        }
        else
        {
            txt_StatusText.gameObject.SetActive(true);
            Debug.Log("Invalid name entered");
            txt_StatusText.text = ("Please enter a valid name");
        }

    }

    public void GenerateRoomCode()
    {
        char[] characters = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        int c = Random.Range(0, 35);
        string code = "";
        for (int i = 1; i <= 7; i++) { }

    }

}

using UnityEngine;
using Photon.Pun;
using TMPro;

public class ConnectionManager : MonoBehaviourPunCallbacks
{

    [SerializeField] public TMP_InputField intxt_InputName, intxt_NumberOfParticipants, intxt_RoomCode;
    [SerializeField] public TextMeshProUGUI txt_StatusText;
    [SerializeField] public Canvas canvasMain, canvasHost, canvasJoin;
    [SerializeField] ReadCSV_ForbiddenNames GetForbiddenNames;
    [SerializeField] ModelSelection modelSelection;

    private string str_RoomCode;


    //On start connect to the Photon servers
    private void Start() 
    {
        canvasMain.gameObject.SetActive(true);
        txt_StatusText.gameObject.SetActive(false);
        canvasHost.gameObject.SetActive(false);
        canvasJoin.gameObject.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    /*
     *Following functions will redirect users to the right scene, and depending on which button they press, will idenfity them as either a teacher or student
     *Manipulates the nickname as the teacher can then add more teachers to a class, which will manipulate their permissions by adding "Teacher_" to their photon name
    */

    public void OnClickCreate()
    {
        if (NameCheck())
        {
            PhotonNetwork.NickName = "Teacher_" + PhotonNetwork.NickName + "_" + modelSelection.int_ModelNumber;
            canvasMain.gameObject.SetActive(false);
            canvasHost.gameObject.SetActive(true);
        }
    }

    public void OnClickJoin()
    {
        if (NameCheck())
        {
            PhotonNetwork.NickName = "Student_" + PhotonNetwork.NickName + "_" + modelSelection.int_ModelNumber;
            canvasMain.gameObject.SetActive(false);
            canvasJoin.gameObject.SetActive(true);
        }
    }

    public void OnClickCreateRoom()
    {
        GenerateRoomCode(); //Generate the unique room code for the students to join
        Debug.Log("Room code is: " + str_RoomCode);
        PhotonNetwork.CreateRoom(str_RoomCode, new Photon.Realtime.RoomOptions() { MaxPlayers = byte.Parse(intxt_NumberOfParticipants.text) });
    }

    public void OnClickJoinRoom()
    {
        PhotonNetwork.JoinRoom(intxt_RoomCode.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("WaitingRoom");
    }

    public bool NameCheck() {

        //Check to see if the username is valid
        if (GetForbiddenNames.CheckName(intxt_InputName.text))
        {
            PhotonNetwork.NickName = intxt_InputName.text;
            txt_StatusText.gameObject.SetActive(false);
            return true;
        }
        else
        {
            txt_StatusText.gameObject.SetActive(true);
            Debug.Log("Invalid name entered");
            txt_StatusText.text = ("Please enter a valid name");
            return false;
        }

    }

    //Create a 7 character room code
    public void GenerateRoomCode()
    {
        char[] characters = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        int c;
        string code = "";

        for (int i = 1; i <= 7; i++)
        {
            c = Random.Range(0, 35);
            code = code + characters[c];
        }

        str_RoomCode = code;

    }

}

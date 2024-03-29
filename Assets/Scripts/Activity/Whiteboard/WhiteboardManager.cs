using UnityEngine;
using Photon.Pun;

public class WhiteboardManager : MonoBehaviour
{
    private PhotonView PV;
    [SerializeField] GameObject mainCamera, participant;
    [SerializeField] public GameObject[] drawingCameras;

    //Set it so that drawing is only enabled while the activity is running

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        mainCamera = GameObject.Find("Camera");
        participant = GameObject.Find("CurrentParticipant");
    }

    //Allow students to draw
    public void SetActive(bool state)
    {
        Awake();

        if (state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Student"))
        {
            this.gameObject.SetActive(true);
            SetCamera(state);
            LineGen.CanDraw(true);
        }
        else if (state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Teacher"))
        {
            this.gameObject.SetActive(false);
        }
        else if (!state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Student"))
        {
            this.gameObject.SetActive(false);
            SetCamera(state);
            LineGen.CanDraw(false);
        }

    }

    //Change the current participants camera, to the camera above their groups table
    public void SetCamera(bool state)
    {
        int int_ParticipantGroupID = participant.GetComponent<GroupData>().GetGroupID() - 1;    //Create an int to make the camera selection more dynamic

        if (state)
        {
            mainCamera.tag = "Untagged";
            drawingCameras[int_ParticipantGroupID].SetActive(true);
            drawingCameras[int_ParticipantGroupID].tag = "MainCamera";
        }
        else
        {
            mainCamera.tag = "MainCamera";
            drawingCameras[int_ParticipantGroupID].SetActive(false);
            drawingCameras[int_ParticipantGroupID].tag = "Drawing Camera";
        }
    }
}

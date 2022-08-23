using UnityEngine;
using Photon.Pun;

public class WhiteboardManager : MonoBehaviour
{
    private GroupData groupData;
    private PhotonView PV;
    private bool bl_Turn = false;
    [SerializeField] GameObject mainCamera, drawingCamera;

    //Set it so that drawing is only enabled while the activity is running

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        groupData = GetComponent<GroupData>();
        mainCamera = GameObject.Find("CameraHolder");
        drawingCamera = GameObject.FindGameObjectWithTag("Drawing Camera Group 1");
    }

    public void UpdateData(string parameter, int index)
    {
        Awake();    //Need to trigger start function manually as it is instantiated inactive
        if (parameter.Equals("SetGroupID"))
            PV.RPC("SetGroupID", RpcTarget.All, index);
    }

    [PunRPC]
    public void SetGroupID(int iD)
    {
        Awake();    //Need to trigger again for the student, maybe change to Awake?
        this.groupData.SetGroupID(iD);
    }

    public void SetActive(bool state, GameObject participant)
    {
        Awake();

        if (state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Student") && this.groupData.GetGroupID() == participant.GetComponent<GroupData>().GetGroupID())
        {
            this.gameObject.SetActive(true);
            mainCamera.SetActive(false);
            drawingCamera.SetActive(true);
            drawingCamera.tag = "MainCamera";
            Draw.CanDraw(true);
        }
        else if ((state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Student") && this.groupData.GetGroupID() != participant.GetComponent<GroupData>().GetGroupID()) || participant.tag.Equals("Teacher"))
        {
            this.gameObject.SetActive(false);
        }
        else if (!state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Student"))
        {
            this.gameObject.SetActive(false);
            mainCamera.SetActive(true);
            drawingCamera.SetActive(false);
            drawingCamera.tag = "Drawing Camera Group 1";
            Draw.CanDraw(false);
        }
    }

    public void SetTurn()
    {
        
    }

    public void EndTurn()
    {
        bl_Turn = false;
    }
}

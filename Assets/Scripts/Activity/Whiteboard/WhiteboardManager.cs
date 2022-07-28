using UnityEngine;
using Photon.Pun;

public class WhiteboardManager : MonoBehaviour
{
    private GroupData groupData;
    private PhotonView PV;
    private Draw draw;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        groupData = GetComponent<GroupData>();
        draw = GetComponent<Draw>();
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
        if (state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Student") && groupData.GetGroupID() == participant.GetComponent<GroupData>().GetGroupID())
        {
            this.gameObject.SetActive(true);
            participant.GetComponent<ParticipantController>().InActivity(1);
            EnableDrawing(true);
        }
        else
        {
            this.gameObject.SetActive(false);
            participant.GetComponent<ParticipantController>().InActivity(0);
            EnableDrawing(false);
        }
    }

    public void EnableDrawing(bool state)
    {
        if (state)
        {
            draw.Drawing(true);
        }
        else
        {
            draw.Drawing(false);
        }
    }
}

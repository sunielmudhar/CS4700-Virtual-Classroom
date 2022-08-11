using UnityEngine;
using Photon.Pun;

public class WhiteboardManager : MonoBehaviour
{
    private GroupData groupData;
    private PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        groupData = GetComponent<GroupData>();
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
        }
        else if ((state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Student") && this.groupData.GetGroupID() != participant.GetComponent<GroupData>().GetGroupID()) || participant.tag.Equals("Teacher"))
        {
            this.gameObject.SetActive(false);
        }
        else if (!state && PhotonNetwork.LocalPlayer.IsLocal && participant.tag.Equals("Student"))
        {
            this.gameObject.SetActive(false);
        }
    }

}

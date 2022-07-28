using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Participant : MonoBehaviour
{
    //[SerializeField] Permissions ParticipantPermissions;
    [SerializeField] TextMeshProUGUI txt_ParticipantName;
    [SerializeField] GameObject mesh1, mesh2;

    private GroupData groupData;
    private PhotonView PV;

    private string str_ParticipantName;
    private string str_ParticipantType;
    private string str_ParticipantMeshCode;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        groupData = GetComponent<GroupData>();

        if (PV.IsMine)
        {
            GetParameters();
            InitialiseParticipantAssets(str_ParticipantMeshCode, str_ParticipantName, str_ParticipantType);

            PV.RPC("InitialiseParticipantAssets", RpcTarget.All, str_ParticipantMeshCode, str_ParticipantName, str_ParticipantType);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }

    }

    [PunRPC]
    public void GetParameters()
    {
        string[] str_PhotonName = PhotonNetwork.NickName.Split('_');

        str_ParticipantType = str_PhotonName[0];
        str_ParticipantName = str_PhotonName[1];
        str_ParticipantMeshCode = str_PhotonName[2];
    }

    [PunRPC]
    public void InitialiseParticipantAssets(string str_MeshCode, string str_Name, string str_Type)
    {
        txt_ParticipantName.text = str_Name;

        if (str_MeshCode.Equals("0"))
        {
            this.mesh1.SetActive(true);
            this.mesh2.SetActive(false);
        }
        else
        {
            this.mesh2.SetActive(true);
            this.mesh1.SetActive(false);
        }

        if (str_Type.Equals("Teacher"))
        {
            this.gameObject.tag = "Teacher";
        }
        else if (str_Type.Equals("Student"))
        {
            this.gameObject.tag = "Student";
        }
    }

    public string CheckData(string parameter)
    {
        if (parameter.Equals("name"))
        {
            return str_ParticipantName;
        }
        else if (parameter.Equals("type"))
        {
            return str_ParticipantType;
        }
        else if (parameter.Equals("meshCode"))
        {
            return str_ParticipantMeshCode;
        }
        else
        {
            return null;
        }               
    }

    public void UpdateData(string parameter, int index)
    {
        if (parameter.Equals("SetGroupID"))
            PV.RPC("SetGroupID", RpcTarget.All, index);
    }

    public void ChangeType()    //This function will enable the host/teacher to grant or revoke permissions from other participants
    {
        if (/*Enter trigger type*/false)
        {
            if (PhotonNetwork.IsMasterClient)   //Check if Master Client/Host
            {
                str_ParticipantType = "Teacher";
            }
        }
        else if (/*Enter trigger type*/false)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                str_ParticipantType = "Student";
            }
        }
    }

    [PunRPC]
    public void SetGroupID(int iD)
    {
        this.groupData.SetGroupID(iD);
    }
}

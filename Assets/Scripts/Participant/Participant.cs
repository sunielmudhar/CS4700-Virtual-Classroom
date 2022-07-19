using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Participant : MonoBehaviour
{
    [SerializeField] Permissions ParticipantPermissions;
    [SerializeField] TextMeshProUGUI txt_ParticipantName;
    [SerializeField] GameObject mesh1, mesh2;
    public PhotonView PV;

    private string str_ParticipantName;
    private string str_ParticipantType;
    private string str_ParticipantMeshCode;

    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            GetParameters();
            InitialiseParticipantAssets(str_ParticipantMeshCode, str_ParticipantName);

            PV.RPC("InitialiseParticipantAssets", RpcTarget.All, str_ParticipantMeshCode, str_ParticipantName);
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

        Debug.Log(str_ParticipantName);
    }

    [PunRPC]
    public void InitialiseParticipantAssets(string str_MeshCode, string str_Name)
    {
        txt_ParticipantName.text = str_Name;
        Debug.Log("Participant Mesh Code: " + str_MeshCode);

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Participant : MonoBehaviour
{
    [SerializeField] Permissions ParticipantPermissions;

    public string str_ParticipantName;
    public string str_ParticipantType;
    //public string str_ParticipantMeshCode;    To be used to identify the participants selected mesh

    public void GetParameters()
    {
        string[] str_PhotonName = PhotonNetwork.NickName.Split('_');

        str_ParticipantType = str_PhotonName[0];
        str_ParticipantName = str_PhotonName[1];

        Debug.Log(str_ParticipantName);
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

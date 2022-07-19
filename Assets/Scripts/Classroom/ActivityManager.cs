using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ActivityManager : MonoBehaviour
{
    [SerializeField] Canvas whiteboardActivityCanvas;



    private GameObject participant;
    private PhotonView PV;
    private string activity;

    void Start()
    {
        participant = GameObject.Find("Participant(Clone)");
        PV = GetComponent<PhotonView>();
        Debug.Log("CheckData works! Data Type = " + participant.GetComponent<Participant>().CheckData("type"));
    }

    [PunRPC]
    public void StartActivity(string activityName)
    {
        if (activityName.Equals("whiteboard") && participant.GetComponent<Participant>().CheckData("type").Equals("Student"))
        {
            whiteboardActivityCanvas.gameObject.SetActive(true);
            activity = activityName;
            UpdateAll();
        }
    }
    
    public void UpdateAll()
    {
        if(PV.IsMine)
            PV.RPC("StartActivity", RpcTarget.All, activity);
    }
}

using UnityEngine;
using Photon.Pun;
using System.IO;

public class ActivityManager : MonoBehaviour
{
    private GameObject whiteboard;
    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void StartActivity(string activityName, int numberOfGroups)
    {
        if (activityName.Equals("whiteboard"))
        {
            for(int i = 1; i <= numberOfGroups; i++)
            {
                whiteboard = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Activities", "Whiteboard"), new Vector3(0f, 0f, 0f), Quaternion.identity);
                PV.RPC("SetVisibility", RpcTarget.All, activityName, true);
                //SetVisibility(activityName, true);
            }
        }
    }

    public void EndActivity(string activityName)
    {
        if (activityName.Equals("whiteboard"))
        {
            SetVisibility(activityName, false);
        }
    }

    [PunRPC]
    public void SetVisibility(string activityName, bool state)
    {

        GameObject participant = GameObject.Find("CurrentParticipant");
        whiteboard = GameObject.Find("Whiteboard(Clone)");

        if (activityName.Equals("whiteboard"))
        {
            if (state)
            {
                whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(true, participant);
            }
            else if (!state)
            {
                foreach (GameObject whiteboardL in TeacherPannel.GetList("whiteboardList"))
                {
                    if(whiteboardL != null)
                        Destroy(whiteboardL); //Destroy the whiteboards to prevent the instantiation of duplicates
                }

                TeacherPannel.ModifyList("whiteboardClear", null);  //Clear the whiteboard list
            }
        }
    }
}

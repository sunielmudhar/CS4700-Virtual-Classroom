using UnityEngine;
using Photon.Pun;
using System.IO;

public class ActivityManager : MonoBehaviour
{
    [SerializeField] private GameObject whiteboard;
    PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void StartActivity(string activityName, int numberOfGroups)
    {
        if (activityName.Equals("whiteboard"))
        {
            PV.RPC("SetVisibility", RpcTarget.All, activityName, true);
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
        if (activityName.Equals("whiteboard"))
        {
            if (state)
            {
                whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(true);
            }
            else if (!state)
            {
                whiteboard.GetComponentInChildren<WhiteboardManager>().SetActive(false);
            }
        }
    }
}

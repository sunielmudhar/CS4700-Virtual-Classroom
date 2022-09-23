using Photon.Pun;
using UnityEngine;

public class InteractionPointManager : MonoBehaviour, IPunObservable
{

    //Sets the seat as occupied and then syncs across other clients

    [SerializeField] private bool bl_IsOccupied = false;
    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Occupy(bool state)
    {
        bl_IsOccupied = state;
    }

    public bool IsOccupied()
    {
        return bl_IsOccupied;
    }

    
    [PunRPC]
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
           // stream.SendNext(bl_IsOccupied);
        }
        else if (stream.IsReading)
        {
            //this.Occupy((bool)stream.ReceiveNext());
        }
    }
}

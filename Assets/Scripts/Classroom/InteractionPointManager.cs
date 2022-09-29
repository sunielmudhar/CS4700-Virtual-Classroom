using Photon.Pun;
using UnityEngine;

public class InteractionPointManager : MonoBehaviour
{

    //Sets the seat as occupied and then syncs across other clients

    [SerializeField] private bool bl_IsOccupied = false;

    public void Occupy(bool state)
    {
        bl_IsOccupied = state;
    }

    public bool IsOccupied()
    {
        return bl_IsOccupied;
    }
}

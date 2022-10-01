using Photon.Pun;
using UnityEngine;

public class InteractionPointManager : MonoBehaviour
{
    //Interaction point script assigned to chairs
    //Assigns occupied status if a participant is in a chair

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

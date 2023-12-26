using Photon.Pun;
using UnityEngine;

public class InteractionPointManager : MonoBehaviour
{
    //Interaction point script assigned to chairs
    //Assigns occupied status if a participant is in a chair

    [SerializeField] private bool bl_IsOccupied = false;
    [SerializeField] private string str_OccupantRefCode;

    public void Occupy(bool state, string refCode)
    {
        bl_IsOccupied = state;
        str_OccupantRefCode = refCode;
    }

    public bool IsOccupied()
    {
        return bl_IsOccupied;
    }

    public string GetRefCode()
    {
        return str_OccupantRefCode;
    }
}

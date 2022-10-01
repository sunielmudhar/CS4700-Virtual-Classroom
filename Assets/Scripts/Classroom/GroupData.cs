using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupData : MonoBehaviour
{
    /// <summary>
    /// Following script file creates and assigns a group value to game objects such as
    /// tables and students
    /// </summary>

    [SerializeField] private int int_GroupID = 0;

    public void SetGroupID(int int_ID)
    {
        int_GroupID = int_ID;
    }

    public int GetGroupID()
    {
        return int_GroupID;
    }
}

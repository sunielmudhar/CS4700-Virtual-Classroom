using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Permissions : MonoBehaviour
{
    /*This script file will firstly check what type of participant is trying to access a perm, before allowing them to use the perm*/
    /*Unused script*/

    public void CheckParticipantType(string permType) 
    {
        if (permType.Equals("Teacher"))
        {
            //Teacher permissions
        }
        else if (permType.Equals("Student"))
        {
            //Student permissions
        }
    }

}

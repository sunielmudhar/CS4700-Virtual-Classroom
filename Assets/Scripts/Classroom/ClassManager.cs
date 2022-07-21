using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ClassManager : MonoBehaviour
{
    GameObject participant;

    void Awake()
    {
        participant = GameObject.Find("Participant(Clone)");
    }

    public void CreateStudentList()
    {
        if (participant)
        {
        
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ParticipantSpawner : MonoBehaviour
{
    public GameObject participantPrefab, classManager;
    public Transform spawnPoint;
        
    void Start()
    {
        InstantiateParticipants();
    }

    [PunRPC]
    public void InstantiateParticipants()
    {
        GameObject participant = PhotonNetwork.Instantiate(participantPrefab.name, spawnPoint.position, Quaternion.identity);
    }

}

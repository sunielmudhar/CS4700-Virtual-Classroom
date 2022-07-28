using UnityEngine;
using Photon.Pun;
using System.IO;

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
        GameObject participant = PhotonNetwork.Instantiate(Path.Combine("Prefabs/Participant", "Participant"), spawnPoint.position, Quaternion.identity);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;
using System.Threading.Tasks;

public class NetworkRunnerHandler : MonoBehaviour
{

    /*TEST SCRIPT*/

    public NetworkRunner networkRunnerPrefab;

    public NetworkRunner networkRunner;

    void Start()
    {
        networkRunner = Instantiate(networkRunnerPrefab);
        networkRunner.name = "Network Runner";

        //AutoHostOrClient sets up network authority, who ever starts the scene is a host
        var clientTask = InitialiseNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

        Debug.Log("Server NetworkRunner has started");
    }

    protected virtual Task InitialiseNetworkRunner(NetworkRunner runner, GameMode gamemode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialised) {

        //Check for a scene object provider, which handles objects that are already in the scene over the network

        var sceneObjectProvider = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneObjectProvider == null) {
            sceneObjectProvider = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        return runner.StartGame(new StartGameArgs { GameMode = gamemode, Address = address, Scene = scene, SessionName = "Classroom", Initialized = initialised, SceneManager = sceneObjectProvider });
    }

}

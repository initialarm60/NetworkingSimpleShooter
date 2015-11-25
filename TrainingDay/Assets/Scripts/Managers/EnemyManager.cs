using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;


public delegate void killedPlayer(PlayerHealth player);
public delegate void killedEnemy(PlayerHealth player, EnemyHealth enemy);

public class EnemyManager : NetworkBehaviour
{
	// The players to track and kill
    private PlayerHealth[] playersToFollow;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

	public void reset() {
		playersToFollow = null;
		enemy = null;
		spawnPoints = null;
	}



	// killed a player
	public void killedPlayer(PlayerHealth health) {
		List<PlayerHealth> filteredArray = new List<PlayerHealth> ();
		for (int index = 0; index < numOfLivingPlayers; index++) {
			if (playersToFollow[index] != health) {
				filteredArray.Add(playersToFollow[index]);
			}
		}

		playersToFollow = filteredArray.ToArray();
	}

	// set things up for the game
	void OnLevelWasLoaded() {
		lobbyManager = (NetworkLobbyManager)FindObjectOfType (typeof(NetworkLobbyManager));
		numOfLivingPlayers = lobbyManager.numPlayers;
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	// spawn the enemies
    void Spawn ()
    {
        if(numOfLivingPlayers <= 0f) {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        Network.Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation, 0);
    }

}

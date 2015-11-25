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
	private bool shouldSpawnEnemies = false;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

	public void reset() {
		playersToFollow = null;
		enemy = null;
		spawnPoints = null;
	}

	public void start() {
		GameManager gameManager = FindObjectOfType<GameManager>();
		gameManager.setupEnemyManger (this);
	}


	// killed a player
	public void killedPlayer(PlayerHealth health) {
		List<PlayerHealth> filteredArray = new List<PlayerHealth> ();
		for (int index = 0; index < playersToFollow.Length; index++) {
			if (playersToFollow[index] != health) {
				filteredArray.Add(playersToFollow[index]);
			}
		}
		playersToFollow = filteredArray.ToArray();
	}

	public void setShouldSpawnEnemies(bool shouldSpawnEnemies) {
		shouldSpawnEnemies = shouldSpawnEnemies;
	}

	// set things up for the game
	void OnLevelWasLoaded() {
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	// spawn the enemies
    void Spawn ()
    {
		if (isServer) {
			if (playersToFollow.Length <= 0) {
				return;
			}

			int spawnPointIndex = Random.Range (0, spawnPoints.Length);
			Network.Instantiate (enemy, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation, 0);
		}
    }

}

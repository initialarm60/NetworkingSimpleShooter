using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BMAEnemyManager : NetworkBehaviour
{
	/// The specific enemy assoicated wiht the manager set in the scene
	public GameObject enemy;
	
	/// How fast enemies will spawn set in the scene
	public float spawnTime = 3f;
	
	/// Spawn points for the manager, for now one, eventually many
	public Transform[] spawnPoints;
	
	
	/// The players to track and kill. Interact with game manager to get players
	private PlayerHealth[] playersToFollow;
	
	
	public void Start() {
		GameManager gameManager = FindObjectOfType<GameManager>();
		gameManager.setupEnemyManger (this);
	}

	public void Update() {
		print ("updated");
	}
	
	public void setPlayersToFollow(PlayerHealth[] players) {
		playersToFollow = players;
	}
	
	/// killed a player
	public void killedPlayer(PlayerHealth health) {
		List<PlayerHealth> filteredArray = new List<PlayerHealth> ();
		for (int index = 0; index < playersToFollow.Length; index++) {
			if (playersToFollow[index] != health) {
				filteredArray.Add(playersToFollow[index]);
			}
		}
		playersToFollow = filteredArray.ToArray();
	}
	
	public void startSpawningEnemies() {
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}
	
	public void stopSpawningEnemies() {
		CancelInvoke ("Spawn");
	}
	
	/// spawn the enemies
	private void Spawn () {
		//if (isServer) {
			if (playersToFollow.Length <= 0) {
				return;
			}
			int spawnPointIndex = Random.Range (0, spawnPoints.Length);
			print ("EnemySpawned");
			Vector3 position = spawnPoints[spawnPointIndex].position;
			Quaternion rotation = spawnPoints[spawnPointIndex].rotation;
			enemy = (GameObject)Instantiate(enemy, position, rotation);
			NetworkServer.Spawn (enemy);

		//	Network.Instantiate (enemy, position, rotation, 0);
	//	}
	}
	
}

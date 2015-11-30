using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// This class receives the player information necessary for 
// the game to be played provided by the network manager. It communicates
// with the score manager and the enemy manager. 
public class GameManager : NetworkBehaviour {

	// MANAGERS: access to managers of the game

	/// have access to the network manager to get information about the players
	private BMANetworkManager lobbyManager;
	// access to the enemy manager to spawn enemies
	private BMAEnemyManager enemyManager;
	// access to the score manager to update scores
	private ScoreManager scoreManager;

	private GameObject localPlayer;

	// GAME VARIABLES:

	private PlayerHealth[] playerHealths;
	private int numberOfTotalPlayers = 0;
	private int numberOfLivingPlayers = 0;

	// LIFE CYCLE:

	// Use this for initialization
	void Start () {
		getLobbyManager();
		scoreManager = GetComponent<ScoreManager> ();
		getPlayers();
	}
	
	private void getLobbyManager() {
		lobbyManager = FindObjectOfType<BMANetworkManager> ();
		setNumberOfTotalPlayers (lobbyManager.numPlayers);
		setNumberOfLivingPlayers (lobbyManager.numPlayers);
	}

	private void getPlayers() {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		playerHealths = new PlayerHealth[players.Length];
		for (int i = 0; i < players.Length; i++) {
			PlayerHealth instance = players[i].GetComponentInChildren<PlayerHealth>();
			if (instance.isLocalPlayer) {
				instance.configurePlayerHud();
				playerHealths[i] = instance;
			}
		}
	}

	public void setupEnemyManger(BMAEnemyManager manager) {
		enemyManager = manager;
		enemyManager.setPlayersToFollow(playerHealths);
		enemyManager.startSpawningEnemies();
	}

	// Update is called once per frame
	void Update () {
		// only want to do game updates on the server, then tell the clients wtf to do
		if (isServer) {
			// have the server perform checks to see if the game is done
			if (isGameOver()) {
				// Tell the clients GG becuase they lost.
				RpcGameOver();
			} 
		}
	}

	// GAME STATE:
	
	public void setNumberOfTotalPlayers(int players) {
		numberOfTotalPlayers = players;
	}

	public void setNumberOfLivingPlayers(int players) {
		numberOfLivingPlayers = players;
	}

	// Determines if the game is considered finished
	public bool isGameOver() {
		return numberOfTotalPlayers - numberOfLivingPlayers <= 0;
	}

	/// Have the clients run the game over scenario.
	[ClientRpc]
	public void RpcGameOver() {
		// game over manager show game over screen.

	}
}

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
	private EnemyManager enemyManager;
	// access to the score manager to update scores
	private ScoreManager scoreManager;

	// GAME VARIABLES:

	private PlayerHealth[] players;
	private int numberOfTotalPlayers = 0;
	private int numberOfLivingPlayers = 0;

	// LIFE CYCLE:

	// Use this for initialization
	void Start () {
		setupLobbyManager();
		scoreManager = GetComponent<ScoreManager> ();
	}

	private void setupLobbyManager() {
		lobbyManager = FindObjectOfType<BMANetworkManager> ();
		setNumberOfTotalPlayers (lobbyManager.numPlayers);
		setNumberOfLivingPlayers (lobbyManager.numPlayers);
	}

	public void setupEnemyManger(EnemyManager manager) {
		enemyManager = manager;
		players = new PlayerHealth[0];
		enemyManager.setShouldSpawnEnemies (true);
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

	// When a player's player health script is run, it will add it to the gmea 
	public void addPlayerHealthToGameManager(PlayerHealth player) {
		PlayerHealth[] added = new PlayerHealth[players.Length + 1];
		for (int index = 0; index < players.Length; index++) {
			added[index] = players[index];
		}
		added [players.Length] = player;

		players = added;
	}

	// Determines if the game is considered finished
	public bool isGameOver() {
		return numberOfTotalPlayers - numberOfLivingPlayers <= 0;
	}

	[ClientRpc]
	public void RpcGameOver() {
		// game over manager show game over screen.

	}

	
}

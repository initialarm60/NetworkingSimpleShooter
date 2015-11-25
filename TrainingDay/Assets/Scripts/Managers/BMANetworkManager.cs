using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

// This is a singleton so the server and clients will access the same object. Ithink???
// This class will handle all management so that the server is in complete control.
// 
public class BMANetworkManager : NetworkLobbyManager {
	
	// The game manager
	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		print("NetworkManager: Start()");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	
	// Server Specific
	public void OnLobbyServerConnent(NetworkConnection conn) {
		print("OnLobbyServerConnect with conn hostId:" + conn.hostId);
	}
	
	public void OnLobbyServerPlayersReady() {
		print ("OnLobbyServerPlayersReady()");
	}
	/*
	// Everyone is ready, setup the level
	public override void OnLobbyServerPlayersReady() {
		Debug ("OnLobbyServerPlayersReady()");
		// change scene
		ServerChangeScene (playScene);
	}
*/
	public override void OnLobbyServerSceneChanged(string sceneName) {
		print("OnLobbyServerSceneChanged" + sceneName);
		//gameManager = GetComponent<GameManager> ();
		//	gameManager.setNumberOfPlayers (this.numPlayers);
	}

	public void OnLevelWasLoaded(int level){
		print ("level" + level);
	}
	
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
		
		return true;
	}
	
	
	// Client Specific
	public override void OnLobbyClientSceneChanged(NetworkConnection conn) {
		print("OnLobbyClientSceneChanged for conn with id" + conn.address);
	}
	
	
}

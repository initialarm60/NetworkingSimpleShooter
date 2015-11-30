using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;


	// TODO: update to find the closest player and go after him
    void Awake () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
    }


    void Update () {
        if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0) {
            nav.SetDestination (player.position);
        }
        else {
            nav.enabled = false;
        }
    }
}

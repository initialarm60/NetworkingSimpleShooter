using UnityEngine;
using UnityEngine.Networking;

public class GameOverManager : NetworkBehaviour
{
    public PlayerHealth playerHealth;
	public float restartDelay = 5f;

    Animator anim;
	float restartTimer;


    void Awake() {
		anim = GetComponent<Animator> ();

		if (isLocalPlayer) {

		}
	}


    void Update()
	{
		if (playerHealth.currentHealth <= 0) {
			anim.SetTrigger ("GameOver");

			restartTimer += Time.deltaTime;

			if (restartTimer >= restartDelay) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}
}

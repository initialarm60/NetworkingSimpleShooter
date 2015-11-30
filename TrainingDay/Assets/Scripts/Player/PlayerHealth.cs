using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHealth : NetworkBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	private bool levelWasLoaded = false;

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
	
	void Start() {
		// add the player to the manager
		anim = GetComponent <Animator> ();
		playerAudio = GetComponent <AudioSource> ();
		currentHealth = startingHealth;
	}

	// meh, no reason to have the server hook this up
	public override void OnStartLocalPlayer() {
		playerMovement = GetComponent <PlayerMovement> ();
		playerShooting = GetComponentInChildren <PlayerShooting> ();
	}

	// HACK: no clue why the player is created before the scene
	// so instead of reading, just setting this from the game manager
	public void configurePlayerHud() {
		levelWasLoaded = true;
		Canvas myCanvas = (Canvas)FindObjectOfType(typeof(Canvas));
		damageImage = GameObject.Find ("HUDCanvas").GetComponent<Canvas> ().GetComponentInChildren<Image>();
	}

	/// Determines if the instance of PlayerHealth is the local instance
	public bool isLocalInstance() {
		return (isLocalPlayer ? true : false);
	}

    void Update () {
		if (levelWasLoaded) {
			if (isLocalPlayer) {
				if (damaged) {
					damageImage.color = flashColour;
				} else {
					damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
				}
				damaged = false;
			}
		}
    }


    public void TakeDamage (int amount) {
        damaged = true;
        currentHealth -= amount;
        healthSlider.value = currentHealth;
        playerAudio.Play ();
        if(currentHealth <= 0 && !isDead) {
            Death ();
        }
    }


    void Death () {
        isDead = true;
        playerShooting.DisableEffects ();
        anim.SetTrigger ("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play ();
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    public void RestartLevel () {
        Application.LoadLevel (Application.loadedLevel);
    }
}

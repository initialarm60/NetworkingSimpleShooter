using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
	// TODO: maybe implement powerups later
    public int damagePerShot = 20;
	// TODO: again, powerups later
    public float timeBetweenBullets = 0.15f;
	// bullet distance travel
    public float range = 100f;
	// attach stuff once the player is ready
	private bool started = false;
	float effectsDisplayTime = 0.2f;
	
	float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
	Transform barrelEnd;
	PlayerHealth health;
	[SyncVar]
    ParticleSystem gunParticles;
	[SyncVar]
    LineRenderer gunLine;
	[SyncVar]
    AudioSource gunAudio;
	[SyncVar]
    Light gunLight;



	public override void OnStartLocalPlayer() {
		started = true;
		health = GetComponent<PlayerHealth> ();
		barrelEnd = findBarrelTip ();
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponentInChildren<ParticleSystem>();
		gunLine = GetComponentInChildren<LineRenderer> ();
		gunAudio = GetComponentsInChildren<AudioSource> ()[1];
		gunLight = GetComponentInChildren<Light> ();
	}

	private Transform findBarrelTip() {
		Transform[] childTransforms = GetComponentsInChildren<Transform> ();
		foreach (Transform childTransform in childTransforms) {
			if (childTransform.tag == "BarrelTip") {
				return childTransform;
			}
		}
		return null;
	}

    void Update () {
		if (started) {
			if (isLocalPlayer) {
				timer += Time.deltaTime;

				if (Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0) {
					Shoot ();
				}

				if (timer >= timeBetweenBullets * effectsDisplayTime) {
					DisableEffects ();
				}
			}
		}
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot() {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, barrelEnd.position);

        shootRay.origin = barrelEnd.position;
        shootRay.direction = barrelEnd.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (health, damagePerShot, shootHit.point);
            }
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}

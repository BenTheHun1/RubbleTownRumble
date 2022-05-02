using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

public class EnemyAICharacterJoints : MonoBehaviour
{
	public AudioSource dwarfSource;
	public AudioClip[] swordHit;
	public AudioClip axeSwing;
	public AudioClip[] callout;
	public AudioClip[] death;
	public AudioClip[] hurt;
	public Material[] outfits;
	public ParticleSystem particleEffect;
	public ParticleSystem bloodEffect;
	public Vector3 lastPos;
	public GameObject rootJoint;
	public GameObject beard;
	public Rigidbody rootRigid;
	public CapsuleCollider rootCapCollide;
	public Rigidbody[] rigids;
	public CapsuleCollider[] capColliders;
	public BoxCollider[] boxColliders;
	public Animator animEnemy;
	public Renderer rend;
	public Text healthDisplay;
	public float Health;
	public float movementSpeed;
	public bool isRagdoll;
	public bool isKicked;
	public bool GetUp;
	public bool skipDeathStruggle;
	public bool exploded;
	private bool isResettingAttack;
	private bool isDEAD;
	private int chance = 1;
	private GameObject player;
	private GameObject spawnManager;
	private Quaternion qTo;
	private Color32 color;
	private float lookSpeed = 2.0f;
	private float stoppingradius = 1.7f;
	private HealthSystem hs;
	private NavMeshAgent navMeshAgent;

	void Start()
	{
		dwarfSource = GetComponent<AudioSource>();
		isResettingAttack = true;
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.speed = movementSpeed;
		hs = GameObject.Find("Player").GetComponent<HealthSystem>();
		rootRigid = GetComponent<Rigidbody>();
		rootCapCollide = GetComponent<CapsuleCollider>();
		rigids = GetComponentsInChildren<Rigidbody>();
		chance = Random.Range(0, 2);
		if (chance == 1)
        {
			rend.material = outfits[Random.Range(0, outfits.Length)];
		}
		capColliders = GetComponentsInChildren<CapsuleCollider>();
		boxColliders = GetComponentsInChildren<BoxCollider>();
		animEnemy = transform.root.GetComponent<Animator>();
		player = GameObject.Find("Player");
		spawnManager = GameObject.Find("Spawns");
		StartCoroutine(Bark());
	}

	void Update()
	{

		if (transform.root.GetComponent<EnemyAICharacterJoints>().Health < 0)
		{
			healthDisplay.text = "0";
		}
		else
		{
			healthDisplay.text = transform.root.GetComponent<EnemyAICharacterJoints>().Health.ToString("N0");
		}

		if (navMeshAgent.velocity.magnitude > 0)
        {
			Vector3 lookDirection = navMeshAgent.velocity.normalized;//(player.transform.position - transform.position).normalized;
			lookDirection.y = 0;
			qTo = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, 90, 0);
		}

		rootJoint.transform.SetParent(transform, true);
		if (!animEnemy.GetCurrentAnimatorStateInfo(0).IsName("Walk") || isRagdoll)
		{
			navMeshAgent.speed = 0;
		}
		else
		{
			navMeshAgent.speed = movementSpeed;
		}

		if (Vector3.Distance(player.transform.position, transform.position) > stoppingradius && !isRagdoll && !animEnemy.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
		{
			animEnemy.SetTrigger("Walking");
			if (navMeshAgent.velocity.magnitude > 0)
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, qTo, Time.deltaTime * lookSpeed);
			}
			navMeshAgent.destination = player.transform.position;
		}
		else
		{
			if (!isRagdoll && isResettingAttack)
			{
				animEnemy.SetTrigger("Attacking");
				isResettingAttack = false;
				StartCoroutine(ResetAttack());
			}
		}

		if (gameObject.transform.position.y < -25)
		{
			spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(gameObject);
			spawnManager.GetComponent<SpawnManager>().enemiesToSpawn++;
			Destroy(gameObject);
		}

		if (GetUp)
		{
			Quaternion q = Quaternion.FromToRotation(rootJoint.transform.up, Vector3.up) * rootJoint.transform.rotation;
			rootJoint.transform.rotation = Quaternion.Slerp(rootJoint.transform.rotation, q, Time.deltaTime * lookSpeed);
			rootJoint.transform.localPosition = Vector3.Slerp(rootJoint.transform.localPosition, new Vector3(rootJoint.transform.localPosition.x, rootJoint.transform.localPosition.y + 0.3f, rootJoint.transform.localPosition.z), Time.deltaTime * 2f);
		}
	}

	void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Slope"))
        {
			rootRigid.AddForce(new Vector3(0, 20, 0), ForceMode.Force);
        }
    }

	public void Hit(float damage)
	{
		animEnemy.SetTrigger("Ouch");
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		var bloodParticle = Instantiate(bloodEffect, rootJoint.transform.position, rootJoint.transform.rotation);
		bloodParticle.Play();
		dwarfSource.PlayOneShot(swordHit[Random.Range(0, swordHit.Length)]);
		if (Health <= hs.AttackAmount)
		{
			dwarfSource.PlayOneShot(death[Random.Range(0, death.Length)]);
		}
		Health -= damage / hs.AttackAmount;
		Debug.Log("Hit! for " + damage / hs.AttackAmount);
		if (Health <= 0 && !isDEAD)
		{
			if (!isRagdoll)
			{
				Ragdoll();
				rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
				GetUp = false;
			}
			StartCoroutine(FinalDeath());
		}
		else
		{
			if (Health > 0)
			{
				color = new Color32(255, 0, 0, 0);
				rend.material.color = color;
				StartCoroutine(React());
			}
		}
	}

	public void Ragdoll()
	{
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		isRagdoll = true;
		animEnemy.enabled = false;
		Destroy(rootRigid);
		rootCapCollide.enabled = false;
		foreach (Rigidbody rb in rigids)
		{
			if (rb != null)
			{
				rb.useGravity = true;
				rb.isKinematic = false;
			}
		}
		foreach (CapsuleCollider cc in capColliders)
		{
			if (cc != null && cc != rootCapCollide)
			{
				cc.enabled = true;
			}
		}
		foreach (BoxCollider bc in boxColliders)
		{
			if (bc != null && bc.gameObject.name != "RootJoint" && bc.gameObject.name != "Beard")
			{
				bc.enabled = true;
			}
		}
		StartCoroutine(KnockedDown());
	}

	IEnumerator KnockedDown()
	{
		if (Health <= 0 && exploded && !dwarfSource.isPlaying)
		{
			dwarfSource.PlayOneShot(death[Random.Range(0, death.Length)]);
		}
		isKicked = true;
		yield return new WaitForSeconds(0.5f);
		isKicked = false;
		yield return new WaitForSeconds(3f);
		if (Health > 0)
		{
			GetUp = true;
			rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
			StartCoroutine(WakingUp());
		}
		else
		{
			if (Health <= 0 && exploded)
			{
				rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
				GetUp = false;
				StartCoroutine(FinalDeath());
			}
		}
	}

	IEnumerator WakingUp()
	{
		yield return new WaitForSeconds(3f);
		{
			if (Health > 0)
			{
				GetUp = false;
				rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
				isRagdoll = false;
				foreach (Rigidbody rb in rigids)
				{
					if (rb != null)
					{
						rb.gameObject.transform.rotation = Quaternion.identity;
						rb.useGravity = false;
						rb.isKinematic = true;
					}
				}
				foreach (CapsuleCollider cc in capColliders)
				{
					if (cc != null)
					{
						cc.enabled = false;
					}
				}
				foreach (BoxCollider bc in boxColliders)
				{
					if (bc != null && bc.gameObject.name != "RootJoint" && bc.gameObject.name != "Beard")
					{
						bc.enabled = false;
					}
				}
				animEnemy.enabled = true;
				lastPos = rootJoint.transform.position;
				transform.position = lastPos;
				rootJoint.transform.position = lastPos;
				if (rootRigid == null)
				{
					rootRigid = gameObject.AddComponent<Rigidbody>();
					rootRigid.constraints = RigidbodyConstraints.FreezeRotation;
				}
				rootCapCollide.enabled = true;
			}
		}
	}

	IEnumerator FinalDeath()
	{
		color = new Color32(108, 0, 0, 0);
		rend.material.color = color;
		isDEAD = true;
		beard.transform.parent = null;
		beard.AddComponent<XRGrabInteractable>();
		beard.layer = LayerMask.NameToLayer("Beard");
		beard.AddComponent<Rigidbody>();
		beard.GetComponent<MeshCollider>().enabled = true;
		beard.GetComponent<Cloth>().clothSolverFrequency = 175;
		beard.gameObject.tag = "Item";
		yield return new WaitForSeconds(3f);
		if (!skipDeathStruggle)
        {
			rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
			GetUp = true;
		}
		yield return new WaitForSeconds(2f);
		GetUp = false;
		rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		yield return new WaitForSeconds(2f);
		var particle = Instantiate(particleEffect, rootJoint.transform.position, rootJoint.transform.rotation);
		particle.Play();
		spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(gameObject);
		Destroy(gameObject);
	}

	IEnumerator React()
	{
		dwarfSource.PlayOneShot(hurt[Random.Range(0, hurt.Length)]);
		yield return new WaitForSeconds(0.05f);
		color = new Color32(255, 255, 255, 0);
		rend.material.color = color;
		animEnemy.ResetTrigger("Ouch");
	}

	IEnumerator ResetAttack()
	{
		dwarfSource.PlayOneShot(axeSwing);
		yield return new WaitForSeconds(1f);
		animEnemy.SetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		isResettingAttack = true;
	}

	IEnumerator Bark()
	{
		if (!isRagdoll && !dwarfSource.isPlaying)
        {
			dwarfSource.PlayOneShot(callout[Random.Range(0, callout.Length)]);
		}
		yield return new WaitForSeconds(Random.Range(10, 30));
		StartCoroutine(Bark());
	}
}

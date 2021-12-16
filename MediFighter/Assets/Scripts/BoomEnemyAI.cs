using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoomEnemyAI : MonoBehaviour
{
	public AudioSource dwarfSource;
	public AudioClip hiss;
	public ParticleSystem particleEffect;
	public ParticleSystem bloodEffect;
	public ParticleSystem fuseEffect;
	public ParticleSystem explosionEffect;
	public Vector3 lastPos;
	public GameObject rootJoint;
	public Rigidbody rootRigid;
	public CapsuleCollider rootCapCollide;
	public Rigidbody[] rigids;
	public CapsuleCollider[] capColliders;
	public BoxCollider[] boxColliders;
	public Animator animEnemy;
	public Animator kegAnim;
	public Renderer rend;
	public int Health;
	public float movementSpeed;
	public bool isRagdoll;
	public bool isKicked;
	public bool GetUp;
	public bool invincible;
	public bool kaboom;
	public bool skipDeathStruggle;
	private Quaternion qTo;
	private GameObject player;
	private GameObject spawnManager;
	private float lookSpeed = 2.0f;
	private float stoppingradius = 1.7f;
	private float explosionRadius;
	private Color32 color;
	private NavMeshAgent navMeshAgent;
	private HealthSystem hs;

	void Start()
	{
		Health = 1;
		explosionRadius = 3f;
		dwarfSource = GetComponent<AudioSource>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		navMeshAgent.speed = movementSpeed;
		rootRigid = GetComponent<Rigidbody>();
		rootCapCollide = GetComponent<CapsuleCollider>();
		rigids = GetComponentsInChildren<Rigidbody>();
		capColliders = GetComponentsInChildren<CapsuleCollider>();
		boxColliders = GetComponentsInChildren<BoxCollider>();
		animEnemy = transform.root.GetComponent<Animator>();
		player = GameObject.Find("Player");
		hs = GameObject.Find("Player").GetComponent<HealthSystem>();
		spawnManager = GameObject.Find("Spawns");
	}

	void Update()
	{
		Vector3 lookDirection = (player.transform.position - transform.position).normalized;
		lookDirection.y = 0;
		qTo = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, 90, 0);
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
			transform.rotation = Quaternion.Slerp(transform.rotation, qTo, Time.deltaTime * lookSpeed);
			navMeshAgent.destination = player.transform.position;
		}
		else
		{
			if (!isRagdoll && Vector3.Distance(player.transform.position, transform.position) <= stoppingradius)
			{
				StartCoroutine(Kaboom());
			}
		}

		if (gameObject.transform.position.y < -25)
		{
			spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(gameObject);
			Destroy(gameObject);
		}

		if (kaboom)
        {
			fuseEffect.Emit(1);
        }
	}

	void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.CompareTag("Slope"))
		{
			rootRigid.AddForce(new Vector3(0, 20, 0), ForceMode.Force);
		}
	}

	public void Ragdoll()
	{
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		isRagdoll = true;
		animEnemy.enabled = false;
		Destroy(rootRigid);
		Destroy(rootCapCollide);
		foreach (Rigidbody rb in rigids)
		{
			if (rb != null)
			{
				rb.isKinematic = false;
			}
		}
		foreach (CapsuleCollider cc in capColliders)
		{
			if (cc != null)
			{
				cc.enabled = true;
			}
		}
		foreach (BoxCollider bc in boxColliders)
		{
			if (bc != null && bc.gameObject.name != "RootJoint")
			{
				bc.enabled = true;
			}
		}
		if (Health <= 0)
		{
			color = new Color32(108, 0, 0, 0);
			rend.material.color = color;
		}
		StartCoroutine(Kaboom());
	}
	public void Slashed()
    {
		animEnemy.SetTrigger("Ouch");
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		var bloodParticle = Instantiate(bloodEffect, rootJoint.transform.position, rootJoint.transform.rotation);
		bloodParticle.Play();
		Health = 0;
		rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		GetUp = false;
		Ragdoll();
	}

	IEnumerator Kaboom()
	{
		animEnemy.SetTrigger("AboutToExplode");
		kegAnim.SetTrigger("Explosion");
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		kaboom = true;
		if (!dwarfSource.isPlaying)
        {
			dwarfSource.PlayOneShot(hiss);
		}
		yield return new WaitForSeconds(1f);
		Vector3 explosionPos = rootJoint.transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, 3);
		float dist = Vector3.Distance(player.transform.position, transform.position);
		if (dist < explosionRadius)
        {
			hs.DamagePlayer();
		}
		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponent<Rigidbody>();

			if (rb != null && rb.gameObject != rootJoint && rb.gameObject.CompareTag("Enemy") && rb.transform.root.GetComponent<EnemyAICharacterJoints>())
			{
				rb.transform.root.GetComponent<EnemyAICharacterJoints>().exploded = true;
				rb.transform.root.GetComponent<EnemyAICharacterJoints>().Health = 0;
				rb.transform.root.GetComponent<EnemyAICharacterJoints>().Ragdoll();
				rb.AddExplosionForce(800, rootJoint.transform.position, 2, 0.3f, ForceMode.Impulse);
			}
			else
            {
				if(rb != null && rb.gameObject != rootJoint && rb.gameObject.CompareTag("Enemy") && rb.transform.root.GetComponent<BoomEnemyAI>())
                {
					//rb.transform.root.GetComponent<EnemyAICharacterJoints>().exploded = true;
					rb.transform.root.GetComponent<BoomEnemyAI>().Health = 0;
					rb.transform.root.GetComponent<BoomEnemyAI>().Ragdoll();
					rb.AddExplosionForce(800, rootJoint.transform.position, 2, 0.3f, ForceMode.Impulse);
				}
            }
		}
		var explosionParticle = Instantiate(explosionEffect, rootJoint.transform.position, rootJoint.transform.rotation);
		explosionParticle.Play();
		spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(gameObject);
		Destroy(gameObject);
	}
}

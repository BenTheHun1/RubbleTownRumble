using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEnemyAI : MonoBehaviour
{
	public ParticleSystem particleEffect;
	public ParticleSystem bloodEffect;
	public ParticleSystem fuseEffect;
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
	public bool isAttacking;
	public bool isWalking;
	public bool GetUp;
	public bool invincible;
	public bool kaboom;
	public bool skipDeathStruggle;
	private Quaternion qTo;
	private GameObject player;
	private GameObject spawnManager;
	private float lookSpeed = 2.0f;
	private float stoppingradius = 1.7f;
	private Color32 color;
	void Start()
	{
		isWalking = true;
		Health = 1;
		rootRigid = GetComponent<Rigidbody>();
		rootCapCollide = GetComponent<CapsuleCollider>();
		rigids = GetComponentsInChildren<Rigidbody>();
		capColliders = GetComponentsInChildren<CapsuleCollider>();
		boxColliders = GetComponentsInChildren<BoxCollider>();
		animEnemy = transform.root.GetComponent<Animator>();
		player = GameObject.Find("Player");
		spawnManager = GameObject.Find("Spawns");
	}

	void Update()
	{
		Vector3 lookDirection = (player.transform.position - transform.position).normalized;
		lookDirection.y = 0;
		qTo = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0, 90, 0);
		rootJoint.transform.SetParent(transform, true);
		if (isWalking && Vector3.Distance(player.transform.position, transform.position) > stoppingradius && !isRagdoll && !animEnemy.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
		{
			isWalking = true;
			isAttacking = false;
			animEnemy.SetTrigger("Walking");
			transform.rotation = Quaternion.Slerp(transform.rotation, qTo, Time.deltaTime * lookSpeed);
			if (animEnemy.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
			{
				transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
			}
		}
		else
		{
			if (!isRagdoll && Vector3.Distance(player.transform.position, transform.position) <= stoppingradius)
			{
				kaboom = true;
				StartCoroutine(Kaboom());
			}
		}

		if (gameObject.transform.position.y < -25)
		{
			spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(gameObject);
			Destroy(gameObject);
		}

		if (GetUp)
		{
			Quaternion q = Quaternion.FromToRotation(rootJoint.transform.up, Vector3.up) * rootJoint.transform.rotation;
			rootJoint.transform.rotation = Quaternion.Slerp(rootJoint.transform.rotation, q, Time.deltaTime * lookSpeed);
			rootJoint.transform.localPosition = Vector3.Slerp(rootJoint.transform.localPosition, new Vector3(rootJoint.transform.localPosition.x, rootJoint.transform.localPosition.y + 0.3f, rootJoint.transform.localPosition.z), Time.deltaTime * 2f);
		}

		if (kaboom)
        {
			fuseEffect.Emit(1);
        }
	}

	public void Ragdoll()
	{
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		isRagdoll = true;
		isAttacking = false;
		isWalking = false;
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
		Health = 0;
		if (Health <= 0)
		{
			rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			GetUp = false;
			Ragdoll();
		}
		else
        {
			if (Health > 0)
			{
				color = new Color32(255, 0, 0, 0);
				rend.material.color = color;
			}
		}
	}

	IEnumerator Kaboom()
	{
		kegAnim.SetTrigger("Explosion");
		isWalking = false;
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		yield return new WaitForSeconds(1f);
		Vector3 explosionPos = rootJoint.transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, 3);
		foreach (Collider hit in colliders)
		{
			Rigidbody rb = hit.GetComponent<Rigidbody>();

			if (rb != null && rb.gameObject != rootJoint && rb.gameObject.CompareTag("Enemy"))
            {
				rb.transform.root.GetComponent<EnemyAICharacterJoints>().Health = 0;
				rb.transform.root.GetComponent<EnemyAICharacterJoints>().Ragdoll();
				rb.AddExplosionForce(800, rootJoint.transform.position, 2, 0.3f, ForceMode.Impulse);
			}
		}
		Destroy(gameObject);
	}
}

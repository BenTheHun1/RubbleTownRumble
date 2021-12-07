using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAICharacterJoints : MonoBehaviour
{
	public ParticleSystem particleEffect;
	public Vector3 lastPos;
	public GameObject rootJoint;
	public Rigidbody rootRigid;
	public CapsuleCollider rootCapCollide;
	public BoxCollider rootBoxCollide;
	public Rigidbody[] rigids;
	public CapsuleCollider[] capColliders;
	public BoxCollider[] boxColliders;
	public Animator animEnemy;
	public Renderer rend;
	public int Health;
	public float movementSpeed;
	public int AttackAmount;
	public bool isDamaged;
	public bool isRagdoll;
	public bool isKicked;
	public bool isAttacking;
	public bool isWalking;
	public bool GetUp;
	private bool invincible;
	private Quaternion qTo;
	private GameObject player;
	private GameObject spawnManager;
	private float lookSpeed = 2.0f;
	private float stoppingradius = 1.7f;
	private Color32 color;

	void Start()
	{
		Health = 3;
		AttackAmount = 1;
		invincible = false;
		rootRigid = GetComponent<Rigidbody>();
		rootCapCollide = GetComponent<CapsuleCollider>();
		rootBoxCollide = GetComponent<BoxCollider>();
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
		if (Vector3.Distance(player.transform.position, transform.position) > stoppingradius && !isDamaged && !isRagdoll)
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
			if (!isDamaged && !isRagdoll)
			{
				isAttacking = true;
				isWalking = false;
				animEnemy.SetTrigger("Attacking");
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
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Boot") && !isDamaged && !isRagdoll && !invincible)
		{
			isKicked = true;
			isDamaged = true;
			Ragdoll();
		}
		else
		{
			if (collision.gameObject.CompareTag("Boot") && invincible)
			{
				StartCoroutine(InvincibilityFrame());
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Sword") && !isDamaged && !isRagdoll && !invincible)
		{
			isDamaged = true;
			StartCoroutine(Slashed());
		}
		else
        {
			if (other.gameObject.CompareTag("Sword") && invincible)
            {
				invincible = true;
				StartCoroutine(InvincibilityFrame());
			}
		}
	}

	/*void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<EnemyAICharacterJoints>().isKicked == true && other.gameObject.GetComponent<EnemyAICharacterJoints>().isRagdoll == true)
		{
			isDamaged = true;
			isKicked = true;
			Ragdoll();
		}
	}*/
	// rip domino effect

	void ResetColliders()
    {
		rootRigid = gameObject.AddComponent<Rigidbody>();
		rootRigid.constraints = RigidbodyConstraints.FreezeRotation;
		rootCapCollide = gameObject.AddComponent<CapsuleCollider>();
		rootCapCollide.center = new Vector3(0, 3, 0);
		rootCapCollide.direction = 1;
		rootCapCollide.radius = 0.4f;
		rootCapCollide.height = 6.6f;
		rootBoxCollide = gameObject.AddComponent<BoxCollider>();
		rootBoxCollide.isTrigger = true;
		rootBoxCollide.center = new Vector3(0, 3, 0);
		rootBoxCollide.size = new Vector3(4, 7, 5);
	}
	void WakeUp()
    {
		isRagdoll = false;
		foreach (Rigidbody rb in rigids)
		{
			if (rb != null)
			{
				rb.gameObject.transform.rotation = Quaternion.identity;
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
			if (bc != null)
			{
				bc.enabled = false;
			}
		}
		animEnemy.enabled = true;
		lastPos = rootJoint.transform.position;
		transform.position = lastPos;
		rootJoint.transform.position = lastPos;
	}

	void Ragdoll()
	{
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		isRagdoll = true;
		isAttacking = false;
		isWalking = false;
		animEnemy.enabled = false;
		rootRigid.constraints = RigidbodyConstraints.None;
		Destroy(rootRigid);
		Destroy(rootCapCollide);
		Destroy(rootBoxCollide);
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
			if (bc != null)
			{
				bc.enabled = true;
			}
		}
		if (Health > 0)
		{
			color = new Color32(108, 108, 108, 0);
			rend.material.color = color;
		}
		else
        {
			if (Health <= 0)
			{
				color = new Color32(108, 0, 0, 0);
				rend.material.color = color;
			}
		}
		StartCoroutine(KnockDown());
	}
	IEnumerator Slashed()
    {
		Health -= AttackAmount;
		if (Health <= 0)
		{
			Ragdoll();
			StartCoroutine(FinalDeath());
		}
		else
        {
			if (Health > 0)
			{
				StartCoroutine(InvincibilityFrame());
				yield return new WaitForSeconds(0.75f);
				isDamaged = false;
			}
		}

	}

	IEnumerator KnockDown()
	{
		isKicked = false;
		yield return new WaitForSeconds(3f);
		if (Health > 0)
		{
			GetUp = true;
			rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
			color = new Color32(255, 255, 255, 0);
			rend.material.color = color;
			StartCoroutine(WakingUp());
        }
	}

	IEnumerator WakingUp()
    {
		yield return new WaitForSeconds(3f);
		GetUp = false;
		rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		WakeUp();
		ResetColliders();
		isDamaged = false;
		invincible = true;
		StartCoroutine(InvincibilityFrame());
	}
	IEnumerator FinalDeath()
	{
		yield return new WaitForSeconds(3f);
		rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		GetUp = true;
		yield return new WaitForSeconds(2f);
		GetUp = false;
		rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		yield return new WaitForSeconds(2f);
		var particle = Instantiate(particleEffect, rootJoint.transform.position, rootJoint.transform.rotation);
		particle.Play();
		spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(gameObject);
		Destroy(gameObject);
		yield return new WaitForSeconds(1.2f);
		Destroy(particle);
	}

	IEnumerator InvincibilityFrame()
	{
		if (invincible)
		{
			color = new Color32(0, 0, 108, 0);
			rend.material.color = color;
			yield return new WaitForSeconds(0.5f);
			color = new Color32(255, 255, 255, 0);
			rend.material.color = color;
			invincible = false;
		}
		else
        {
			if (!invincible)
			{
				color = new Color32(108, 108, 108, 0);
				rend.material.color = color;
				yield return new WaitForSeconds(0.5f);
				color = new Color32(255, 255, 255, 0);
				rend.material.color = color;
				invincible = false;
			}
		}		
	}
}

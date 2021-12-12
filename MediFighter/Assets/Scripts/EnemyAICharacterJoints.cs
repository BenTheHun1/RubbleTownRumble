using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAICharacterJoints : MonoBehaviour
{
	public ParticleSystem particleEffect;
	public ParticleSystem bloodEffect;
	public Vector3 lastPos;
	public GameObject rootJoint;
	public Rigidbody rootRigid;
	public CapsuleCollider rootCapCollide;
	public Rigidbody[] rigids;
	public CapsuleCollider[] capColliders;
	public BoxCollider[] boxColliders;
	public Animator animEnemy;
	public Renderer rend;
	public int Health;
	public float movementSpeed;
	public bool isRagdoll;
	public bool isKicked;
	public bool isAttacking;
	public bool isWalking;
	public bool GetUp;
	public bool invincible;
	public bool skipDeathStruggle;
	private Quaternion qTo;
	private GameObject player;
	private GameObject spawnManager;
	private float lookSpeed = 2.0f;
	private float stoppingradius = 1.7f;
	private Color32 color;
	private HealthSystem hs;

	void Start()
	{
		Health = 5;
		hs = GameObject.Find("Player").GetComponent<HealthSystem>();
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
		if (Vector3.Distance(player.transform.position, transform.position) > stoppingradius && !isRagdoll && !animEnemy.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
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
			if (!isRagdoll)
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

	void ResetColliders()
	{
		if (rootRigid == null)
        {
			rootRigid = gameObject.AddComponent<Rigidbody>();
			rootRigid.constraints = RigidbodyConstraints.FreezeRotation;
		}
		rootCapCollide = gameObject.AddComponent<CapsuleCollider>();
		rootCapCollide.center = new Vector3(0, 3, 0);
		rootCapCollide.direction = 1;
		rootCapCollide.radius = 1.4f;
		rootCapCollide.height = 6.6f;
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
		StartCoroutine(KnockDown());
	}
	public void Slashed()
    {
		Health -= hs.AttackAmount;
		if (Health <= 0)
		{
			rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			GetUp = false;
			Ragdoll();
			StartCoroutine(FinalDeath());
		}
		else
        {
			if (Health > 0)
			{
				color = new Color32(255, 0, 0, 0);
				rend.material.color = color;
				StartCoroutine(InvincibilityFrame());
			}
		}
	}

	IEnumerator KnockDown()
	{
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
					if (bc != null && bc.gameObject.name != "RootJoint")
					{
						bc.enabled = false;
					}
				}
				animEnemy.enabled = true;
				lastPos = rootJoint.transform.position;
				transform.position = lastPos;
				rootJoint.transform.position = lastPos;
				ResetColliders();
				invincible = false;
			}
		}
	}
	IEnumerator FinalDeath()
	{
		var bloodParticle = Instantiate(bloodEffect, rootJoint.transform.position, rootJoint.transform.rotation);
		bloodParticle.Play();
		yield return new WaitForSeconds(3f);
		Destroy(bloodParticle);
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
		hs.beards++;
		Destroy(gameObject);
		yield return new WaitForSeconds(1.2f);
		Destroy(particle);

	}

	IEnumerator InvincibilityFrame()
	{
		animEnemy.SetTrigger("Ouch");
		animEnemy.ResetTrigger("Walking");
		animEnemy.ResetTrigger("Attacking");
		var bloodParticle = Instantiate(bloodEffect, rootJoint.transform.position, rootJoint.transform.rotation);
		bloodParticle.Play();
		yield return new WaitForSeconds(0.1f);
		invincible = false;
		color = new Color32(255, 255, 255, 0);
		rend.material.color = color;
		invincible = false;
		animEnemy.ResetTrigger("Ouch");
		yield return new WaitForSeconds(0.74f);
		Destroy(bloodParticle);
	}
}

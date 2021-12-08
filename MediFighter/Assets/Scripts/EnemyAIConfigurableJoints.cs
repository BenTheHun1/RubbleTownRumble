using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIConfigurableJoints : MonoBehaviour
{
	public ParticleSystem particleEffect;
	public ConfigurableJoint[] XDrivejoints;
	public ConfigurableJoint[] YZDrivejoints;
	public ConfigurableJoint[] XYZMotions;
	public GameObject enemyObject;
	public GameObject rotationCorrection;
	public GameObject mimicker;
	public GameObject rootJoint;
	public Animator animEnemy;
	public Renderer rend;
	public int Health;
	public float movementSpeed;
	public bool roleModel;
	public bool isRagdoll;
	public bool isKicked;
	public bool isAttacking;
	public bool isWalking;
	public bool GetUp;
	public bool invincible;
	public bool skipDeathStruggle;
	private GameObject player;
	private GameObject spawnManager;
	private bool stunned;
	private float stoppingradius = 1f;
	private float driveValue;
	private ConfigurableJointMotion XYZMotionValue;
	private Color32 color;
	private HealthSystem hs;
	private CapsuleCollider playerSword;
	private PlayerController playerController;
	Quaternion initialRotation;
	ConfigurableJoint joint;

	void Awake()
	{
		joint = rootJoint.gameObject.GetComponent<ConfigurableJoint>();
		initialRotation = joint.transform.localRotation * Quaternion.Euler(0, -90, 0);
	}

	void Start()
	{
		if (transform.name != "RoleModel")
        {
			roleModel = false;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			GetComponent<Rigidbody>().isKinematic = false;
		}
		else
        {
			if (transform.name == "RoleModel")
			{
				roleModel = true;
				GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
				GetComponent<Rigidbody>().isKinematic = true;
			}
		}
		Health = 3;
		hs = GameObject.Find("Player").GetComponent<HealthSystem>();
		playerSword = GameObject.Find("Sword").GetComponent<CapsuleCollider>();
		playerController = GameObject.Find("Player").GetComponent<PlayerController>();
		mimicker = GameObject.Find("Mimic");
		animEnemy = mimicker.GetComponent<Animator>();
		XYZMotions = GetComponentsInChildren<ConfigurableJoint>();
		XDrivejoints = GetComponentsInChildren<ConfigurableJoint>();
		YZDrivejoints = GetComponentsInChildren<ConfigurableJoint>();
		player = GameObject.Find("Player");
		spawnManager = GameObject.Find("Spawns");
	}

	void Update()
	{
		if (Vector3.Distance(player.transform.position, transform.position) > stoppingradius && !invincible && !isRagdoll)
		{
			isWalking = true;
			isAttacking = false;
			animEnemy.SetTrigger("Walking");
			if (animEnemy.GetCurrentAnimatorStateInfo(0).IsName("Walk") && !stunned && !roleModel)
			{
				joint.SetTargetRotationLocal(rotationCorrection.transform.localRotation, initialRotation);
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
			spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(enemyObject);
			Destroy(gameObject);
		}

		if (GetUp)
		{
			Quaternion q = Quaternion.FromToRotation(rootJoint.transform.up, Vector3.up) * rootJoint.transform.rotation;
			rootJoint.transform.rotation = Quaternion.Slerp(rootJoint.transform.rotation, q, Time.deltaTime * 2f);
			rootJoint.transform.localPosition = Vector3.Slerp(rootJoint.transform.localPosition, new Vector3(rootJoint.transform.localPosition.x, rootJoint.transform.localPosition.y + 0.3f, rootJoint.transform.localPosition.z), Time.deltaTime * 2f);
		}
	}

	/*void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.CompareTag("Sword") && !invincible && !isRagdoll)
		{
			invincible = true;
			Ragdoll();
		}

		if (collision.gameObject.CompareTag("Boot") && !invincible && !isRagdoll)
		{
			isKicked = true;
			invincible = true;
			Ragdoll();
		}

	}*/

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Sword") && !invincible)
		{
			if (playerController.animSword.GetCurrentAnimatorStateInfo(0).IsName("Swipe") || playerController.animSword.GetCurrentAnimatorStateInfo(0).IsName("Swipe"))
			{
				playerSword.enabled = false;
				Slashed();
			}
		}
		else
		{
			if (other.gameObject.CompareTag("Boot") && !isRagdoll)
			{
				isKicked = true;
				Ragdoll();
			}
		}

		if (other.gameObject.CompareTag("Boot") && isRagdoll && Health <= 0)
		{
			skipDeathStruggle = true;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		}
		if (other.transform.root != transform.root && other.gameObject.CompareTag("EnemyRoot") && isRagdoll && isKicked == true)
		{
			other.gameObject.GetComponent<EnemyAICharacterJoints>().isKicked = true;
			other.gameObject.GetComponent<EnemyAICharacterJoints>().Ragdoll();
		}
	}

	/*void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<EnemyAIConfigurableJoints>().isKicked == true && other.gameObject.GetComponent<EnemyAIConfigurableJoints>().isRagdoll == true)
		{
			isDamaged = true;
			isKicked = true;
			Ragdoll();
		}
	}*/

	/*void Ragdoll()
	{
		//color = new Color32(255, 0, 0, 0);
		//rend.material.color = color;
		isRagdoll = true;
		StartCoroutine(Damage());
	}*/

	void Ragdoll()
	{
		XYZMotionValue = ConfigurableJointMotion.Locked;
		driveValue = 0f;
		isRagdoll = true;
		isAttacking = false;
		isWalking = false;
		if (Health <= 0)
		{
			color = new Color32(108, 0, 0, 0);
			rend.material.color = color;
		}
		ConfigurableJointModifier();
		StartCoroutine(KnockDown());
	}

	void Slashed()
	{
		Health -= hs.AttackAmount;
		if (Health <= 0)
		{
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

	void ConfigurableJointModifier()
	{
		foreach (ConfigurableJoint joint in XYZMotions)
		{
			if (joint.name != gameObject.name)
			{
				//joint.angularXMotion = XYZMotionValue;
				joint.angularYMotion = XYZMotionValue;
				joint.angularZMotion = XYZMotionValue;
			}
		}
		foreach (ConfigurableJoint joint in XDrivejoints)
		{
			JointDrive jointDrive = joint.angularXDrive;
			jointDrive.positionSpring = driveValue;
			joint.angularXDrive = jointDrive;
		}
		foreach (ConfigurableJoint joint in YZDrivejoints)
		{
			JointDrive jointDrive = joint.angularXDrive;
			jointDrive.positionSpring = driveValue;
			joint.angularYZDrive = jointDrive;
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
			rootJoint.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
			isRagdoll = false;
			invincible = false;
			driveValue = 800f;
			XYZMotionValue = ConfigurableJointMotion.Free;
			ConfigurableJointModifier();
			StartCoroutine(Stunned());
		}
	}

	IEnumerator FinalDeath()
	{
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
		hs.beards++;
		Destroy(gameObject);
		yield return new WaitForSeconds(1.2f);
		Destroy(particle);
	}

	IEnumerator InvincibilityFrame()
	{
		invincible = true;
		yield return new WaitForSeconds(0.75f);
		color = new Color32(255, 255, 255, 0);
		rend.material.color = color;
		invincible = false;
	}
	IEnumerator Stunned()
    {
		stunned = true;
		yield return new WaitForSeconds(4f);
		stunned = false;
	}
}

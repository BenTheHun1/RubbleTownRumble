using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public GameObject rotationCorrection;
	public GameObject mimicker;
	public Animator animEnemy;
	public ConfigurableJoint[] XDrivejoints;
	public ConfigurableJoint[] YZDrivejoints;
	public ConfigurableJoint[] XYZMotions;
	public GameObject rootJoint;
	public BoxCollider hitBox;
	public float movementSpeed;
	public bool isDamaged;
	public GameObject enemyObject;
	public Renderer rend;
	private Rigidbody rb;
	private GameObject player;
	private GameObject spawnManager;
	private float stoppingradius = 1f;
	private float driveValue;
	private ConfigurableJointMotion XYZMotionValue;
	private Color32 color;
	Quaternion initialRotation;
	ConfigurableJoint joint;

	void Awake()
	{
		joint = rootJoint.gameObject.GetComponent<ConfigurableJoint>();
		initialRotation = joint.transform.localRotation * Quaternion.Euler(0, -90, 0);
	}

	void Start()
	{
		mimicker = GameObject.Find("Mimic");
		animEnemy = mimicker.GetComponent<Animator>();
		XYZMotions = GetComponentsInChildren<ConfigurableJoint>();
		XDrivejoints = GetComponentsInChildren<ConfigurableJoint>();
		YZDrivejoints = GetComponentsInChildren<ConfigurableJoint>();
		player = GameObject.Find("Player");
		spawnManager = GameObject.Find("Spawns");
		rb = gameObject.GetComponent<Rigidbody>();
	}

	void Update()
	{
		if (Vector3.Distance(player.transform.position, transform.position) > stoppingradius && !isDamaged)
		{
			hitBox.enabled = false;
			animEnemy.SetTrigger("Walking");
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
			joint.SetTargetRotationLocal(rotationCorrection.transform.localRotation, initialRotation);
		}
		else
		{
			hitBox.enabled = true;
			animEnemy.SetTrigger("Attacking");
		}

		if (gameObject.transform.position.y < -25)
		{
			spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(enemyObject);
			Destroy(gameObject);
			Debug.Log("AAAAAAAAAAAAAAAAAAAA");
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Sword") && !isDamaged)
		{
			isDamaged = true;
			Ragdoll();
		}

		if (collision.gameObject.CompareTag("Boot") && !isDamaged)
		{
			isDamaged = true;
			Ragdoll();
		}

	}

	void Ragdoll()
	{
		color = new Color32(108, 0, 0, 0);
		rend.material.color = color;
		rb.freezeRotation = false;
		XYZMotionValue = ConfigurableJointMotion.Locked;
		driveValue = 0f;
		ConfigurableJointModifier();
		StartCoroutine(Damage());
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

	IEnumerator Damage()
	{
		color = new Color32(108, 108, 108, 0);
		rend.material.color = color;
		yield return new WaitForSeconds(6f);
		color = new Color32(225, 255, 255, 0);
		rend.material.color = color;
		XYZMotionValue = ConfigurableJointMotion.Free;
		driveValue = 800f;
		ConfigurableJointModifier();
		StartCoroutine(InvincibilityFrame());
	}

	IEnumerator InvincibilityFrame()
    {
		yield return new WaitForSeconds(4f);
		isDamaged = false;
	}
}

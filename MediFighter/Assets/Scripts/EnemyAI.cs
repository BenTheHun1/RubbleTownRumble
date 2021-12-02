using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public ConfigurableJoint[] XDrivejoints;
	public ConfigurableJoint[] YZDrivejoints;
	public Rigidbody[] rigids;
	public GameObject rootJoint;
	public GameObject mimicRootJoint;
	public float movementSpeed;
	public bool isDamaged;
	public GameObject enemyObject;
	public Renderer rend;
	private Quaternion qTo;
	private Rigidbody rb;
	private ConfigurableJoint cj;
	private GameObject player;
	private GameObject spawnManager;
	//private float detectionradius	= 10f;
	private float stoppingradius = 1.1f;
	private float lookSpeed = 2.0f;
	private float driveValue;
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
		rigids = GetComponentsInChildren<Rigidbody>();
		XDrivejoints = GetComponentsInChildren<ConfigurableJoint>();
		YZDrivejoints = GetComponentsInChildren<ConfigurableJoint>();
		player = GameObject.Find("Player");
		spawnManager = GameObject.Find("Spawns");
		qTo = rootJoint.gameObject.transform.rotation;
		rb = gameObject.GetComponent<Rigidbody>();
	}

	void Update()
	{
		//if	(Vector3.Distance(player.transform.position, transform.position) <=	detectionradius) //&&	!isDamaged)
		//{

		Vector3 lookDirectionPlayer = (player.transform.position - transform.position).normalized;
		lookDirectionPlayer.y = 0;
		mimicRootJoint.transform.localRotation = Quaternion.Slerp(mimicRootJoint.transform.localRotation, Quaternion.LookRotation(lookDirectionPlayer), Time.deltaTime * lookSpeed);

		//Vector3 lookDirection = (player.transform.position - transform.position).normalized;
		//Vector3 lookDirection = (rootJoint.transform.position - player.transform.position).normalized;
		//lookDirection.y = 0;
		//qTo = Quaternion.LookRotation(lookDirection);
		if (Vector3.Distance(player.transform.position, transform.position) > stoppingradius && !isDamaged)
		{
			//rootJoint.gameObject.transform.localPosition = new Vector3(0, 0, 0);
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
			joint.SetTargetRotationLocal(mimicRootJoint.transform.localRotation, initialRotation);
			//transform.rotation = Quaternion.Slerp(transform.rotation,	qTo, Time.deltaTime	* lookSpeed);
		}
		//}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Sword") && !isDamaged)
		{
			isDamaged = true;
			//Debug.Log("Damaged");
			Ragdoll();
			//StartCoroutine(Damage());
		}
	}

	void Ragdoll()
	{
		spawnManager.GetComponent<SpawnManager>().enemyAmount.Remove(enemyObject);
		color = new Color32(108, 0, 0, 0);
		rend.material.color = color;
		rb.freezeRotation = false;
		//this.enabled = false;
		driveValue = 0f;
		ConfigurableJointModifier();
		/*foreach (Rigidbody rb	in rigids)
		{
			rb.isKinematic = false;
		}*/
		StartCoroutine(Damage());
	}

	void ConfigurableJointModifier()
    {
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

	/*IEnumerator Remove()
	{
	yield return new WaitForSeconds(6f);
	Destroy(enemyObject);
	}*/

	IEnumerator Damage()
	{
		color = new Color32(108, 0, 0, 0);
		rend.material.color = color;
		yield return new WaitForSeconds(6f);
		color = new Color32(225, 255, 255, 0);
		rend.material.color = color;
		isDamaged = false;
		//this.enabled = true;
		driveValue = 800f;
		ConfigurableJointModifier();
	}

}

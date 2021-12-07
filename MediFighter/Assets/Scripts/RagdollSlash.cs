using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSlash : MonoBehaviour
{

    public EnemyAICharacterJoints enemyAI;

    // Start is called before the first frame update
    void Start()
    {
        enemyAI = transform.root.GetComponent<EnemyAICharacterJoints>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slashing()
	{
            enemyAI.Slashed();
    }
}

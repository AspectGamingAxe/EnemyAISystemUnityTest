using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//PUT THIS ON THE ENEMY

public class EnemyAI : MonoBehaviour
{
    [Header("Basics Walking")]
	[SerializeField] Transform Nextpos;
    public NavMeshAgent Agent;//Agent on the enemy Ref
    public Transform Player; //Player Ref
    public float smoothness; //how smooth the npc turns
    [SerializeField] Vector3 rotationdir;
    

    [Header("Distance Between Player")]
    public float distancebeforemove; //set the distance of how far the player can get before the NPC moves closer
	[SerializeField] float distancetoplayer;
    [SerializeField] bool distancebool;

	[Header("NewSpots")]
	public Manager manager;





	// Start is called before the first frame update
	void Start()
    {
        Agent.updateRotation = false;
    
        distancebool = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Looking At Player Script
		rotationdir = Player.position - transform.position;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotationdir), smoothness);
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
			PickNewSpot();
		}

        //Picking New Spot Incase Current Spot Is No Longer Valid
		if (!manager.ValidSpots.Contains(manager.currentspot) )
        {
            PickNewSpot();
        }
        
        

        //New Pos Function
        void PickNewSpot()
        {
            Nextpos = manager.FindNewSpot();

            if (Nextpos != null)
            {
                Agent.SetDestination(Nextpos.position);
            }
        }


        //checks if the player is too far away from the enemy, and if so moves closer
        distancetoplayer = Vector3.Distance(transform.position, Player.transform.position);

        if (distancetoplayer > distancebeforemove && distancebool == true && Agent.remainingDistance <= Agent.stoppingDistance)
        {
            
            PickNewSpot();
            StartCoroutine(DistanceToFix());
        }


        //quick way i could think of to fix a bug.
        IEnumerator DistanceToFix()
        {
            if (distancebool == true)
            {
                distancebool = false;
                yield return new WaitForSeconds(2f);
                distancebool = true;
            }
        }



	}
}

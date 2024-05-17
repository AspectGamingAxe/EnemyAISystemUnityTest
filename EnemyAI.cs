using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//PUT THIS ON THE ENEMY

public class EnemyAI : MonoBehaviour
{
    [Header("Basics Walking")]
    [SerializeField] Transform Nextpos; //Holds the position of the position that the Enemy will move to.
    public NavMeshAgent Agent; //Agent on the enemy Ref
    public Transform Player; //Player Ref
    public float smoothness; //how smooth the npc turns
    [SerializeField] Vector3 rotationdir; //Holds the Vector3 Difference, so the direction from the enemy to the player can be found.
    

    [Header("Distance Between Player")]
    public float distancebeforemove; //set the distance of how far the player can get before the NPC moves closer
    [SerializeField] float distancetoplayer; //Holds the distance of the player
    [SerializeField] bool distancebool;//I use this bool to fix a bug where an if statement would be called every second, so i used a coroutine to set this to false and after a while set to true

    [Header("NewSpots")]
    public Manager manager; //Refrence to the manager script





	// Start is called before the first frame update
	void Start()
    {
        Agent.updateRotation = false; //This makes it so the Navmesh Agent Rotaiton isnt controlled by the path it takes, this is so the enemy is always facing the player even if it is moving.
        distancebool = true; //I set this to true so that the IEnumerator works.
    }

    // Update is called once per frame
    void Update()
    {
        //Looking At Player Script
	rotationdir = Player.position - transform.position; //Standered way to find the direction from one object to another by anothre by subtracting their Vector3 Positions using the rotationdir variable.
 
	transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotationdir), smoothness); //Updates the rotation of the Enemy to the
        
	//manually make the enemy move
        if (Input.GetKeyDown(KeyCode.Space))
        {
		PickNewSpot();//Runs the PickNewSpot Function.
	}

        //Picking New Spot Incase Current Spot Is No Longer Valid
	if (!manager.ValidSpots.Contains(manager.currentspot))
        {
            PickNewSpot();//Runs the PickNewSpot Function.
        }
        
        

        //New Pos Function
        void PickNewSpot()
        {
            Nextpos = manager.FindNewSpot(); //Sets the NexPos variable as the return value from the FineNewSpot function in the manager script.

            if (Nextpos != null)//Makes sure that the NextPos has a value attachted to it, this is so I dont get any "Variable is null" errors;.
            {
                Agent.SetDestination(Nextpos.position); //Makes the enemy move to the Nextpos variable.
            }
        }


        //checks if the player is too far away from the enemy, and if so moves closer
	
        distancetoplayer = Vector3.Distance(transform.position, Player.transform.position);//gets the distance from the player to the enemy and saving that as a variable.

        if (distancetoplayer > distancebeforemove)//checks if the distance to the player is greater than the distance before move                                
        {
            if(distancebool == true)//checks if the distancebool is true so it doesnt get called every frame
	    {
     		if(Agent.remainingDistance <= Agent.stoppingDistance)//checks if the enemy has finished moving to its destination, so it doesnt keep trying to move to a new location even though it's currently moving to one
       		{
	 		PickNewSpot(); //Runs the PickNewSpot Function.
            		StartCoroutine(DistanceToFix());//Runs the DistanceToFix Coroutine
		}
       
	    }
          
        }


        //quick way i could think of to fix a bug, that stops the if statement
        IEnumerator DistanceToFix()
        {
            if (distancebool == true)//checks if i distance is set to true, and only if it is true, will the code run under it
            {
                distancebool = false;
                yield return new WaitForSeconds(2f); //Waits 2 seconds before continuing the code
                distancebool = true;
            }
        }



	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;


//PUT THIS ON EMPTY GAME OBJECTS YOU WANT TO USE AS THE SPOTS


public class HidingSpots : MonoBehaviour
{



	[Header("Valid Check")]
	[SerializeField] float RayDistance = 2f; //This changes the distance of the raycast
	[SerializeField] bool inrange; //Checks if the player is within range of the sphereoverlay
	[SerializeField] bool inrangecd = true; //This is created for an TurnOffRadius Ienumerator so it is not called every frame
	public float sphereradius = 10f; //This changes the radius of the sphereoverlay
	[SerializeField] RaycastHit? lasthit; //This stores the gameobject that is currently being hit if it is cover
	[SerializeField] RaycastHit hit; ///This stores the gameobject that is currently being hit
	public LayerMask CoverMask; // LayerMask of your cover, for the Raycast.

	[Header("Refrences")]
	public Transform playerloc; //Refrence to the player location
	public Manager manager; //Refrence to the Manager Script
	public GameObject Player; //Refrence to the players gameobject
	
	
	public LayerMask playermask; //Layermask for the player
	
	[SerializeField] bool Valid = false; //This will let the enemyAI know if this hiding spot is valid or not
	

	// Start is called before the first frame update
	void Start()
    	{
		inrangecd = true; //This is just so the Ienumerator can run when it is called for the first time.

	}

    // checks to see if the player is within the hiding spots overlapshpere.
    void Update()
	{
		Collider[] sphereinrange = Physics.OverlapSphere(this.gameObject.transform.position, sphereradius, playermask); //Creatse a sphereoverlay that only registers items that have the Layermask that playermask is set to.
		
		foreach (Collider sphereingranges in sphereinrange) //gets each item within the overlapsphere that has the playermask layer on it and runs the code below.
		{

			if (sphereingranges.CompareTag("Player")) //Just a double check to see if its the player, Makes sure you create a tag called "Player" and add it to the player.
			{
					inrange = true;
					StartCoroutine(TurnOffInRadius()); //Starts the TurnOffInRadius Coroutine.
			}
		}

        transform.LookAt(playerloc); //Looks at the player


		StartCoroutine(Raycast()); //runs the raycast function to return a hit value;


		//checks to see if the raycast has a hit and if the player is withing the overlap sphere, then calls the true ray function, if false runs the falseray function.

		if (lasthit != null) //Just to make sure that last hit has a value attachted to it, this is to stop "Variable is null" errors and to check that the raycast actually hit cover
		{
			if (Valid != true)//Checks if valid is not true, this is so the code isnt infinitly repeated over and over again.
			{
				if (inrange)
				{
					StartCoroutine(TrueRay());// Starts the TrueRay Coroutine
				}

				else
				{
					StartCoroutine(FalseRay());// Starts the FalseRay Coroutine
				}
			}

			
		}
		else
		{
			StartCoroutine(FalseRay()); // Starts the FalseRay Coroutine since lasthit is equal to null
		}

		if (!inrange) //this is just to check if at anytime the player leaves the spherecast that it makes the spot invalid
		{
			StartCoroutine(FalseRay());// Starts the FalseRay Coroutine
		}

		
	}

    IEnumerator Raycast() // Runs a raycast and either returns a hit value, or null.
    {
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, RayDistance, CoverMask)) //Runs a raycast that is only able to hit the CoverMask layermask
		{
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * RayDistance, Color.yellow);//This is just to see the lines of the raycast

				lasthit = hit;	//If the raycast does hit cover then saves its value to lasthit
		}
		else
		{	
				lasthit = null;	//if the raycast doesnt hit cover then it makes lasthit value null
		}
		yield return new WaitForSeconds(0.1f);//This is just so the code isnt run every frame
	}

	IEnumerator TrueRay() //if the conditions are right, it adds the hiding spots to the validspot list.
	{

		if (!manager.ValidSpots.Contains(this))//This is just to check that this spot isnt already set to Valid
		{
			manager.UpdateStatusToTrue(this); //Runs the UpdateStatusToTrue function in manager and runs this gameobject as a parameter
			Valid = true; 
			Debug.Log("Hitting Cover"); //Just so i can check if the code actually works
			yield return new WaitForSeconds(5f);//This is so it doesn't run every frame
		}
		
		
	}
	IEnumerator FalseRay() //if the conditions are not right, it removes the hiding spots to the validspot list.
	{
		if (manager.ValidSpots.Contains(this))//This is just to check that this spot isnt already set to Invalid
		{
			manager.UpdateStatusToFalse(this);//Runs the UpdateStatusToFalse function in manager and runs this gameobject as a parameter
			Valid = false;
			Debug.Log("Hitting Not");//Just so i can check if the code actually works
			yield return new WaitForSeconds(5f);//This is so it doesn't run every frame
			
		}
		
	}
	//This is just to fix a bug i found.
	IEnumerator TurnOffInRadius()
	{
		if (inrangecd)// Checks to see if inrangecd is set to true, and so it doesnt get run every frame.
		{
			inrangecd = false;
			yield return new WaitForSeconds(2f);//Waits 2 seconds
			inrange = false;
			inrangecd = true;
		}
		
	}
	//This is just so the shphereoverlay is visable
	private void OnDrawGizmos()    
	{
		Gizmos.color = Color.green;
		Gizmos.color *= 0.1f;
		Gizmos.DrawSphere(transform.position, sphereradius);

		
	}
}

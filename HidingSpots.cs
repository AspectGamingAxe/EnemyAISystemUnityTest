using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;


//PUT THIS ON EMPTY GAME OBJECTS YOU WANT TO USE AS THE SPOTS


public class HidingSpots : MonoBehaviour
{



	[Header("Valid Check")]
	[SerializeField] float RayDistance = 2f; 
	[SerializeField] bool inrange;
	[SerializeField] bool inrangecd = true;
	public float sphereradius = 10f;
	[SerializeField] RaycastHit? lasthit;
	[SerializeField] RaycastHit hit;
	public LayerMask CoverMask;

	[Header("Refrences")]
	public Transform playerloc;
	public Manager manager;
	public GameObject Player;
	
	
	public LayerMask playermask;
	
	[SerializeField] bool Valid = false;
	

	// Start is called before the first frame update
	void Start()
    {
		inrangecd = true;

	}

    // checks to see if the player is within the hiding spots overlapshpere.
    void Update()
	{
		Collider[] sphereinrange = Physics.OverlapSphere(this.gameObject.transform.position, sphereradius, playermask);
		
		foreach (Collider sphereingranges in sphereinrange)
		{

		
			if (sphereingranges.CompareTag("Player"))
			{
					inrange = true;
					StartCoroutine(TurnOffInRadius());
			}
		}

        transform.LookAt(playerloc); //Looks at the player


		StartCoroutine(Raycast()); //runs the raycast function to return a hit value;


		//checks to see if the raycast has a hit and if the player is withing the overlap sphere, then calls the true ray function, if false runs the falseray function.

		if (lasthit != null)
		{
			if (Valid != true)
			{
				if (inrange)
				{
					StartCoroutine(TrueRay());
				}

				else
				{
					StartCoroutine(FalseRay());
				}
			}

			
		}
		else
		{
			StartCoroutine(FalseRay());
		}

		if (!inrange)
		{
			StartCoroutine(FalseRay());
		}

		
	}

    IEnumerator Raycast() // Runs a raycast and either returns a hit value, or null.
    {
		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, RayDistance, CoverMask))
		{
			Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * RayDistance, Color.yellow);

				lasthit = hit;	
		}
		else
		{	
				lasthit = null;	
		}
		yield return new WaitForSeconds(0.1f);
	}

	IEnumerator TrueRay() //if the conditions are right, it adds the hiding spots to the validspot list.
	{

		if (!manager.ValidSpots.Contains(this))
		{
			manager.UpdateStatusToTrue(this);
			Valid = true;
			Debug.Log("Hitting Cover");
			yield return new WaitForSeconds(5f);
		}
		
		
	}
	IEnumerator FalseRay() //if the conditions are not right, it removes the hiding spots to the validspot list.
	{
		if (manager.ValidSpots.Contains(this))
		{
			manager.UpdateStatusToFalse(this);
			Valid = false;
			Debug.Log("Hitting Not");
			yield return new WaitForSeconds(5f);
			
		}
		
	}

	IEnumerator TurnOffInRadius()
	{
		if (inrangecd)
		{
			inrangecd = false;
			yield return new WaitForSeconds(2f);
			inrange = false;
			inrangecd = true;
		}
		
	}

	private void OnDrawGizmos()    
	{
		Gizmos.color = Color.green;
		Gizmos.color *= 0.1f;
		Gizmos.DrawSphere(transform.position, sphereradius);

		
	}
}

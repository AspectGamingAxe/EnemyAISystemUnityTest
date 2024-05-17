using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//PUT THIS ON AN EMPTY GAME OBJECT


public class Manager : MonoBehaviour
{

	public List<HidingSpots> ValidSpots = new List<HidingSpots>();//Creates a list for the valid hiding spots
	public List<HidingSpots> Hidingspots = new List<HidingSpots>();//Creates a list for the all hiding  spots
	[SerializeField] int index; //This will be used to pick a random spot out of the valid spots
	[SerializeField] Transform togotoposition; //The position that the player would go to.
	public HidingSpots currentspot; //The current hiding spot that is selected
	public EnemyAI enemyAI; //enemy reference
	

	// Start is called before the first frame update
	private void Awake()
	{
		Hidingspots.Clear();//This is just to make sure that the Hiding spot list is clear before i start adding to the list to avoid bugs.
	}

	void Start()
    	{
        Hidingspots.Add(FindAnyObjectByType<HidingSpots>()); //This gets every gameobject with the script hidingspots and adds it to the hidingspots list
	}

	

	// Update is called once per frame
	public void UpdateStatusToTrue(HidingSpots hidinspots)//This function will let you passthrough a hidingspots gameobject as a parameter 
    	{
		ValidSpots.Add(hidinspots);//adds the gameobject that was a parameter to the validspots list
    	}

	public void UpdateStatusToFalse(HidingSpots hidinspots)//This function will let you passthrough a hidingspots gameobject as a parameter 
	{
		ValidSpots.Remove(hidinspots);//removes the gameobject that was a parameter from the valid spots list
	}


	public Transform FindNewSpot()//This is a transform function that will let me return a transform
	{
		index = Random.Range(0, ValidSpots.Count);//gets a random number from 0 to the amount of valid spots there are, and saves that as a variable
		if(index > 0)//this is just to check that there are any valid spots were chosen to avoid errors.
		{
            HidingSpots randomlocation = ValidSpots[index]; //creates a temporary variable that holds the hiding spot within the validspots list corespodant to the Index variable
            currentspot = randomlocation;//saves the random location to a variable
            return randomlocation.gameObject.transform;//this returns the value of the randomlocations transform, to where ever the function was called.
            
            
        }
		else
		{
			return null;// if there are no valid spots that were chosen it will return null, this is also to avoid bugs.
		}
		
		
	}
}

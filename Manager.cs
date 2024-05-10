using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//PUT THIS ON AN EMPTY GAME OBJECT


public class Manager : MonoBehaviour
{

	public List<HidingSpots> ValidSpots = new List<HidingSpots>();
	public List<HidingSpots> Hidingspots = new List<HidingSpots>();
	[SerializeField] int index;
	[SerializeField] Transform togotoposition;
	public HidingSpots currentspot;
	public EnemyAI enemyAI; //enemy reference
	

	// Start is called before the first frame update
	private void Awake()
	{
		Hidingspots.Clear();
	}

	void Start()
    {
        Hidingspots.Add(FindAnyObjectByType<HidingSpots>());
	}

	

	// Update is called once per frame
	public void UpdateStatusToTrue(HidingSpots hidinspots)
    {
		ValidSpots.Add(hidinspots);
    }

	public void UpdateStatusToFalse(HidingSpots hidinspots)
	{
		ValidSpots.Remove(hidinspots);
	}


	public Transform FindNewSpot()
	{
		index = Random.Range(0, ValidSpots.Count);
		if(index > 0)
		{
            HidingSpots randomlocation = ValidSpots[index];
            currentspot = randomlocation;
            return randomlocation.gameObject.transform;
            
            
        }
		else
		{
			return null;
		}
		
		
	}
}

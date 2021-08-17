// Original source: Unify Community Wiki - https://wiki.unity3d.com/index.php/DontGoThroughThings
//
// This script uses raycasting to avoid the physics engine letting fast-moving objects go through other objects (particularly meshes).
// Used to counteract clipping through walls /platforms during jump
// NOTE: This script was modified by me for use in a 2D environment.

using UnityEngine;
using System.Collections;

public class DontGoThroughThings : MonoBehaviour
{
	// Careful when setting this to true - it might cause double
	// events to be fired - but it won't pass through the trigger
	public bool sendTriggerMessage = false; 	

	public LayerMask layerMask = -1; //make sure we aren't in this layer 
	public float skinWidth = 0.1f; //probably doesn't need to be changed -- higher skinWidth => stop more collisions

	private float minimumExtent; 
	private float partialExtent; 
	private float sqrMinimumExtent; 
	private Vector2 previousPosition; 
	private Rigidbody2D myRigidbody;
	private Collider2D myCollider;

	//initialize values 
	void Start() 
	{ 
		myRigidbody = GetComponent<Rigidbody2D>();
		myCollider = GetComponent<Collider2D>();
		previousPosition = myRigidbody.position; 
		minimumExtent = Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y) / 3; // for some reason /3 for scaling reasons
		partialExtent = minimumExtent * (1.0f - skinWidth); 
		sqrMinimumExtent = minimumExtent * minimumExtent; 
	} 

	void FixedUpdate() 
	{ 
		//have we moved more than our minimum extent? 
		Vector2 movementThisStep = (myRigidbody.position - previousPosition); 
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;
		
		if (movementSqrMagnitude > sqrMinimumExtent) 
		{ 
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
			RaycastHit2D hitInfo = Physics2D.Raycast(previousPosition, movementThisStep, movementMagnitude, layerMask.value); 
			//print(hitInfo.collider);
			
			//check for obstructions we might have missed 
			if (hitInfo)
				{
					if (!hitInfo.collider)
						return;

					if (hitInfo.collider.isTrigger) 
						hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);

					if (!hitInfo.collider.isTrigger)
						myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * partialExtent; 
						myRigidbody.velocity = Vector2.zero; // stop on impact
				}
		} 

		previousPosition = myRigidbody.position; 
	}
}
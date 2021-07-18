using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curry.Util
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Collider2D))]
	//Taken from wiki with some mods for 2D: http://wiki.unity3d.com/index.php/DontGoThroughThings
	// Used to stop fast moving objects from travelling through barriers
	public class PhaseThroughPreventer : MonoBehaviour
	{
		[SerializeField] Rigidbody2D myRigidbody = default;
		[SerializeField] Collider2D myCollider = default;

		LayerMask layerMask = -1; //make sure we aren't in this layer 
		static float skinWidth = 0.1f; //probably doesn't need to be changed 
		float minimumExtent;
		float partialExtent;
		float sqrMinimumExtent;
		Vector2 previousPosition;


		//initialize values 
		void Start()
		{
			previousPosition = myRigidbody.position;
			minimumExtent = Mathf.Min(Mathf.Min(myCollider.bounds.extents.x, myCollider.bounds.extents.y), myCollider.bounds.extents.z);
			partialExtent = minimumExtent * (1.0f - skinWidth);
			sqrMinimumExtent = minimumExtent * minimumExtent;
		}

		void FixedUpdate()
		{
			//have we moved more than our minimum extent? 
			Vector2 movementThisStep = myRigidbody.position - previousPosition;
			float movementSqrMagnitude = movementThisStep.sqrMagnitude;

			if (movementSqrMagnitude > sqrMinimumExtent)
			{
				float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
				RaycastHit hitInfo;

				//check for obstructions we might have missed 
				if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value))
				{
					if (!hitInfo.collider)
						return;

					if (hitInfo.collider.isTrigger)
						hitInfo.collider.SendMessage("OnTriggerEnter", myCollider);

					if (!hitInfo.collider.isTrigger) 
					{
						Vector2 hitPos = hitInfo.point;
						myRigidbody.position = hitPos - (movementThisStep / movementMagnitude) * partialExtent;
					}

				}
			}

			previousPosition = myRigidbody.position;
		}
	}
}

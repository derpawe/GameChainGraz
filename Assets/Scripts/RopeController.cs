using UnityEngine;
using System.Collections;

public class RopeController : MonoBehaviour {
	void FixedUpdate() {
		Transform jumper = GameObject.Find("JumperCharacter").transform;
		Transform climber = GameObject.Find("ClimberCharacter").transform;
		float distance = (jumper.position - climber.position).magnitude;

		float radius = distance * distance / transform.childCount * 0.6f;
		radius = Mathf.Min(0.5f, Mathf.Max(0.1f, radius));

		Transform prevChild = null;
		foreach (Transform child in transform) {
			child.GetComponent<CircleCollider2D>().radius = radius;
			if (prevChild != null) {
				// break rope
				if ((child.position - prevChild.position).magnitude > 2.0) {
					child.GetComponent<HingeJoint2D>().enabled = false;
					climber.GetComponent<DistanceJoint2D>().enabled = false;
					// TODO loose game

					// reset chain
					foreach (Transform innerChild in transform) {
						innerChild.GetComponent<CircleCollider2D>().radius = 0.1f;
					}
					return;
				}
			}
			prevChild = child;
		}

		// hochzieh-hack by valentin
		/*if (climber.position.y > jumper.position.y) {
			climber.GetComponent<Rigidbody2D>().mass = 40f;
		} else {
			climber.GetComponent<Rigidbody2D>().mass = 0.01f;
			if (distance > 13) {
				// push climber up (in case due to current physics this won't happen)
				climber.rigidbody2D.AddForce(new Vector2(0, distance * 0.005f));
				climber.Translate(new Vector2(0, distance * 0.005f));
			}
		}*/
	}
}

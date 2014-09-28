using UnityEngine;
using System.Collections;

public class ClimableController : MonoBehaviour {
	LayerMask whatIsClimbable;
	ClimberCharacterController controller;

	void Awake() {
		controller = GetComponentInParent<ClimberCharacterController>();
		whatIsClimbable = controller.whatIsClimbable;
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if ((1 << col.gameObject.layer & whatIsClimbable.value) != 0) {
			controller.walled = true;
		}
	}
	
	
	void OnTriggerExit2D(Collider2D col)
	{
		if ((1 << col.gameObject.layer & whatIsClimbable.value) != 0) {
			controller.walled = false;
		}
	}
}

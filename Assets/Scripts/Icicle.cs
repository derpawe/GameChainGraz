using UnityEngine;
using System.Collections;

public class Icicle : MonoBehaviour {
	bool killedAlreadyACharacter = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Jumper")) {
			GameObject.Find("JumperCharacter").GetComponent<JumperCharacterController>().isAlreadyDead = true;
			killedAlreadyACharacter = true;
		} else if (col.gameObject.layer == LayerMask.NameToLayer("Climber")) {
			killedAlreadyACharacter = true;
			GameObject.Find("ClimberCharacter").GetComponent<ClimberCharacterController>().isAlreadyDead = true;
		} else if ((col.gameObject.layer == LayerMask.NameToLayer("Rope")) && (killedAlreadyACharacter == false)) {
			col.transform.GetComponent<HingeJoint2D>().enabled = false;
			GameObject.Find("JumperCharacter").GetComponent<JumperCharacterController>().isAlreadyDead = true;
			GameObject.Find("ClimberCharacter").GetComponent<ClimberCharacterController>().isAlreadyDead = true;
		}
	}
}

using UnityEngine;
using System.Collections;

public class Icicle : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if ((col.gameObject.layer == LayerMask.NameToLayer("Jumper"))
		    || (col.gameObject.layer == LayerMask.NameToLayer("Climber"))) {
			Debug.Log("Die"); // TODO die
		} else if (col.gameObject.layer == LayerMask.NameToLayer("Rope")) {
			col.transform.GetComponent<HingeJoint2D>().enabled = false;
			// TODO die
		}
	}
}

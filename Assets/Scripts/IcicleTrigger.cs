using UnityEngine;
using System.Collections;

public class IcicleTrigger : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D col)
	{
		if ((col.gameObject.layer == LayerMask.NameToLayer("Jumper"))
		  || (col.gameObject.layer == LayerMask.NameToLayer("Climber"))) {
			transform.parent.Find("Icicle").GetComponent<Rigidbody2D>().isKinematic = false;
		}
	}
}

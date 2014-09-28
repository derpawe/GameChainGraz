using UnityEngine;
using System.Collections;

public class FlagTouched : MonoBehaviour {

	private bool levelEnded = false;

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log("something touched the flag..");

		if(!levelEnded && col.gameObject.tag.Equals("Player"))
			FallDown();
	}

	void FallDown()
	{
		float yStartingValue = transform.position.y;
		var move = new Vector3(0, -0.02f, 0);

		while (yStartingValue - transform.position.y < 1.5f)
						transform.position += move * Time.deltaTime;

		levelEnded = true;
		Application.LoadLevel("Level1");

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

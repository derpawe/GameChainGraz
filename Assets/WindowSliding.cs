using UnityEngine;
using System.Collections;

public class WindowSliding : MonoBehaviour {

	public float speed = 1.0f;
	public float leftEnd = -15.0f;
	public float rightEnd = 15.0f;
	public bool leftToRight = true;

	// Update is called once per frame
	void Update () {
		var move = new Vector3(leftToRight ? 1 : -1, 0, 0);

		if (transform.position.x > rightEnd || transform.position.x < leftEnd) {
			transform.position = new Vector3(leftToRight ? leftEnd : rightEnd, transform.position.y, 0);
		}

		transform.position += move * speed * Time.deltaTime;
	}
}

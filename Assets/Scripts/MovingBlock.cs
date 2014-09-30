using UnityEngine;
using System.Collections;

public class MovingBlock : MonoBehaviour {
	public Transform left;
	public Transform right;
	public bool directionIsLeft = true;
	public float speed = 0.3f;

	// Update is called once per frame
	void Update() {
		// turn around
		Bounds b = transform.GetComponent<BoxCollider2D>().bounds;
		float topPos = transform.position.y + b.size.y / 2;
		float leftPos = transform.position.x - b.size.x / 2;
		float bottomPos = transform.position.y - b.size.y / 2;
		float rightPos = transform.position.x + b.size.x / 2;

		if (directionIsLeft && PointInRect(topPos, leftPos, bottomPos, rightPos, left.position)) {
			directionIsLeft = false;
		} else if ((!directionIsLeft) && PointInRect(topPos, leftPos, bottomPos, rightPos, right.position)) {
			directionIsLeft = true;
		}

		if (new Rect(-10,10,20,20).Contains(new Vector2(0,0)))
			Debug.Log("Inside");

		// move
		Vector3 currentSpeed = (directionIsLeft ? 1 : -1) * (left.position - right.position).normalized * speed * Time.deltaTime;
		transform.position += currentSpeed;

		// move player characters too
		Vector2 topLeft = new Vector2(leftPos, topPos);
		Vector2 bottomRight = new Vector2(rightPos, bottomPos);
		if (Physics2D.OverlapArea(topLeft, bottomRight, LayerMask.GetMask("Jumper"))) {
			GameObject.Find("MainLevel/Characters/JumperCharacter").transform.position += currentSpeed;
		}
		if (Physics2D.OverlapArea(topLeft, bottomRight, LayerMask.GetMask("Climber"))) {
			GameObject.Find("MainLevel/Characters/ClimberCharacter").transform.position += currentSpeed;
		}

	}

	bool PointInRect(float top, float left, float bottom, float right, Vector2 p) {
		return ((left < p.x) && (p.x < right) && (bottom < p.y) && (p.y < top));
	}
}

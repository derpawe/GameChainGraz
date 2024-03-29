﻿using UnityEngine;

public class ClimberCharacterController : MonoBehaviour 
{
	bool facingRight = false;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.	
	[SerializeField] float airSpeed = 2f;				// The fastest the player can travel in air.
	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
    public LayerMask whatIsClimbable;			// A mask determining what is ground to the character

	int grounded = 0;								// Whether or not the player is grounded.
	Animator anim;										// Reference to the player's animator component.
    public bool walled = false;
    //CharacterController controller;

	int fallToDeathLimit = 7;
	Vector3 lastGroundedPosition;
	public bool isAlreadyDead = false;

    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
        //controller = GetComponent<CharacterController>();
	}
	
	void FixedUpdate()
	{
		if (grounded > 0) {
			if (!isAlreadyDead) {
				if (walled)
				{
					anim.SetInteger("state", 2);
				}
				else if (rigidbody2D.velocity.x > 0) // moving
				{
					anim.SetInteger("state", 1);
				}
				else
				{
					anim.SetInteger("state", 0);
				}
				lastGroundedPosition = transform.position;
			} else {
				foreach (var circleColliderObj in gameObject.GetComponents<CircleCollider2D>()) {
					circleColliderObj.enabled = false;
				}
				anim.SetInteger("state", 5);
			}
		} else if (grounded <= 0 || !walled) {
			if((lastGroundedPosition.y - transform.position.y) > fallToDeathLimit && !isAlreadyDead)
			{
				anim.SetInteger("state", 4);
				isAlreadyDead = true;
			}
		}

	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if ((1 << col.gameObject.layer & whatIsGround.value) != 0) {
			grounded = grounded + 1;
		}
	}
	
	
	void OnTriggerExit2D(Collider2D col)
	{
		if ((1 << col.gameObject.layer & whatIsGround.value) != 0) {
			grounded = grounded -1;
		}
	}

	public void Move(float move, float climbing)
	{
		if (isAlreadyDead)
			return;

		Vector2 newVelocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y);

		//only control the player if grounded or airControl is turned on
		if (grounded > 0 || airControl) {
			newVelocity.x = move * (grounded > 0 ? maxSpeed : airSpeed) * (walled ? 0.1f : 1f);

			if (move > 0 && !facingRight) {
				Flip(); // If the input is moving the player right and the player is facing left...

				if (walled) {
					// jump from wall
					walled = false;
					rigidbody2D.transform.Translate(0.5f, 0, 0);
					newVelocity.x *= 10f;
					anim.SetInteger("state", 0);
				}
			} else if (move < 0 && facingRight) {
				Flip(); // Otherwise if the input is moving the player left and the player is facing right...

				if (walled) {
					// jump from wall
					walled = false;
					rigidbody2D.transform.Translate(-0.5f, 0, 0);
					newVelocity.x *= 10f;
					anim.SetInteger("state", 0);
				}
			}
		}
        
        if (walled)
        {
            newVelocity.y = climbing * 7f;
            if (climbing != 0 && !audio.isPlaying)
            {
                audio.Play();
            }
        }

		rigidbody2D.velocity = newVelocity;
	}

	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}

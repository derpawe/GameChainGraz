﻿using UnityEngine;

public class ClimberCharacterController : MonoBehaviour 
{
	bool facingRight = false;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.	
	[SerializeField] float airSpeed = 2f;				// The fastest the player can travel in air.
	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
    [SerializeField] LayerMask whatIsClimbable;			// A mask determining what is ground to the character

	bool grounded = false;								// Whether or not the player is grounded.
	Animator anim;										// Reference to the player's animator component.
    bool walled = false;
    CharacterController controller;

    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
	}
	
	void FixedUpdate()
	{
		float charRadius = collider2D.bounds.size.x / 2 + 1f;
		Vector3 charCenter = new Vector3(collider2D.bounds.center.x, collider2D.bounds.center.y + 0.3f, 0);
		walled = Physics2D.OverlapCircle(charCenter, charRadius, whatIsClimbable);

		if (grounded)
		{
			anim.SetInteger("state", 0);
		}
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if ((1 << col.gameObject.layer & whatIsGround.value) != 0) {
			grounded = true;
		}
	}
	
	
	void OnTriggerExit2D(Collider2D col)
	{
		if ((1 << col.gameObject.layer & whatIsGround.value) != 0) {
			grounded = false;
		}
	}

	public void Move(float move, float climbing)
	{
		//only control the player if grounded or airControl is turned on
		if (grounded || airControl) {
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight) {
				Flip ();// ... flip the player.
					// Otherwise if the input is moving the player left and the player is facing right...
			} else if (move < 0 && facingRight) {
				Flip ();// ... flip the player.
			}

			rigidbody2D.velocity = new Vector2 (move * (grounded ? maxSpeed : airSpeed), rigidbody2D.velocity.y);

			if (move != 0 && grounded && !walled) {
				anim.SetInteger ("state", 1);
				//Debug.Log("1");
			}
		}
        
        if (walled)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, climbing * 7f);
            anim.SetInteger("state", 2);
            //Debug.Log("2");
        }

        if (grounded && !walled && (move == 0))
        {
            anim.SetInteger("state", 0);
            //Debug.Log("0");
        }
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

using UnityEngine;

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
    CharacterController controller;

	int fallToDeathLimit = 7;
	Vector3 lastGroundedPosition;
	public bool isAlreadyDead = false;

    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
	}
	
	void FixedUpdate()
	{
		if (grounded > 0) {
			if (!isAlreadyDead) {
				if (walled)
				{
					anim.SetInteger("state", 2);
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
		} else if (grounded <= 0 || walled <= 0) {
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

		//only control the player if grounded or airControl is turned on
		if (grounded > 0|| airControl) {
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight) {
				Flip ();// ... flip the player.
					// Otherwise if the input is moving the player left and the player is facing right...
			} else if (move < 0 && facingRight) {
				Flip ();// ... flip the player.
			}

			rigidbody2D.velocity = new Vector2 (move * (grounded > 0 ? maxSpeed : airSpeed), rigidbody2D.velocity.y);

			if (move != 0 && grounded > 0 && !walled) {
				anim.SetInteger ("state", 1);
				//Debug.Log("1");
			}
		}
        
        if (walled)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, climbing * 7f);
            if (climbing != 0 && !audio.isPlaying)
            {
                audio.Play();
            }
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

using UnityEngine;

public class JumperCharacterController : MonoBehaviour 
{
	bool facingRight = false;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float airSpeed = 2f;				// The fastest the player can travel in air.
	[SerializeField] float jumpForce = 1500f;			// Amount of force added when the player jumps.	

	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character

	bool grounded = false;								// Whether or not the player is grounded.
	Animator anim;										// Reference to the player's animator component.
	Vector2 groundColliderPoint;
	float groundColliderRadius;


    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();

		// set groundCollider a little bit lower and smaller than physics-circleCollider
		CircleCollider2D collider = transform.GetComponent<CircleCollider2D>();
		groundColliderRadius = collider.radius * 0.95f;
		groundColliderPoint = new Vector2(collider.center.x, collider.center.y - 0.1f);
	}


	void FixedUpdate()
	{
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

	public void Move(float move, bool crouch, bool jump)
	{
		//only control the player if grounded or airControl is turned on
		if(grounded || airControl)
		{

			// Move the character
			rigidbody2D.velocity = new Vector2(move * (grounded ? maxSpeed : airSpeed), rigidbody2D.velocity.y);
            if (move != 0 && grounded)
            {
            	anim.SetInteger("state", 1);
            }
            
			
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}

        // If the player should jump...
        if (grounded && jump) {
            // Add a vertical force to the player.
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
        if (!grounded)
        {
            anim.SetInteger("state", 2);
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

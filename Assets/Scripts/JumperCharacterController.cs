using UnityEngine;

public class JumperCharacterController : MonoBehaviour 
{
	bool facingRight = false;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float airSpeed = 2f;				// The fastest the player can travel in air.
	[SerializeField] float jumpForce = 1500f;			// Amount of force added when the player jumps.	

	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character

	int grounded = 0;								// Whether or not the player is grounded.
	Animator anim;										// Reference to the player's animator component.

	int fallToDeathLimit = 7;
	Vector3 lastGroundedPosition;
	public bool isAlreadyDead = false;

    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
		lastGroundedPosition = transform.position;
	}


	void FixedUpdate()
	{
        if (grounded > 0)
        {
			if(!isAlreadyDead)
			{
				anim.SetInteger("state", 0);
				lastGroundedPosition = transform.position;
			} else
			{
				foreach (var circleColliderObj in gameObject.GetComponents<CircleCollider2D>()) {
					circleColliderObj.enabled = false;
				}
				anim.SetInteger("state", 5);
			}
        } else if (grounded <=  0) {
			if((lastGroundedPosition.y - transform.position.y) > fallToDeathLimit)
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
		if((1 << col.gameObject.layer & whatIsGround.value) != 0) {
			grounded = grounded - 1;
		}
	}

	public void Move(float move, bool crouch, bool jump)
	{
		if (isAlreadyDead)
			return;

		//only control the player if grounded or airControl is turned on
		if(grounded > 0 || airControl)
			{
				
			// Move the character
			rigidbody2D.velocity = new Vector2(move * (grounded>0 ? maxSpeed : airSpeed), rigidbody2D.velocity.y);
			if (move != 0 && grounded > 0)
			{
				anim.SetInteger("state", 1);//walk anim
			}
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight){
				Flip();
			} else if(move < 0 && facingRight){
				Flip();
			}
		}
		
		// The player can jump if grounded
		if (grounded > 0 && jump) {
			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            audio.Play();
		}
		if (grounded <= 0 )
		{
			anim.SetInteger("state", 2);// air animation
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

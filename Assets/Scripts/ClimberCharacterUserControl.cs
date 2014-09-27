using UnityEngine;

[RequireComponent(typeof(ClimberCharacterController))]
public class ClimberCharacterUserControl : MonoBehaviour 
{
    private ClimberCharacterController character;
    private bool climb;


	void Awake()
	{
        character = GetComponent<ClimberCharacterController>();
	}

    void Update ()
    {
        // Read the jump input in Update so button presses aren't missed.
#if CROSS_PLATFORM_INPUT
        if (CrossPlatformInput.GetButtonDown("Jump")) jump = true;
#else
		if (Input.GetButtonDown("Jump")) climb = true;
#endif

    }

	void FixedUpdate()
	{
		// Read the inputs.
		#if CROSS_PLATFORM_INPUT
		float h = CrossPlatformInput.GetAxis("Horizontal");
		#else
		float h = Input.GetAxis("Horizontal2");
		#endif

        float v = Input.GetAxis("Vertical2");

		// Pass all parameters to the character control script.
		character.Move( h,  v );

        // Reset the jump input once it has been used.
	    climb = false;
	}
}

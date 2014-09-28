using UnityEngine;
using System.Collections;

public class DyingScript : MonoBehaviour {

	// Update is called once per frame
    public AudioClip dying;

	void Update () {

        if (GameObject.Find("JumperCharacter").GetComponent<JumperCharacterController>().isAlreadyDead && GameObject.Find("ClimberCharacter").GetComponent<ClimberCharacterController>().isAlreadyDead)
        {
            audio.PlayOneShot(dying);
        }
        
	}
}

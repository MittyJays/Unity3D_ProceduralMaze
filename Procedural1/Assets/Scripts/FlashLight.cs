using UnityEngine;

public class FlashLight : MonoBehaviour {
	public KeyCode flashLightKey;
	public AudioClip flashLightSound;

	void Update ()
	{
		if (Input.GetKeyUp (flashLightKey)) 
		{
			GetComponent <Light> ().enabled = !GetComponent <Light> ().enabled;
			GetComponent <AudioSource> ().PlayOneShot (flashLightSound);
		}
	}
}



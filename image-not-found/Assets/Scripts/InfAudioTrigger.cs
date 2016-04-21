using UnityEngine;
using System.Collections;

public class InfAudioTrigger : MonoBehaviour {

	public GameObject[] finishTheseToStart; // these triggers need to be finished before this trigger may start
	public bool enableWhenPlayerIsInRange = false; // useful for non-looping sounds
	public GameObject[] finishTheseToEnd; // these need to be finished before this one can
	public GameObject[] finishOneOfTheseToEnd; // one these need to be finished before this one can
	public bool finishWhenSoundIsHeard = false; // useful for non-looping sounds
	public bool finishWhenPlayerIsInCloseRange = false; // for luring looping sounds
	public GameObject[] finishOneOfTheseToCleanup; // to clean up if it will never be played

	public GameObject disableWhenThisOneFinished;
	public bool debugNeverEnd = false;

	private GameObject player;
	private float playerCollisionRadius = 0.25f; // hard coded for now
	private float playerCloseRangeRadius = 0.3f; // hard coded for now
	private AudioSource audioSource;
	private bool finished = false;
	private bool enabled = false;
	private bool disabled = false;

	// Use this for initialization
	void Start () {
	
		player =  GameObject.FindWithTag("Player");
		audioSource = GetComponentsInChildren<AudioSource> ()[0];
/*

Een AudioTrigger (AT) is een Spatial AudioSource

Een AT heeft een voorwaarde om te starten ( playing )
	-	Eventueel een aantal andere AT's die geëindigd zijn
	-	Eventueel binnen bereik

Een AT heeft een voorwaarde om te eindigen ( fading out )
  	-	Eventueel een aantal andere AT's die geëindigd zijn
  	-	Eventueel de player heeft het item gehoord / kunnen horen.
 
 */
	}

	bool CanBeEnabled () {
		bool can = true;
	
		if ( finishTheseToStart.Length > 0 ) {
			foreach ( GameObject audioTrigger in finishTheseToStart ) {
				if ( ! audioTrigger.GetComponent<InfAudioTrigger>().IsFinished() ) {
					can = false;
				}
			}
		}

		if ( enableWhenPlayerIsInRange ) {
			
			float playerDistance = Vector3.Distance (transform.position, player.transform.position) - playerCollisionRadius;
			if ( playerDistance > audioSource.maxDistance ) {
				can = false;
			}
		}

		return can;
	}

	bool CanFinish () {
		bool can = true;

		if ( finishWhenSoundIsHeard ) {
			if ( audioSource.isPlaying ) {
				can = false;

			/* 
				Indien geluid gehoord moet zijn en speler is al weg voordat
				geluid geëindigd is, dan resetten?
				Of fake ik the positie van de playert? ;)
			 */
			}
		}

		if ( finishOneOfTheseToEnd.Length > 0 ) {
			bool oneFinished = false;
			foreach ( GameObject audioTrigger in finishOneOfTheseToEnd ) {
				if ( audioTrigger.GetComponent<InfAudioTrigger>().IsFinished() ) {
					oneFinished = true;
				}
			}
			can = oneFinished;
		}

		if ( finishTheseToEnd.Length > 0 ) {
			foreach ( GameObject audioTrigger in finishTheseToEnd ) {
				if ( ! audioTrigger.GetComponent<InfAudioTrigger>().IsFinished() ) {
					can = false;
				}
			}
		}

		if ( finishWhenPlayerIsInCloseRange ) {
			float playerDistance = Vector3.Distance (transform.position, player.transform.position) - playerCollisionRadius;
			if ( playerDistance > playerCloseRangeRadius ) {
				can = false;
			}			
		}

		if ( debugNeverEnd ) {
			can = false;
		}

		return can;	
	}

	bool IsEnabled () {
		return enabled;
	}

	bool IsFinished () {
		return finished;
	}

	// Update is called once per frame
	void Update () {
	
		if ( disableWhenThisOneFinished != null ) {
			if ( disableWhenThisOneFinished.GetComponent<InfAudioTrigger>().IsFinished() ) {
				disabled = true;
			}
		}

		if (!disabled) {
			
			if (IsFinished ()) {

				if (audioSource.volume != 0.0f) {
					// TODO: fade out
					audioSource.Stop ();
					audioSource.volume = 0.0f;
				}

			} else if (IsEnabled () && !IsFinished ()) {

				if (CanFinish ()) {
					Debug.Log ("going to finish " + name);
					finished = true;
				}

			} else if (!IsEnabled ()) {
				if (CanBeEnabled ()) {
					Debug.Log ("going to enable" + name);				
					enabled = true;
					audioSource.time = 0.0f;
					audioSource.Play ();
				}
			}

			// it might never have played but needs to be done with
			if (!IsFinished () && !IsEnabled () && finishOneOfTheseToCleanup.Length > 0) {
				bool oneFinished = false;
				foreach (GameObject audioTrigger in finishOneOfTheseToCleanup) {
					if (audioTrigger.GetComponent<InfAudioTrigger> ().IsFinished ()) {
						oneFinished = true;
					}
				}	
				if (oneFinished) {
					finished = true;
				}
			}
		}
	}
}

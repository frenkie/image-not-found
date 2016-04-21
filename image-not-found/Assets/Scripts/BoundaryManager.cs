using UnityEngine;
using System.Collections;

public class BoundaryManager : MonoBehaviour {

	public GameObject boundaryTarget; // the object whose bounds are watched
	public GameObject boundaryOrigin; // the object whose position is matched against bounds
	public AudioClip boundarySound;

	private AudioSource boundaryAudioSource;
	private float maxOutOfBoundsDistance=0.5f;

	void Start () {
		boundaryAudioSource = boundaryOrigin.AddComponent<AudioSource> ();
		boundaryAudioSource.clip = boundarySound;
		boundaryAudioSource.loop = true;
	}
	

	void Update () {
		Bounds targetBounds = boundaryTarget.GetComponent<Renderer> ().bounds; 
		Vector3 originPosition = boundaryOrigin.transform.position;

		// force the origin's y position in the bounds
		originPosition = new Vector3 ( originPosition.x, targetBounds.center.y, originPosition.z );

		if (!targetBounds.Contains ( originPosition )) {
			
			float distance = targetBounds.SqrDistance ( originPosition );
			float perc = distance / maxOutOfBoundsDistance;

			if ( perc > 1.0f ) {
				perc = 1.0f;
			}
			Debug.Log ("out of bounds: "+ distance );
			boundaryAudioSource.volume = perc;
			boundaryAudioSource.Play ();

		} else {
			Debug.Log ("In bounds");
			boundaryAudioSource.Stop();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class InfGameHandler : MonoBehaviour {

	private SocketIOComponent socket;
	private bool connected=false;
	private float fYRot=0.05f;
	public float fYOffset=0.0f;

	public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("error", TestError);
		socket.On("close", TestClose);
		socket.On("yrotation", OnYRotation);
		socket.On("bla", TestBla);
	}

	public void OnYRotation (SocketIOEvent e) {
		Debug.Log ("[SocketIO] yrotation received: " + e.name + " " + e.data);
		if (e.data != null && e.data.GetField("rotation") != null ) {
			fYRot = float.Parse ( e.data.GetField("rotation").str ) + fYOffset;
		}
	}

	public void TestBla( SocketIOEvent e) {
		Debug.Log ("[SocketIO] testbla received: " + e.name + " " + e.data);
	}

	public void TestOpen(SocketIOEvent e)
	{
		connected = true;
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void TestError(SocketIOEvent e)
	{
		connected = false;
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}

	public void TestClose(SocketIOEvent e)
	{	
		connected = true;
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}

	public void Update () {
		if ( connected ) {
			// This script operates on the GameObject it is attached to
			transform.rotation = Quaternion.AngleAxis(fYRot, Vector3.up);
		}
		if (Input.GetMouseButton (0)) {			
			RaycastHit hit = new RaycastHit();
			if(Physics.Raycast( Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000)) {				
				transform.position = new Vector3 ( hit.point.x, transform.position.y, hit.point.z );
			}
		}
	}
}

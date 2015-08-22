using UnityEngine;
using System.Collections;

public class SmoothCameraFollow : MonoBehaviour {

	Camera mainCamera;
	public float cameraSpeed;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		mainCamera.transform.position = Vector3.MoveTowards (mainCamera.transform.position, 
		                                              transform.position, cameraSpeed * Time.deltaTime);
		mainCamera.transform.position = new Vector3 (mainCamera.transform.position.x,
		                                            mainCamera.transform.position.y,
		                                            -10f);
	}
}
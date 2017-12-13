using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Quick script to make the name panel look at the camera all the time
/// </summary>
public class UI_Player_Name_Panel : MonoBehaviour 
{

	Camera mainCamera;

	// Use this for initialization
	void Start () 
	{

		mainCamera = Camera.main;
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		gameObject.transform.LookAt (mainCamera.transform);
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour 
{

	IRC irc;

	// Use this for initialization
	void Start () 
	{

		irc = GameObject.Find ("IRCManager").GetComponent<IRC> ();
		
	}

	void Update()
	{

		GetCommand ();
		
	}

	void GetCommand ()
	{
		
		if (irc.cmdToExecute.Count > 0) 
		{

			lock (irc.cmdToExecute) 
			{

				string cmdUserName = irc.cmdToExecute.Peek ().userName;
				string cmdUserMSG = irc.cmdToExecute.Peek ().userMSG;

				switch (cmdUserMSG) 
				{

				case "&join":
					{

						Debug.Log (cmdUserName + " Has Joined The Game");
						irc.cmdToExecute.Dequeue ();

						GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
						cube.transform.position = new Vector3 (0, 0, 0);
						cube.transform.name = cmdUserName;

						break;
						
					}
					

				}

			}
			
		}

	}
}

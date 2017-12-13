using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CommandManager : MonoBehaviour 
{
	//Scripts
	IRC irc;
    _PlayerController pController;
	PlayerProfiles profiles;

	//GameObjects
	GameObject playerPrefab;
	GameObject spawnPoint;

	//Variables
	public string cmdUserName = string.Empty;
	public string cmdUserMSG = string.Empty;

	// Use this for initialization
	void Start () 
	{
		irc = GameObject.Find ("IRCManager").GetComponent<IRC> ();
		playerPrefab = Resources.Load("PlayerModels/Knight") as GameObject;
		spawnPoint = GameObject.Find ("SpawnPoint");
		profiles = GameObject.Find ("PlayerManager").GetComponent<PlayerProfiles> ();
    }

	void Update()
	{

		SortTheCommands ();
		
	}

	//Function to sort the commands and run the correct function
	//In my case this will check for players name and apply specific command to that player.
	void SortTheCommands ()
	{
		
		if (irc.cmdToExecute.Count > 0) 
		{

			lock (irc.cmdToExecute) 
			{
				//Get Our UserName and Message
				cmdUserName = irc.cmdToExecute.Peek ().userName;
				cmdUserMSG = irc.cmdToExecute.Peek ().userMSG;

				switch (cmdUserMSG) 
				{
                    //Actions/Command State machine
                    case "&stop":
                        {
							
							PlayerStop ();
                            break;

                        }

                    case "&f":
                        {
						
							MovePlayerForward ();
                            break;

                        }

                    case "&b":
                        {
						
							MovePlayerBack ();
                            break;

                        }

                    case "&r":
                        {
						
							MovePlayerRight ();
                            break;

                        }

                    case "&l":
                        {
						
							MovePlayerLeft();
                            break;

                        }

					case "&msg":
						{
							irc.IRCSendWhisper ("My User Name is: " + cmdUserName + "My msg was: " + cmdUserMSG, cmdUserName);
							break;

						}

                        //Instantiate the player with their name
					case "&join":
						{

							CreatePlayer ();
							break;

						}
                }
			}

            irc.cmdToExecute.Dequeue();

        }

	}

	////////////////////
	///Player Movement
	////////////////////
	///This will find the correct player and apply the state change to it
	void MovePlayerForward()
	{

		pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
		pController.StateForward();
		
	}

	void MovePlayerBack()
	{

		pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
		pController.StateBack();

	}

	void MovePlayerLeft()
	{

		pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
		pController.StateLeft();

	}

	void MovePlayerRight()
	{

		pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
		pController.StateRight();

	}

	void PlayerStop()
	{

		pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
		pController.StateIdle();

	}

	////////////////////
	/// Creates the player.
	////////////////////
	void CreatePlayer()
	{

		if (!profiles.playerProfiles.Any (x => x.playerName.Contains (cmdUserName))) {
			//Create our profile if it does not exist
			profiles.CreatePlayerProfile (cmdUserName);

			//Spawn our player, set name and color
			GameObject Player = Instantiate (playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
			Player.name = cmdUserName;
			Player.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = cmdUserName;
			Player.GetComponent<MeshRenderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
		} else
			irc.IRCSendWhisper ("You have already Joined! " , cmdUserName);
		
	}
}
	
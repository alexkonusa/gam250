using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommandManager : MonoBehaviour 
{

	IRC irc;
	PlayerProfiles _playerProfiles;
	GameStateManager gameStateManager;
    _PlayerController pController;
	GameObject playerPrefab;
	GameObject spawnPoint;

	// Use this for initialization
	void Start () 
	{
		irc = GameObject.Find ("IRCManager").GetComponent<IRC> ();
		playerPrefab = Resources.Load("PlayerModels/Knight") as GameObject;
		spawnPoint = GameObject.Find ("SpawnPoint");

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
				string cmdUserName = irc.cmdToExecute.Peek ().userName;
				string cmdUserMSG = irc.cmdToExecute.Peek ().userMSG;

				switch (cmdUserMSG) 
				{
                    //Actions/Command State machine
                    case "&stop":
                        {
							
							pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
                            pController.StateIdle();
                            break;

                        }

                    case "&f":
                        {
							pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
                            pController.StateForward();
                            break;

                        }

                    case "&b":
                        {
							pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
                            pController.StateBack();
                            break;

                        }

                    case "&r":
                        {
							pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
                            pController.StateRight();
                            break;

                        }

                    case "&l":
                        {
							pController = GameObject.Find(cmdUserName).GetComponent<_PlayerController>();
                            pController.StateLeft();
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

							GameObject Player = Instantiate (playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
							Player.name = cmdUserName;
							Player.GetComponent<MeshRenderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
							break;

						}
                }
			}

            irc.cmdToExecute.Dequeue();

        }

	}
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

/*		//When command &Join Knight is executed it will create a character profile in profiles with name, class and gameObject.
case "&Join Knight":
{
	if (gameStateManager.currentState == GameStateManager.GameState.Lobby && 
		!_playerProfiles.playerProfiles.Any(x => x.playerName.Contains(cmdUserName))) 
	{
		Debug.Log (cmdUserName + " Has Joined The Game");

		_playerProfiles.CreatePlayerProfile(cmdUserName,"Knight",
			Resources.Load ("PlayerModels/Knight") as GameObject, 100);

		irc.cmdToExecute.Dequeue ();
	}
	break;
}

//Create our Mage character
case "&Join Mage":
{
	if (gameStateManager.currentState == GameStateManager.GameState.Lobby && 
		!_playerProfiles.playerProfiles.Any(x => x.playerName.Contains(cmdUserName))) 
	{
		Debug.Log (cmdUserName + " Has Joined The Game");

		_playerProfiles.CreatePlayerProfile(cmdUserName,"Mage",
			Resources.Load ("PlayerModels/Mage") as GameObject, 100);

		irc.cmdToExecute.Dequeue ();
	}
	break;
}*/
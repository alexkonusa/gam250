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

	// Use this for initialization
	void Start () 
	{
		irc = GameObject.Find ("IRCManager").GetComponent<IRC> ();
		//_playerProfiles = GameObject.Find ("PlayerManager").GetComponent<PlayerProfiles> ();
		//gameStateManager = GameObject.Find ("GameManager").GetComponent<GameStateManager> ();
        pController = GameObject.Find("Player").GetComponent<_PlayerController>();

    }

	void Update()
	{

		SortTheCommands ();
		
	}

	//Class to sort the commands and execute them accoring to the action
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
                            pController.StateIdle();
                            break;

                        }

                    case "&f":
                        {
                            pController.StateForward();
                            break;

                        }

                    case "&b":
                        {
                            pController.StateBack();
                            break;

                        }

                    case "&r":
                        {
                            pController.StateRight();
                            break;

                        }

                    case "&l":
                        {
                            pController.StateLeft();
                            break;

                        }
					case "&msg":
					{
						irc.IRCSendWhisper ("My User Name is: " + cmdUserName + "My msg was: " + cmdUserMSG, cmdUserName);
						break;

					}
                }
			}

            irc.cmdToExecute.Dequeue();

        }

	}
}

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
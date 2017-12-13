using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfiles : MonoBehaviour 
{
	//We are storing all of our player profiles in a list
	public List<PLAYER_PROFILE_CLASS> playerProfiles = new List<PLAYER_PROFILE_CLASS>();

	//Create a profile for our player
	public void CreatePlayerProfile(string playerName)
	{

		playerProfiles.Add (new PLAYER_PROFILE_CLASS (playerName));
		
	}
}

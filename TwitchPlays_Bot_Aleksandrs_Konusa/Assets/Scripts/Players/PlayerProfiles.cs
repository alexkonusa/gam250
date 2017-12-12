using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfiles : MonoBehaviour 
{
	//List to Store all of our Current player profiles
	public List<PlayerProfile> playerProfiles = new List<PlayerProfile>();

	//Function to create our player and add them to the playerProfile
	public void CreatePlayerProfile(string playerName, string playerClass, GameObject playerModel, int gold)
	{
		PlayerProfile playerProfile = new PlayerProfile (playerName, playerClass, playerModel, gold);
		playerProfiles.Add (playerProfile);
	}
}

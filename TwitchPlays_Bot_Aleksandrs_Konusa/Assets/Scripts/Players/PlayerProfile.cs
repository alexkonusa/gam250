using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile  
{

	public string playerName;
	public string playerClass;
	public GameObject playerModel;
	public int gold;

	public PlayerProfile(string playerName, string playerClass, GameObject playerModel, int gold)
	{

		this.playerName = playerName;
		this.playerClass = playerClass;
		this.playerModel = playerModel;
		this.gold = gold;

	}

}

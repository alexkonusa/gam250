﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SetUpWindow : EditorWindow
{

	//This is the UI set up window 
    //Using this window we create our twitch IRC manager in the scene
	string twitchUserName;
	string twitchOAUTHKey;
	string twitchChannelName;

	//Our IRC manager GameObject
	GameObject ircManager;

	[MenuItem("Window/TwitchSetUp")]
	public static void ShowWindow()
	{

		EditorWindow.GetWindow (typeof(SetUpWindow));
		
	}

	void Awake()
	{
		ircManager = Resources.Load("IRC/IRCManager") as GameObject;
		
	}

	void OnGUI()
	{

		//Our input fields for information needed to connect to the twitch channel
		twitchUserName = EditorGUILayout.TextField ("Twitch User Name:", twitchUserName);
		twitchOAUTHKey = EditorGUILayout.TextField ("Twitch OAUTH Token:", twitchOAUTHKey);
		twitchChannelName = EditorGUILayout.TextField ("Twitch Channel Name:", twitchChannelName);

		//If our fields are not empty and there is no IRCmanager in the scene 
		//A button becomes avalible so that we can create our IRC manager.
		if (twitchUserName != string.Empty && twitchOAUTHKey != string.Empty
		    && twitchChannelName != string.Empty && GameObject.Find ("IRCManager") == null) 
		{
			if (GUILayout.Button ("Set UP")) {



				GameObject ircObj = Instantiate (ircManager);
				ircObj.name = "IRCManager";

				IRC_CONNECTION_INFO _IRC_CONNECTION_INFO = ircObj.GetComponent<IRC_CONNECTION_INFO> ();

				_IRC_CONNECTION_INFO.userName = twitchUserName;
				_IRC_CONNECTION_INFO.aouthToken = twitchOAUTHKey;
				_IRC_CONNECTION_INFO.channelName = twitchChannelName;

			}
		} else 
		{

			EditorGUILayout.LabelField ("IRCManager is already set up in this scene.");
			
		}

		//Display our GUI help labels
		EditorGUILayout.LabelField ("HELP");
		EditorGUILayout.LabelField ("Twitch User Name: Your Twitch Bot/Account username lower case.");
		EditorGUILayout.LabelField ("Twitch OAUTH Token: Can be generated here:");
		EditorGUILayout.LabelField ("https://twitchapps.com/tmi/");
		EditorGUILayout.LabelField ("Twitch Channel Name: Channel you want to connect too, lower case.");
	}
}

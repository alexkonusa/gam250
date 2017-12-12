using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SetUpWindow : EditorWindow
{

	//This is the set Up UI which created a prefab that creates a connection with twitch
	string twitchUserName;
	string twitchOAUTHKey;
	string twitchChannelName;

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

				IRC irc;

				GameObject ircObj = Instantiate (ircManager);
				ircObj.name = "IRCManager";
				irc = ircObj.GetComponent<IRC> ();

				irc.userName = twitchUserName;
				irc.aouthToken = twitchOAUTHKey;
				irc.channelName = twitchChannelName;


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

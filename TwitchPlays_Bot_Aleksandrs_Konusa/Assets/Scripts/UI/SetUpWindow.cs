using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SetUpWindow : EditorWindow
{

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

		ircManager = Resources.Load("/IRC/IRCManager") as GameObject;
		
	}

	void OnGUI()
	{

		twitchUserName = EditorGUILayout.TextField ("Twitch User Name:", twitchUserName);
		twitchOAUTHKey = EditorGUILayout.TextField ("Twitch UOAUTH Key:", twitchOAUTHKey);
		twitchChannelName = EditorGUILayout.TextField ("Twitch Channel Name:", twitchChannelName);

		if (GUILayout.Button ("Set UP"))
		{

			if (GameObject.Find ("IRCManager") != null) 
			{

				GameObject test = Instantiate (ircManager);
				test.name = "IRCManager";
			}

		}
	}
}

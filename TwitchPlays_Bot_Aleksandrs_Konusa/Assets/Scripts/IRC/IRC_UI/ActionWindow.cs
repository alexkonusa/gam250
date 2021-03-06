﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

//This is the Actions Window
//Here you can create commands so that the IRC can process them

[ExecuteInEditMode][System.Serializable]
public class ActionWindow : EditorWindow 
{

	IRC irc;
	public string newAction;
	object Button;
	//IRC_ACTION_CLASS _IRC_ACTION_CLASS;

	//Adding our action window to window in the editor
	[MenuItem("Window/IRCActions")]
	public static void ShowWindow()
	{

		EditorWindow.GetWindow (typeof(ActionWindow));
		
	}
		
	void OnGUI()
	{
		//If we can find IRC manager then we can draw our items and get the component we need
		if (GameObject.Find ("IRCManager") != null) 
		{
			if (irc == null)
				irc = GameObject.Find ("IRCManager").GetComponent<IRC> ();

			EditorGUILayout.LabelField ("[User Commands]");

			//Here we draw our actions, so for each item in our action list we 
			//draw a new label field
			EditorGUILayout.BeginVertical ("box");
			foreach (IRC_ACTION_CLASS s in irc.actions) {
				EditorGUILayout.LabelField (s.action);
			}

			EditorGUILayout.EndVertical ();

			newAction = EditorGUILayout.TextField ("Action", newAction);

			//In here we add our action when the button is clicked to the list
			if (GUILayout.Button ("ADD ACTION")) 
			{
				if (newAction != string.Empty) {
					irc.actions.Add (new IRC_ACTION_CLASS(newAction));
					newAction = string.Empty;
				}

			}

			//This button finds the action we want to remove and removes it from the action List

			if (GUILayout.Button ("REMOVE ACTION")) {
				if (newAction != string.Empty) 
				{
					irc.actions.Remove (irc.actions.Find(x => x.action == newAction));

					newAction = string.Empty;
				}

			}
		}
		
	}
}

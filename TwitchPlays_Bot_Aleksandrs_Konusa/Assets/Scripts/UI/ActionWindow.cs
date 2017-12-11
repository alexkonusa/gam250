using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ActionWindow : EditorWindow 
{

	IRC irc;

	string newAction;
	object Button;

	void Start()
	{

		irc = GameObject.Find ("IRCManager").GetComponent<IRC> ();
		
	}

	[MenuItem("Window/IRCActions")]
	public static void ShowWindow()
	{

		EditorWindow.GetWindow (typeof(ActionWindow));
		
	}

	void OnGUI()
	{
		irc = GameObject.Find ("IRCManager").GetComponent<IRC> ();
		newAction = EditorGUILayout.TextField ("Action", newAction);

		if (GUILayout.Button ("ADD ACTION"))
		{
			if (newAction != string.Empty) {
				
				irc.actions.Add (newAction);
			}

		}
		
	}
}

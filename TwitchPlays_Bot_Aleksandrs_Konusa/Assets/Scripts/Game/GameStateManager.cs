using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour 
{

	//State Machine
	public enum GameState
	{

		Lobby,
		Match,
		MatchFinish
		
	}

	public GameState currentState;

	//Count Down timer Variables
	public float timeLeft = 300f;
	public string minutes;
	public string seconds;

	Text countDownTXT;

	void Start()
	{

		currentState = GameState.Lobby;
		countDownTXT = GameObject.Find("CountDownPanel").GetComponentInChildren<Text> ();
		
	}

	void Update()
	{
		switch (currentState) 
		{
			
		case GameState.Lobby:
			{
				StartCountDown ();
				break;
			}

		case GameState.Match:
			{
				countDownTXT.gameObject.SetActive (true);
				StartCountDown ();
				break;
			}

		case GameState.MatchFinish:
			{
				break;
			}
		}

	}

	//Count Down Timer
	public void StartCountDown()
	{

			timeLeft -= Time.deltaTime;

			minutes = Mathf.Floor (timeLeft / 60).ToString ("00");
			seconds = (timeLeft % 60).ToString ("00");

			countDownTXT.text = minutes + ":" + seconds;
			//Debug.Log (minutes + ":" + seconds);

			if (timeLeft < 0) {
			countDownTXT.text = "00:00";
			currentState = GameState.Match;
			countDownTXT.gameObject.SetActive (false);

			timeLeft = 300f;
		}
	}
}


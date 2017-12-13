using UnityEngine;

//Requered information to create our connection with twitch
//[System.Serializable]
public class IRC_CONNECTION_INFO : MonoBehaviour
{
	[HideInInspector]
	public string ip = "irc.chat.twitch.tv";
	[HideInInspector]
	public int port = 6667;
	[HideInInspector]
	public string userName = "USER NAME";
	[HideInInspector]
	public string aouthToken = "OAUTH KEY";
	[HideInInspector]
	public string channelName = "CHANNEL NAME";

}

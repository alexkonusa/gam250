//Creating connection and managing the data from/to twitch. 
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class IRC : MonoBehaviour
{
	//Requered information to create our connection with twitch
	[SerializeField]
	private string ip = "IP";
	[SerializeField]
	private int port = 6667;
	[SerializeField]
	private string userName = "USER NAME";
	[SerializeField]
	private string aouthToken = "OAUTH KEY";
	[SerializeField]
	private string channelName = "CHANNEL NAME";

	//Other Variables
	[SerializeField]
	private int msgSendDelay = 1750;

	//STATUS CODES
	const string 
	JOIN_STATUS_CODE = "001",
	INVALID_CMD_CODE = "423",
	MSG_CODE = "PRIVMSG",
	SERVER_REPLY_CODE = "PING";

	//TCP and Threading
	private TcpClient tcpClient;
	private StreamReader streamReader;
	private StreamWriter streamWriter;
	private Thread inStream, outStream;

	bool stopThreading;

	//buffer for recieved messages
	private string buffer;

	//Lists and Queue's
	List<string> recievedMSGs = new List<string>();
	Queue<string> ircCmdQueue = new Queue<string>();


	//Use this for initialization
	void Start ()
	{

		IRCStart();

	}

	//Creating our tcp connection and threading for in and out. 
	void IRCStart()
	{

		tcpClient = new TcpClient(ip, port);

		if (!tcpClient.Connected)
		{ 
			Debug.Log ("Connection failed");
			return;
		} else 
			Debug.Log ("Connected");

		var networkStream = tcpClient.GetStream();
		streamReader = new StreamReader(networkStream);
		streamWriter = new StreamWriter(networkStream);

		streamWriter.WriteLine("PASS " + aouthToken);
		streamWriter.WriteLine("NICK " + userName.ToLower());
		streamWriter.WriteLine("USER " + userName + "8 * : " + userName);
		streamWriter.Flush();

		inStream = new Thread(() => IRCInput(streamReader, networkStream));
		inStream.Start();
		outStream = new Thread(() => IRCOutput(streamWriter));
		outStream.Start();

	}

	private void IRCInput(TextReader inStream, NetworkStream networkStream)
	{
		while (!stopThreading) {
			if (networkStream.DataAvailable) {

				buffer = inStream.ReadLine ();

				Debug.Log (buffer);

				string msg = buffer.Split (' ') [1];

				Debug.Log (msg);

				switch (msg) {

				case JOIN_STATUS_CODE:
					{
						IRCJoinChannel ();
						break;
					}

				case INVALID_CMD_CODE:
					{
						Debug.Log ("INVALID COMMAND");
						break;
					}

				case MSG_CODE:
					{
						Debug.Log (buffer + " I GOT ONE");
						IRCSendWhisper ("Channel Name: WhySo-Shy | Player Stats: Health = 100 Money = 5000 | Time Played: 1Hr", "whyso_shy");
						break;
					}

				case SERVER_REPLY_CODE:
					{
						IRCSendCommand ("PONG");
						break;
					}

				}
			}

		}
	}


	void IRCOutput(TextWriter outStream)
	{
		//Thread for sending mesagges to the channel! :) 
		Stopwatch stopWatch = new Stopwatch ();
		stopWatch.Start ();

		while (!stopThreading)
		{

			lock (ircCmdQueue) {

				if (ircCmdQueue.Count > 0) {

					if (stopWatch.ElapsedMilliseconds > msgSendDelay) {
						//sending our message to the server
						outStream.WriteLine (ircCmdQueue.Peek ());
						outStream.Flush ();

						//Remove the msg
						ircCmdQueue.Dequeue ();

						//Reset and Stop the stopwatch
						stopWatch.Reset ();
						stopWatch.Start ();

					}

				}

			}
		} 
	}

	public void IRCJoinChannel()
	{

		streamWriter.WriteLine("JOIN #" + channelName);
		streamWriter.Flush();

	}

	//Sending messages to the twitch channel
	public void IRCSendMessage(string msg)
	{

		lock (ircCmdQueue) 
		{

			ircCmdQueue.Enqueue ("PRIVMSG #" + channelName + " :" + msg + "\r\n");

		}

	}

	//Private Message Function
	public void IRCSendWhisper(string prvMsg, string destination)
	{

		lock (ircCmdQueue) 
		{

			ircCmdQueue.Enqueue ("PRIVMSG #" + channelName + " :" + "/w " + destination + " :" + prvMsg + "\r\n");

		}

	}

	//Sending Messages to IRC 
	public void IRCSendCommand(string cmd)
	{

		lock (ircCmdQueue) 
		{

			ircCmdQueue.Enqueue (cmd);

		}

	}


	//End off connection and threading
	private void OnApplicationQuit()
	{
		if (outStream != null && inStream != null) 
		{
			stopThreading = true;

		}

		if (tcpClient != null) 
		{

			tcpClient.Close();

		}

	}
}

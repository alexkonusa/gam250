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
	private string ip = "irc.chat.twitch.tv";
	private int port = 6667;
	[HideInInspector]
	public string userName = "USER NAME";
	[HideInInspector]
	public string aouthToken = "OAUTH KEY";
	[HideInInspector]
	public string channelName = "CHANNEL NAME";

	//Other Variables
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

	//To stop our threading
	bool stopThreading;

	//Buffer for recieved messages
	private string buffer;

	//Queue's & Lists
	Queue<string> recievedMessages = new Queue<string>();
	Queue<string> ircCmdQueue = new Queue<string>();
	public Queue<MSGClass> cmdToExecute = new Queue<MSGClass>();
	[HideInInspector]
	public List<string> actions = new List<string> ();


	//Use this for initialization
	void Start ()
	{

		IRCStart();

	}

	void Update()
	{

		ProcessOurRecievedMessage();

	}

	//Creating our tcp connection and threading for in and out. 
	void IRCStart()
	{
		//Create a new tcp connection
		tcpClient = new TcpClient(ip, port);

		if (!tcpClient.Connected)
		{ 
			print("Connection failed");
			return;
		} else 
			print("Connected");

		var networkStream = tcpClient.GetStream();
		streamReader = new StreamReader(networkStream);
		streamWriter = new StreamWriter(networkStream);

		//Send our details to the server to create the connection
		streamWriter.WriteLine("PASS " + aouthToken);
		streamWriter.WriteLine("NICK " + userName.ToLower());
		streamWriter.WriteLine("USER " + userName + "8 * : " + userName);
		streamWriter.Flush();

		//Create our threads and start them
		inStream = new Thread(() => IRCInput(streamReader, networkStream));
		inStream.Start();
		outStream = new Thread(() => IRCOutput(streamWriter));
		outStream.Start();

	}

	//Our input thread
	//In this thread we check for our message codes and
	//figure out what to do with them
	private void IRCInput(TextReader inStream, NetworkStream networkStream)
	{
		while (!stopThreading)
		{
			if (networkStream.DataAvailable) 
			{

				buffer = inStream.ReadLine ();

				print(buffer);

				string msg = buffer.Split (' ') [1];

				switch (msg) 
				{

				case JOIN_STATUS_CODE:
					{
						IRCJoinChannel ();
						break;
					}

				case INVALID_CMD_CODE:
					{
						print ("INVALID COMMAND");
						break;
					}

				case MSG_CODE:
					{
						recievedMessages.Enqueue (buffer);
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

	//Out thread to send messages to the server
	void IRCOutput(TextWriter outStream)
	{
		//Thread for sending mesagges to the channel! With a slight
		//delay between each message so that we dont get timeout. 
		Stopwatch stopWatch = new Stopwatch ();
		stopWatch.Start ();

		while (!stopThreading)
		{

			lock (ircCmdQueue) {

				if (ircCmdQueue.Count > 0) {

					if (stopWatch.ElapsedMilliseconds > msgSendDelay) {
						//Sending our message to the server
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

	//Process our Messages and Check if they contain an action
	public void ProcessOurRecievedMessage()
	{

		if (recievedMessages.Count > 0) 
		{
			lock (recievedMessages)
			{
				//Get the first message
				string msg = recievedMessages.Peek ();

				//Process our string and get a msg/name
				string userName = msg.Split ('!') [0];
				userName = userName.Replace (":", "");

				msg = msg.Split ('#') [1];
				string userMsg = msg.Split (':') [1];

				//If the message contains one of our actions do something with it
				for (int i = 0; i < actions.Count; i++) 
				{
					string cmd = actions [i];

					if (userMsg.Contains (cmd)) 
					{
						cmdToExecute.Enqueue (new MSGClass (userName, userMsg));
						//Debug.Log (cmdToExecute.Peek ().userName + "   " + cmdToExecute.Peek ().userCmd);
						break;
					}
					else
						print ("This message does not contain any actions");
				} 

			}

			recievedMessages.Dequeue ();

		}

	}

	//We can join multiple IRC channels at once :) 
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
	public void IRCSendWhisper(string msg, string destination)
	{

		lock (ircCmdQueue) 
		{

			ircCmdQueue.Enqueue ("PRIVMSG #" + channelName + " :" + "/w " + destination + " :" + msg + "\r\n");

		}

	}

	//Send message to the IRC server
	public void IRCSendCommand(string cmd)
	{

		lock (ircCmdQueue) 
		{

			ircCmdQueue.Enqueue (cmd + " tmi.twitch.tv\r\n");

		}

	}

	//Function to leave the channel
	public void IRCLeaveChannel()
	{

		streamWriter.WriteLine("PART #" + channelName);
		streamWriter.Flush();

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

	//Function to add the action to our action List
	public void AddAction(string action)
	{

		actions.Add (action);
		
	}
}
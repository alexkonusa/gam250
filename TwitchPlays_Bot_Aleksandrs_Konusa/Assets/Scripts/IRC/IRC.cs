//Creating connection and managing the data from/to twitch. 
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class IRC : MonoBehaviour
{
	//Connection Data
	IRC_CONNECTION_INFO _IRC_CONNECTION_INFO;

	//Other Variables
	private int msgSendDelay = 1750;

	//TCP and Threading
	private TcpClient tcpClient;
	private StreamReader streamReader;
	private StreamWriter streamWriter;
	private Thread inStream, outStream;
	NetworkStream networkStream;

	//To stop our threading
	bool stopThreading;

	//Buffer for recieved messages
	private string buffer;

	//Queue's & Lists
	Queue<string> recievedMessages = new Queue<string>();
	Queue<string> ircCmdQueue = new Queue<string>();
	public Queue<MSG_CLASS> cmdToExecute = new Queue<MSG_CLASS>();
	[HideInInspector]
	[SerializeField]
	public List<IRC_ACTION_CLASS> actions = new List<IRC_ACTION_CLASS> ();


	//Use this for initialization
	void Start ()
	{
		_IRC_CONNECTION_INFO = GetComponent<IRC_CONNECTION_INFO> ();

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
		tcpClient = new TcpClient(_IRC_CONNECTION_INFO.ip, _IRC_CONNECTION_INFO.port);

		if (tcpClient.Connected)
		{ 
			print("Connected");

			networkStream = tcpClient.GetStream();
			streamReader = new StreamReader(networkStream);
			streamWriter = new StreamWriter(networkStream);

			//Send our details to the server to create the connection
			streamWriter.WriteLine("PASS " + _IRC_CONNECTION_INFO.aouthToken);
			streamWriter.WriteLine("NICK " + _IRC_CONNECTION_INFO.userName.ToLower());
			streamWriter.WriteLine("USER " + _IRC_CONNECTION_INFO.userName + "8 * : " + _IRC_CONNECTION_INFO.userName);
			streamWriter.Flush();

			//Create and Start Our Threads
			Threading ();

		} 
		else 
		{
			print("Failed To Connect");
			Destroy (gameObject); //if connection is Failed destroy this game object. 
		}
	}

	//Threading
	void Threading()
	{

		//Create our threads and start them
		inStream = new Thread(() => IRCInput(streamReader, networkStream));
		inStream.Start();
		outStream = new Thread(() => IRCOutput(streamWriter));
		outStream.Start();
		
	}

	//Input thread, this thread listens for messages then deals with them
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

				case IRC_STATUS_CODES.JOIN_STATUS_CODE:
					{
						IRCJoinChannel (_IRC_CONNECTION_INFO.channelName);
						break;
					}

				case IRC_STATUS_CODES.INVALID_CMD_CODE:
					{
						print ("INVALID COMMAND");
						break;
					}

				case IRC_STATUS_CODES.MSG_CODE:
					{
						recievedMessages.Enqueue (buffer);
						break;
					}

				case IRC_STATUS_CODES.SERVER_REPLY_CODE:
					{
						IRCSendCommand ("PONG");
						break;
					}

				}
			}

		}
	}

	//Out thread to send messages to the server
	private void IRCOutput(TextWriter outStream)
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
	void ProcessOurRecievedMessage()
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
					string cmd = actions [i].action;

					if (userMsg.Contains (cmd)) 
					{
						cmdToExecute.Enqueue (new MSG_CLASS (userName, userMsg));
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
	public void IRCJoinChannel(string channelName)
	{

		streamWriter.WriteLine("JOIN #" + channelName);
		streamWriter.Flush();

	}

	//Sending messages to the twitch channel
	public void IRCSendMessage(string msg)
	{

		lock (ircCmdQueue) 
		{

			ircCmdQueue.Enqueue ("PRIVMSG #" + _IRC_CONNECTION_INFO.channelName + " :" + msg + "\r\n");

		}

	}

	//Private Message Function
	public void IRCSendWhisper(string msg, string destination)
	{

		lock (ircCmdQueue) 
		{

			ircCmdQueue.Enqueue ("PRIVMSG #" + _IRC_CONNECTION_INFO.channelName + " :" + "/w " + destination + " :" + msg + "\r\n");

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

		streamWriter.WriteLine("PART #" + _IRC_CONNECTION_INFO.channelName);
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
	[SerializeField]
	public void AddAction(string action)
	{

		//actions.Add (action);
		
	}
}
//Creating connection and managing the data from/to twitch. 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

public class IRC : MonoBehaviour
{

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

    private TcpClient tcpClient;
    private StreamReader streamReader;
    private StreamWriter streamWriter;
    private Thread inStream, outStream;

    public string buffer;
    bool canJoin = false;

    // Use this for initialization
    void Start ()
    {

        IRCStart();

    }

    void Update()
    {

        JoinChannel();

    }

    void IRCStart()
    {

        tcpClient = new TcpClient(ip, port);

        if (!tcpClient.Connected)
        { 
            Debug.Log("Connection failed");
        return;
        }
        else
            Debug.Log("Connected");

        var networkStream = tcpClient.GetStream();
        streamReader = new StreamReader(networkStream);
        streamWriter = new StreamWriter(networkStream);

        streamWriter.WriteLine("PASS " + aouthToken);
        streamWriter.WriteLine("NICK " + userName.ToLower());
        streamWriter.WriteLine("USER " + userName + "8 * : " + userName);
        streamWriter.Flush();

        inStream = new Thread(() => IRCInput(streamReader, networkStream));
        inStream.Start();

    }

    void IRCInput(TextReader inStream, NetworkStream networkStream)
    {

        while (true)
        {

            if (!networkStream.DataAvailable)
            {

                buffer = inStream.ReadLine();


                if (buffer.Split(' ')[1] == "001")
                {

                    Debug.Log("You can now join a channel!");

                   // streamWriter.WriteLine("JOIN #" + channelName);
                   // streamWriter.Flush();

                }

            }

            buffer = string.Empty;

        }

    }

    void JoinChannel()
    {



    }

    void SendMessage()
    {



    }
}

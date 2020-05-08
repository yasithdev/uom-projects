using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ServerListener : MonoBehaviour
{
    // Stores all required gamedata for the current session
    private Game gamedata;
	public int xsize;
	public int ysize;

	private Queue<string> commandQueue = new Queue<string>();
	private object commandLock = new object();

    // Use this for initialization
    private void Start()
    {
		gamedata = new Game();
        var thread = new System.Threading.Thread(Listen);
        thread.Start();
    }

    // Update is called once per frame
    private void Update()
    {
		if (commandQueue.Count > 0) {
			string command;
			lock (commandLock) {
				command = commandQueue.Dequeue ();
			}
			gamedata.ExecuteServerCommand (command);
		}
    }

    private void Listen()
    {
        try
        {
			var tcpListener = new TcpListener(IPAddress.Parse(Constants.CLIENT_IP), Constants.CLIENT_PORT);
            Debug.Log("Starting TCP Listener...");
            tcpListener.Start();
			Debug.Log("Started TCP Listener");
            var listen = true;
            var result = string.Empty;

            while (listen)
            {
				Debug.Log("Opening Network Stream...");
                using (var networkStream = tcpListener.AcceptTcpClient().GetStream())
                {
                    Debug.Log("Opened Network Stream");
                    var reader = new StreamReader(networkStream);
                    var data = reader.ReadToEnd();
                    networkStream.Flush();
                    Debug.Log("Received Data -> " + data);
                    result += data;
					Debug.Log("Closing Network Stream...");
                }
				Debug.Log("Closed Network Stream");

                // Make use of the string
                while (result.Contains("#"))
                {
                    // extract single command
                    var command = result.Substring(0, result.IndexOf('#') + 1);

					Debug.Log("Adding command to Queue...");
					lock(commandLock){
						commandQueue.Enqueue(command);
					}
					Debug.Log("Added command to Queue");
                    result = result.Remove(0, command.Length);
                }

            }
        }
        catch (Exception e)
        {
			Debug.LogException (e);
        }
    }
}
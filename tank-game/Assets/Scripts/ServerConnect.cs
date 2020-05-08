using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ServerConnect : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
		new System.Threading.Thread (() => MakeC2SRequest (Constants.C2S_JOIN)).Start ();
    }

    // Update is called once per frame
    private void Update()
    {
		// Ignore
    }

	public static void MakeC2SRequest(string message){
		try
		{
			Debug.Log("Creating TCP Client...");
			using (var client = new TcpClient(Constants.SERVER_IP, Constants.SERVER_PORT))
			{
				Debug.Log("Created TCP Client");
				var byteData = Encoding.ASCII.GetBytes(message);
				Debug.Log("Writing to Stream...");
				client.GetStream().Write(byteData, 0, byteData.Length);
				Debug.Log("Written to Stream");
				Debug.Log("Closing TCP Client");
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
	}
}
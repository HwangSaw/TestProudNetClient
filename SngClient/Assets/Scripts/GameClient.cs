using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Nettention.Proud;



public class GameClient : MonoBehaviour
{
    string m_serverAddr = "localhost";
    string m_villeName = "Janna";
    bool m_requestNEwVille = false;
    string m_loginButtonText = "Connect!";

    enum State { Standby, Connecting, LoggingOn, InVille, }
    State m_state;

    NetClient m_netClient = new NetClient();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_netClient != null)
            m_netClient.FrameMove();
    }


    void OnGUI()
    {
        // logon GUI
        GUI.Label(new Rect(10, 10, 300, 70), "ProudNEt example:\na simple social ville");
        GUI.Label(new Rect(10, 60, 180, 30), "Server address");
        m_serverAddr = GUI.TextField(new Rect(10, 80, 180, 30), m_serverAddr);
        GUI.Label(new Rect(10, 110, 180, 30), "Ville name");
        m_villeName =  GUI.TextField(new Rect(10, 130, 180, 30), m_villeName);
        m_requestNEwVille =  GUI.Toggle(new Rect(10, 160, 180, 20), m_requestNEwVille, "Create a new Vill");

        if (GUI.Button(new Rect(10, 190, 100, 30), "Connect!"))
        {
            if (m_state == State.Standby)
            {
                m_state = State.Connecting;
                m_loginButtonText = "Connecting...";
                IssueConnect();

            }
        }
    }

    private void IssueConnect()
    {
        m_netClient.JoinServerCompleteHandler = (ErrorInfo info, ByteArray replyFromServer) =>
        {
            if (info.errorType == ErrorType.Ok)
            {
                m_loginButtonText = "Connected!";
            }
            else
            {
                m_loginButtonText = "Failed";
            }
        };

        m_netClient.LeaveServerHandler = (ErrorInfo info) =>
        {
            m_loginButtonText = "LEFT!!!!";
        };

        // asynchronous connect
        NetConnectionParam cp = new NetConnectionParam();
        cp.serverIP = m_serverAddr;
        cp.serverPort = 12349;
        cp.protocolVersion = new Nettention.Proud.Guid("{0xbe3ab433,0x18ef,0x44fa,{0xbe,0xcb,0x50,0x8a,0x6f,0x9d,0x1b,0x77}}");

        m_netClient.Connect(cp);

    }

    private void OnDestroy()
    {
        m_netClient.Dispose();
    }
}

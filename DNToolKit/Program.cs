﻿using System;
using System.Threading;
using Common;
using DNToolKit.Packet;
using Fleck;
using Serilog;

namespace DNToolKit;

public class Program
{
    private static Dictionary<IWebSocketConnectionInfo, IWebSocketConnection> _webSocketConnections = new();
    public static void Main(string[] args)
    {
        //todo: add iridium compatability option
        
        Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().CreateLogger();
        Log.Information("DNToolKit for v2.8");


        //ugh figure out what to rename the sniffer namespace 
        var sniffer = new Sniffer.Sniffer();
        sniffer.Start();
        
        
        var server = new WebSocketServer("ws://127.0.0.1:51222");
        ProtobufFactory.Initialize();
        server.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                _webSocketConnections.Add(socket.ConnectionInfo,socket);
                //Console.WriteLine("Open!");
            };
            socket.OnClose = () =>
            {
                _webSocketConnections.Remove(socket.ConnectionInfo);
                //Console.WriteLine("Close!");
            };
            socket.OnMessage = message => socket.Send(message);
        });
        
        
        Console.WriteLine("Press Any Key to Stop!");
        Console.ReadKey(true);
        server.Dispose();
        sniffer.Close();
    }

    public static void SendWSPacket(string data)
    {
        foreach (var webSocketConnection in _webSocketConnections)
        {
            webSocketConnection.Value.Send(data);
        }
    }
    public static void SendWSPacket(byte[] data)
    {
        foreach (var webSocketConnection in _webSocketConnections)
        {
            webSocketConnection.Value.Send(data);
        }
    }
}
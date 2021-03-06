﻿using System;
using System.Net;
using System.Net.Sockets;

namespace DeltaEngine.Networking.Sockets
{
	public class TcpServerSocket : TcpSocket
	{
		public TcpServerSocket(IPEndPoint endPoint)
		{
			this.endPoint = endPoint;
		}

		private readonly IPEndPoint endPoint;

		public void StartListening()
		{
			nativeSocket.Bind(endPoint);
			nativeSocket.Listen(MaxNumberOfSimultaneouslyAcceptedClients);
			IsListening = true;
			nativeSocket.BeginAccept(AcceptCallback, null);
		}

		public bool IsListening { get; private set; }

		private const int MaxNumberOfSimultaneouslyAcceptedClients = 10;

		private void AcceptCallback(IAsyncResult asyncResult)
		{
			lock (this)
			{
				if (!IsListening)
					return;

				Socket clientHandle = nativeSocket.EndAccept(asyncResult);
				var newConnectedClient = new TcpSocket(clientHandle);
				TriggerClientConnectedEvent(newConnectedClient);
				newConnectedClient.WaitForData();
				nativeSocket.BeginAccept(AcceptCallback, null);
			}
		}

		private void TriggerClientConnectedEvent(TcpSocket newConnectedClient)
		{
			if (ClientConnected != null)
				ClientConnected(newConnectedClient);
		}

		public event Action<TcpSocket> ClientConnected;

		public int ListenPort
		{
			get { return endPoint.Port; }
		}

		public override void Dispose()
		{
			lock (this)
			{
				IsListening = false;
				base.Dispose();
			}
		}
	}
}
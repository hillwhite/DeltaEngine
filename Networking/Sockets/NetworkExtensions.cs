﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace DeltaEngine.Networking.Sockets
{
	internal static class NetworkExtensions
	{
		public static IPEndPoint ToEndPoint(this string serverAddress, int serverPort)
		{
			IPAddress ipAddress;
			if (IPAddress.TryParse (serverAddress, out ipAddress))
				return new IPEndPoint(ipAddress, serverPort);

			return GetEndpointFromHostname(serverAddress, serverPort);
		}

		private static IPEndPoint GetEndpointFromHostname(string serverAddress, int serverPort)
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(serverAddress);
			return GetIPv4Address(hostEntry.AddressList, serverPort);
		}

		private static IPEndPoint GetIPv4Address(IEnumerable<IPAddress> addresses, int port)
		{
			IPAddress ipAddress = addresses.First(x => x.AddressFamily == AddressFamily.InterNetwork);
			return new IPEndPoint(ipAddress, port);
		}
	}
}
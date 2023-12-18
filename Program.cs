using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sbCurlClone
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: sbCurlClone [-v] [-X METHOD] [-d DATA] [-H HEADER] [URL]");
                return;
            }

            bool verbose = false;
            string url = "";
            string method = "GET"; // Default method
            string data = "";
            List<string> customHeaders = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-v":
                    case "--verbose":
                        verbose = true;
                        break;
                    case "-X":
                        method = args[++i];
                        break;
                    case "-d":
                        data = args[++i];
                        break;
                    case "-H":
                        customHeaders.Add(args[++i]);
                        break;
                    default:
                        url = args[i];
                        break;
                }
            }

            if (string.IsNullOrEmpty(url))
            {
                Console.WriteLine("URL is required.");
                return;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                Console.WriteLine("Invalid URL");
                return;
            }

            string host = uri.Host;
            int port = uri.Port == -1 ? 80 : uri.Port;
            string path = string.IsNullOrEmpty(uri.AbsolutePath) ? "/" : uri.AbsolutePath;

            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(host, port);
                    if (!socket.Connected)
                    {
                        Console.WriteLine("Unable to connect to the server.");
                        return;
                    }

                    StringBuilder request = new StringBuilder();
                    request.Append($"{method} {path} HTTP/1.1\r\n");
                    request.Append($"Host: {host}\r\n");

                    foreach (var header in customHeaders)
                    {
                        request.Append(header + "\r\n");
                    }

                    if (!string.IsNullOrEmpty(data))
                    {
                        request.Append("Content-Length: " + Encoding.ASCII.GetByteCount(data) + "\r\n");
                    }

                    request.Append("Connection: close\r\n");
                    request.Append("\r\n");

                    if (!string.IsNullOrEmpty(data))
                    {
                        request.Append(data);
                    }

                    byte[] requestBytes = Encoding.ASCII.GetBytes(request.ToString());
                    socket.Send(requestBytes);

                    if (verbose)
                    {
                        Console.WriteLine("> " + request.ToString().Replace("\r\n", "\n> "));
                    }

                    byte[] buffer = new byte[256];
                    int received;
                    StringBuilder response = new StringBuilder();

                    do
                    {
                        received = socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                        if (received > 0)
                        {
                            string responsePart = Encoding.ASCII.GetString(buffer, 0, received);
                            if (verbose)
                            {
                                Console.WriteLine("< " + responsePart.Replace("\r\n", "\n< "));
                            }
                            response.Append(responsePart);
                        }
                    }
                    while (received > 0);

                    if (!verbose)
                    {
                        Console.WriteLine(response.ToString());
                    }
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine($"SocketException: {se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}

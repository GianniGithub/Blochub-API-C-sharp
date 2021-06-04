using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Blochub_API_C_sharp
{
    public class BlocStream
    {
        public event Action<Dictionary<string, dynamic>> BlockUpdate;
        public event Action OnReconnection;

        private readonly string blockServerURI;
        private ClientWebSocket socket;
        private Dictionary<string, dynamic> streamSettings;
        /// <summary>
        /// Gets the process ID of the server process.
        /// </summary>
        public int ProcessId { get; private set; }
        public bool IsConnected { get; private set; }
        public bool KeepConnected { get; set; }

        public BlocStream(string blockServerURI, Dictionary<string, dynamic> streamSettings)
        {
            this.blockServerURI = blockServerURI;
            this.streamSettings = streamSettings;

            this.socket = new ClientWebSocket();
        }

        public virtual async Task Connect()
        {

            do
            {
                using (socket = new ClientWebSocket())
                    try
                    {
                        await socket.ConnectAsync(new Uri(blockServerURI), CancellationToken.None);
                        this.IsConnected = true;
                        var json = JsonConvert.SerializeObject(streamSettings);

                        await Send(socket, json);
                        await Receive(socket);
                    }
                    catch (Exception ex)
                    {
                        string errMas;

                        if (ex is BlocStremException)
                        {
                            var blockEx = ex as BlocStremException;
                            throw blockEx;
                        }
                        else
                        {
                            errMas = string.Format("Failed to connect to WebSocket server. Error was '{0}'", ex.Message);
                            Console.WriteLine(errMas);
                        }

                        this.IsConnected = false;

                        throw ex;
                    }
            }
            while (KeepConnected);

            this.IsConnected = false;
        }

        async Task Send(ClientWebSocket socket, string data) =>
    await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);

        // see https://thecodegarden.net/websocket-client-dotnet
        async Task Receive(ClientWebSocket socket)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);

            do
            {
                WebSocketReceiveResult result;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    ms.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        var json = await reader.ReadToEndAsync();

                        Dictionary<string, dynamic> values = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

                        if (values["type"] == "error")
                            handleError(values);

                        BlockUpdate?.Invoke(values);
                    }
                }
            } while (IsConnected);
        }

        private static void handleError(Dictionary<string, dynamic> values)
        {
            string errMs;
            int errCode = 0;

            if (values.TryGetValue("code", out dynamic code))
            {

                errCode = Int32.Parse(code);

                switch (errCode)
                {
                    case 101:
                        errMs = "INVALID_EXCHANGE Self explanatory";
                        break;
                    case 102:
                        errMs = "INVALID API_KEY Something is wrong with the api-key";
                        break;
                    case 103:
                        errMs = "NOT_AUTHORIZED Not authorized(when trying so subscribe to more connection than is allowed)";
                        break;
                    case 104:
                        errMs = "BAD_REQUEST Some thing is wrong with the message format";
                        break;
                    case 105:
                        errMs = "INTERNAL_ERROR Something went wrong internally(user will be disconnected)";
                        break;
                    case 106:
                        errMs = "Exchange is offline Self explanatory";
                        break;
                    default:
                        errMs = "Some other error";
                        break;
                }

            }
            else
            {
                errMs = values["message"];
            }
            throw new BlocStremException(errMs)
            {
                ErrorCode = errCode,
                Values = values
            };
        }

        public void Disconnect()
		{
            KeepConnected = false;
            IsConnected = false;
            socket.Abort();
        }
    }

    public class BlocStremException : Exception
	{
        public BlocStremException(string errorMassage) : base(errorMassage) { }
        public int ErrorCode;
        public Dictionary<string, dynamic> Values;
        public string ErrorMassage => Values["message"];
    }
}

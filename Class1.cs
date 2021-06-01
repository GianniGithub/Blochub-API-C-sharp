using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;


namespace Blochub_API_C_sharp
{
    public class BlocStream
    {
        private readonly string serverUrl;
        private ClientWebSocket socket;
        private BlocWebSocketStream stream;

        /// <summary>
        /// Gets the process ID of the server process.
        /// </summary>
        public int ProcessId { get; private set; }
		public bool IsConnected { get; private set; }

		/// <summary>
		/// Initializes an instance of the WebsocketClientChannel.
		/// </summary>
		/// <param name="url">The full path to the server process executable.</param>
		public BlocStream(string url)
        {
            this.serverUrl = url;
            this.socket = new ClientWebSocket();
            this.stream = new BlocWebSocketStream(socket);
        }

        public virtual async Task WaitForConnection()
        {
            try
            {
                await this.socket.ConnectAsync(new Uri(serverUrl), CancellationToken.None);
            }
            catch (AggregateException ex)
            {
                var wsException = ex.InnerExceptions.FirstOrDefault() as WebSocketException;

                if (wsException != null)
                {
                    var errMas =    string.Format("Failed to connect to WebSocket server. Error was '{0}'", wsException.Message);

                    // Log here
					Console.WriteLine(errMas);

                }

                throw;
            }

            this.IsConnected = true;
        }

    }

    /// <summary>
    /// Extension of <see cref="MemoryStream"/> that sends data to a WebSocket during FlushAsync 
    /// and reads during WriteAsync.
    /// <see cref="https://www.csharpcodi.com/vs2/?source=4178/PowerShellEditorServices/src/PowerShellEditorServices.Channel.WebSocket/WebsocketServerChannel.cs"/>
    /// </summary>
    internal class BlocWebSocketStream : MemoryStream
    {
        private readonly ClientWebSocket socket;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// It is expected that the socket is in an Open state. 
        /// </remarks>
        /// <param name="socket"></param>
        public BlocWebSocketStream(ClientWebSocket socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// Reads from the WebSocket. 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (socket.State != WebSocketState.Open)
            {
                return 0;
            }

            WebSocketReceiveResult result;
            do
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, count), cancellationToken);
            } while (!result.EndOfMessage);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
                return 0;
            }

            return result.Count;
        }

        /// <summary>
        /// Sends the data in the stream to the buffer and clears the stream.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            await socket.SendAsync(new ArraySegment<byte>(ToArray()), WebSocketMessageType.Binary, true, cancellationToken);
            SetLength(0);
        }
    }
}

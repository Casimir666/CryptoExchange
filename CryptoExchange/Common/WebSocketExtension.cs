using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoExchange.Common
{
    static class WebSocketExtension
    {

        public static async Task Send(this ClientWebSocket websocket, CancellationToken cancelToken, string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await websocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancelToken);
        }


        public static async Task<string> Receive(this ClientWebSocket websocket, CancellationToken cancelToken)
        {
            var sb = new StringBuilder();
            var buffer = new byte[1024];
            WebSocketReceiveResult result;
            do
            {
                result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.Count > 0)
                {
                    var receiveMsg = new byte[result.Count];
                    Array.Copy(buffer, receiveMsg, result.Count);
                    sb.Append(Encoding.UTF8.GetString(receiveMsg));
                }
            } while (!result.EndOfMessage);
            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace AstralToolbox.Messaging
{
    internal class ToolboxWedSocketClient
    {
        private WebSocket _ws;
        public ToolboxWedSocketClient()
        {
            _ws = new WebSocket("ws://localhost:9292/getport");
        }

        public int GetportNumber()
        {
            int port = 0;

            _ws.OnOpen += (sender, e) =>
            {
                Console.WriteLine("WebSocket connection opened");
                _ws.Send("Hello server!");
            };

            _ws.OnMessage += (sender, e) =>
            {
                int myInt = int.Parse(e.Data);
                Console.WriteLine("Received integer: " + myInt);
            };
            _ws.Connect();

            return port + 9292;
        }
    }
}

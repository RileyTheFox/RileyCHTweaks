using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using UnityEngine;

namespace PlugHero
{
    public enum PlugMessageType
    {
        SONG_PERCENTAGE,
        NOTE_MISSED,
        HIGHEST_STREAK
    }

    public class PlugClient
    {

        public WebSocket Socket { get; }

        public PlugClient()
        {
            Socket = new WebSocket("ws://127.0.0.1/PlugHero");
            Socket.OnMessage += Ws_OnMessage;

            Socket.ConnectAsync();
        }

        public void SendPlugMessage(PlugMessageType type, params object[] data)
        {
            switch(type)
            {
                case PlugMessageType.SONG_PERCENTAGE:
                    SendMessage(new byte[] { (byte)PlugMessageType.SONG_PERCENTAGE, (byte)data[0] });
                    //Debug.Log("Sent Song Percentage Message");
                    break;
                case PlugMessageType.NOTE_MISSED:
                    SendMessage(new byte[] { (byte)PlugMessageType.NOTE_MISSED });
                    //Debug.Log("Sent Note Missed Message");
                    break;
                case PlugMessageType.HIGHEST_STREAK:
                    byte[] streak = BitConverter.GetBytes((int)data[0]);

                    byte[] endStreakArray = new byte[5];

                    Array.Copy(new byte[] { (byte)PlugMessageType.HIGHEST_STREAK }, 0, endStreakArray, 0, 1);
                    Array.Copy(streak, 0, endStreakArray, 1, streak.Length);

                    SendMessage(endStreakArray);
                    //Debug.Log("Sent Highest Streak Message: " + data[0]);
                    break;
            }
        }

        private void SendMessage(byte[] bytes)
        {
            if (!Socket.IsAlive)
                return;

            Socket.SendAsync(bytes, doNothing);
        }

        private void SendMessage(string data)
        {
            if (!Socket.IsAlive)
                return;

            Socket.SendAsync(data, doNothing);
        }

        private void doNothing(bool obj)
        {
            
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            
        }
    }
}

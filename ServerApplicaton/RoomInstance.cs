using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplicaton
{
    class RoomInstance
    {
        public static Room[] _room = new Room[100];
        public static RoomInstance instance = new RoomInstance();

        public void JoinOrCreateRoom(int index)
        {
            if (Network.TempPlayer[index].Room > 0)
            {
                Console.WriteLine("Player already in room");
                return;
            }

            int i = 0;

            for(i = 1; i < 100; i++)
            {
                if(_room[i]._state != Room.RoomState.Searching)
                {
                    if(_room[i]._state == Room.RoomState.Empty)
                    {
                        _room[i].player[0] = Network.Clients[index].Index;
                        _room[i]._state = Room.RoomState.Searching;
                        Network.TempPlayer[index].Room = i;
                        Console.WriteLine("Room created: " + i + " | Player added: " + " Index: " + index);
                        return;
                    }
                }
                else
                {
                    _room[i].player[1] = Network.Clients[index].Index;
                    _room[i]._state = Room.RoomState.Closed;
                    Network.TempPlayer[index].Room = i;
                    Console.WriteLine("Room joined: " + i + " |Player added: " + index);
                    ServerSendData.instance.SendMatchMaking(i);
                    return;
                }

            }
        }

        public void LeaveRoom(int index)
        {
            _room[Network.TempPlayer[index].Room]._state = Room.RoomState.Empty;
            if(_room[Network.TempPlayer[index].Room].player[0] == index)
            {
                _room[Network.TempPlayer[index].Room].player[0] = 0;
                ServerSendData.instance.SendIngame(_room[Network.TempPlayer[index].Room].player[1]);
            }
            else
            {
                _room[Network.TempPlayer[index].Room].player[1] = 0;
                ServerSendData.instance.SendIngame(_room[Network.TempPlayer[index].Room].player[0]);
            }
            Network.TempPlayer[index].Room = 0;
            Network.TempPlayer[index].Castbar = 0;
        }
    }

    class Room
    {
        public int roomIndex;
        public int[] player = new int[2];

        public RoomState _state;
        public enum RoomState
        {
            Empty,
            Searching,
            Closed
        }
    }
}

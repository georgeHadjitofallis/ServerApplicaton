using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplicaton
{
    class ServerSendData
    {
        public static ServerSendData instance = new ServerSendData();

        public void SendDataToRoom(int room, byte[]data)
        {
            for(int i = 0; i < 2; i++)
            {
                if(RoomInstance._room[room].player[i] > 0)
                {
                    SendDataTo(RoomInstance._room[room].player[i], data);
                }
            }
        }

        public void SendDataTo(int index, byte[] data)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteBytes(data);
            Network.Clients[index].myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer = null;
        }

        public void SendIngame(int index)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteInteger(1);

            //Player Data
            buffer.WriteString(Network.Player[index].Username);
            buffer.WriteString(Network.Player[index].Password);
            buffer.WriteInteger(Network.Player[index].Level);
            buffer.WriteByte(Network.Player[index].Access);
            buffer.WriteByte(Network.Player[index].FirstTime);

            SendDataTo(index, buffer.ToArray());
            buffer = null;
        }

        public void SendMatchMaking(int room)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteInteger(3);
            buffer.WriteString(Network.Player[RoomInstance._room[room].player[0]].Username);
            buffer.WriteString(Network.Player[RoomInstance._room[room].player[1]].Username);
            SendDataToRoom(room, buffer.ToArray());
            buffer = null;

            Network.TempPlayer[RoomInstance._room[room].player[0]].inMatch = true;
            Network.TempPlayer[RoomInstance._room[room].player[1]].inMatch = true;
        }

        public void SendRefreshBar(int index)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteInteger(4);
            buffer.WriteInteger(Network.TempPlayer[index].Castbar);
            SendDataTo(index, buffer.ToArray());
            buffer = null;
        }
    }
}

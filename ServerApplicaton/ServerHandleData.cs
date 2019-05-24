using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplicaton
{
    class ServerHandleData
    {
        public static ServerHandleData instance = new ServerHandleData();
        private delegate void Packet_(int Index, byte[] Data);
        private Dictionary<int, Packet_> Packets;

        public void InitMessages()
        {
            Packets = new Dictionary<int, Packet_>();
            Packets.Add(1, HandleNewAccount);
            Packets.Add(2, HandleLogin);
            Packets.Add(3, HandleLookingForMatch);
        }

        public void HandleData(int index, byte[] data)
        {
            int packetnum;
            Packet_ Packet;
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteBytes(data);
            packetnum = buffer.ReadInteger();
            buffer = null;
            if (packetnum == 0)
                return;

            if (Packets.TryGetValue(packetnum, out Packet))
            {
                Packet.Invoke(index, data);
            }

        }

        void HandleNewAccount(int index, byte[] Data)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteBytes(Data);
            int packet = buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();

            if (Database.instance.AccountExist(username) == true)
            {
                //SendAlertMsg
                return;
            }


            Database.instance.AddAccount(index, username, password);

            ServerSendData.instance.SendIngame(index);
        }

        void HandleLogin(int index, byte[] Data)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteBytes(Data);
            int packet = buffer.ReadInteger();
            string username = buffer.ReadString();
            string password = buffer.ReadString();

            if (Database.instance.AccountExist(username) == false)
            {
                //SendAlertMsg user does not exist
                return;
            }

            if(Database.instance.PasswordOK(username,password) == false)
            {
                //SendAlertMsg password does not match
                return;
            }
            Database.instance.LoadPlayer(index, username);
            ServerSendData.instance.SendIngame(index);
        }

        void HandleLookingForMatch(int index, byte[] Data)
        {
            KaymakGames.KaymakGames buffer = new KaymakGames.KaymakGames();
            buffer.WriteBytes(Data);
            int packet = buffer.ReadInteger();

            RoomInstance.instance.JoinOrCreateRoom(index);
        }
    }
}

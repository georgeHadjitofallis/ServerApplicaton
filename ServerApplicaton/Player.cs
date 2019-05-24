using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplicaton
{
    [Serializable]
    class Player
    {
        //General
        public string Username;
        public string Password;
        public int Level;
        public byte Access;
        public byte FirstTime;
    }
}

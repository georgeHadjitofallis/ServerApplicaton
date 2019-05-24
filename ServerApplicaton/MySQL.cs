using System;
using ADODB;

namespace ServerApplicaton
{
    class MySQL
    {
        public Recordset DB_RS;
        public Connection DB_CONN;

        public static MySQL mysql = new MySQL();

        public void MySQLInit()
        {
            try
            {
                DB_RS = new Recordset();
                DB_CONN = new Connection();

                DB_CONN.ConnectionString = "Driver={MySQL ODBC 3.51 Driver};Server=localhost;Port=3306;Database=clashroyaletutorial;User=root;Password=;Option=3;";
                DB_CONN.CursorLocation = CursorLocationEnum.adUseServer;
                DB_CONN.Open();
                Console.WriteLine("Connection to MYYSQL Server was successfully.");

                var db = DB_RS;
                {
                    db.Open("SELECT * FROM accounts WHERE 0=1", DB_CONN, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockOptimistic);
                    db.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}

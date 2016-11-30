using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class Logger
    {
        public enum Fields { User, DateTime, Area, Type };

        private static Logger instance;

        public static Logger getInstance()
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        private SqlConnection connectionSQL;

        private Logger()
        {
            connectionSQL = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\Jack\\Documents\\Visual Studio 2015\\Projects\\SuperSimpleLogger\\Logger\\Log.mdf\"; Integrated Security = True; Connect Timeout = 30");
        }

        public void AddLog(string userName, string area, string type)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "INSERT INTO Log VALUES ((SELECT COUNT(id) FROM log), @UserName, @DateTime, @Area, @Type, 1)";
            sqlCommand.Parameters.AddWithValue("@UserName", userName);
            sqlCommand.Parameters.AddWithValue("@DateTime", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@Area", area);
            sqlCommand.Parameters.AddWithValue("@Type", type);
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            sqlCommand.ExecuteNonQuery();
            connectionSQL.Close();
        }

        public List<Log> DumpAll()
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT * FROM Log";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            SqlDataReader dR = sqlCommand.ExecuteReader();
            List<Log> logs = new List<Log>();
            while (dR.Read())
            {
                logs.Add(new Log(dR["Id"], dR["User"], dR["DateTime"], dR["Area"], dR["Type"]));
            }
            connectionSQL.Close();
            return logs;
        }

        public List<Log> OrderBy(Fields field)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT * FROM Log ORDER BY " + fieldToString(field) + " DESC";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            SqlDataReader dR = sqlCommand.ExecuteReader();
            List<Log> logs = new List<Log>();
            while (dR.Read())
            {
                logs.Add(new Log(dR["Id"], dR["User"], dR["DateTime"], dR["Area"], dR["Type"]));
            }
            connectionSQL.Close();
            return logs;
        }

        private String fieldToString(Fields field)
        {
            switch (field)
            {
                case Fields.User:
                    return "User";
                case Fields.DateTime:
                    return "DateTime";
                case Fields.Area:
                    return "Area";
                case Fields.Type:
                    return "Type";
                default:
                    throw new InvalidOperationException();
            }
        }

        private void AddSuperLog(string userName, string area, string type)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "INSERT INTO Log VALUES ((SELECT COUNT(id) FROM log), @UserName, @DateTime, @Area, @Type, 0)";
            sqlCommand.Parameters.AddWithValue("@UserName", userName);
            sqlCommand.Parameters.AddWithValue("@DateTime", DateTime.Now);
            sqlCommand.Parameters.AddWithValue("@Area", area);
            sqlCommand.Parameters.AddWithValue("@Type", type);
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            sqlCommand.ExecuteNonQuery();
            connectionSQL.Close();
        }

        public bool DeleteLog(String user, int idLog)
        {
            SqlCommand selCommand = new SqlCommand();
            selCommand.CommandText = "SELECT COUNT(*) FROM SuperUsers WHERE [User] = @User";
            selCommand.Parameters.AddWithValue("@User", user);
            selCommand.Connection = connectionSQL;
            connectionSQL.Open();
            SqlDataReader dR = selCommand.ExecuteReader();
            if (dR.Read())
            {
                dR.Close();
                connectionSQL.Close();
                AddSuperLog(user, "logger", "Deleted a row");
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.CommandText = "DELETE FROM Log WHERE Id=@Id AND Updatable=1";
                sqlCommand.Parameters.AddWithValue("@id", idLog);
                sqlCommand.Connection = connectionSQL;
                connectionSQL.Open();
                int i = sqlCommand.ExecuteNonQuery();
                connectionSQL.Close();
                return !(i == 0);
            }
            else
            {
                dR.Close();
                connectionSQL.Close();
                AddSuperLog(user, "logger", "Attemp to delete row with id(" + idLog.ToString() + ")");
                return false;
            }
        }

        public List<Log> LastUses()
        {
            List<Log> logs = OrderBy(Fields.DateTime);
            List<String> aux = new List<string>();
            List<Log> ret = new List<Log>();
            foreach (Log log in logs)
            {
                if (!aux.Contains(log.userName) )
                {
                    aux.Add(log.userName);
                    ret.Add(log);
                }
            }
            return ret;
        }

    }
}

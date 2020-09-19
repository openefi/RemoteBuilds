using System;
using MySqlConnector;
using OpenEFI_RemoteBuild.Utils;

namespace OpenEFI_RemoteBuild.DB
{
    public static class DBController
    {
        public static MySqlConnection createConnection()
        {
            string connectionString = "datasource=localhost;port=13306;username=root;password=examplepass;database=OpenEFI_RemoteBuilds;";
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            return databaseConnection;
        }

        public static IBuildStatus BuildStatus(string ID)
        {
            IBuildStatus result = new IBuildStatus();
            MySqlConnection _DBC = createConnection();
            string query = $"SELECT * FROM builds WHERE HASH='{ID}'";
            MySqlCommand commandDatabase = new MySqlCommand(query, _DBC);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;
            try
            {
                _DBC.Open();
                reader = commandDatabase.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    result.ID = ID;
                    result.Status = reader.GetString(2);
                    result.Build_info = reader.GetString(3);
                    return result;
                }
                _DBC.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ooops");
                Console.WriteLine(ex);
            }
            result = null;
            return result;
        }
        public static bool UpdateBuildStatus(string ID, string newState)
        {
            MySqlConnection _DBC = createConnection();
            string query = $"UPDATE `builds` SET `STATUS`='{newState}' WHERE `HASH`= '{ID}'";
            MySqlCommand CDB = new MySqlCommand(query, _DBC);
            CDB.CommandTimeout = 60;
            try
            {
                _DBC.Open();
                CDB.ExecuteReader();
                _DBC.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ooops");
                Console.WriteLine(ex);
            }
            return false;
        }

        public static bool AddBuild(string ID)
        {
            MySqlConnection _DBC = createConnection();
            string query = $"INSERT INTO `builds`( `HASH`, `STATUS`, `BUILD_INFO`) VALUES ('{ID}','BUILD_INIT','')";
            MySqlCommand CDB = new MySqlCommand(query, _DBC);
            CDB.CommandTimeout = 60;
            try
            {
                _DBC.Open();
                CDB.ExecuteReader();
                _DBC.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ooops");
                Console.WriteLine(ex);
            }
            return false;
        }

    }
}
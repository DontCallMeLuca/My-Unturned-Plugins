using System;
using MySqlConnector;

namespace AutoRespawnPlugin
{
    public class DBManager
    {
        public static Configuration config = Main.Instance.Configuration.Instance;

        private static readonly string ConnectionString = $"Server={config.server};Database={config.database};User={config.user};Password={config.password};Port={config.port}";

        public static void InsertRespawnModel(RespawnModel model)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("INSERT INTO RespawnTable (SteamId, ShouldRespawn) VALUES (@SteamId, @ShouldRespawn) ON DUPLICATE KEY UPDATE ShouldRespawn = VALUES(ShouldRespawn)", connection);
                command.Parameters.AddWithValue("@SteamId", model.steamid);
                command.Parameters.AddWithValue("@ShouldRespawn", model.shouldRespawn);

                command.ExecuteNonQuery();
            }
        }

        public static RespawnModel RetrieveRespawnModels(ulong steamid)
        {
            RespawnModel model = null;

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM RespawnTable WHERE SteamId = @SteamId", connection);
                command.Parameters.AddWithValue("@SteamId", steamid);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        model = new RespawnModel(
                            steamid: Convert.ToUInt64(reader["SteamId"]),
                            shouldRespawn: Convert.ToBoolean(reader["ShouldRespawn"])
                        );
                    }
                }
            }

            return model;
        }
    }
}

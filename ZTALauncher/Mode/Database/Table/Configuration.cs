using System.Data.SQLite;

namespace ZTALauncher.Mode.Database.Table
{
    public sealed class Configuration : ITable
    {
        public static readonly string TableName = "Configuration";
        public long Id {  get; set; }
        public long GameId { get; set; }
        public string? WorkShopPath {  get; set; }

        public string GetName()
        {
            return TableName;
        }

        public long Insert(ITable configuration)
        {
            using (var connection = new SQLiteConnection(Utility.GetConnectionString()))
            {
                string insertQuery = $"INSERT INTO {GetName()} (GameId, WorkShopPath) VALUES (@gameId, @workShopPath);";
                using (var cmd = new SQLiteCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@gameId", (configuration as Configuration).GameId);
                    cmd.Parameters.AddWithValue("@workShopPath", (configuration as Configuration).WorkShopPath);
                    cmd.ExecuteNonQuery();
                    return connection.LastInsertRowId;
                }
            }
        }

        public ITable Read(long id)
        {
            using (var connection = new SQLiteConnection(Utility.GetConnectionString()))
            {
                ZTALauncher.Mode.Database.Table.Configuration configuration = new();
                string selectQuery = $"SELECT * FROM {GetName()} WHERE Id = {id};";
                using (var cmd = new SQLiteCommand(selectQuery, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Users in database:");
                    while (reader.Read())
                    {
                        configuration.WorkShopPath = reader["WorkShopPath"].ToString();
                        configuration.GameId = long.Parse(reader["GameId"].ToString());
                        configuration.Id = long.Parse(reader["Id"].ToString());
                    }

                    return configuration;
                }
            }             
        }

        public int Update(ITable configuration)
        {
            using (var connection = new SQLiteConnection(Utility.GetConnectionString()))
            {
                string updateQuery = @$"
                UPDATE {GetName()} 
                SET WorkShopPath = @WorkShopPath, 
                GameId = @GameId
                WHERE Id = {(configuration as Configuration).Id};";
                using (var cmd = new SQLiteCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@WorkShopPath", (configuration as Configuration).WorkShopPath);
                    cmd.Parameters.AddWithValue("@GameId", (configuration as Configuration).GameId);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Delete(long id)
        {
            string deleteQuery = $"DELETE FROM {GetName()} WHERE Id = {id}";
            using (var connection = new SQLiteConnection(Utility.GetConnectionString()))
            {
                using (var cmd = new SQLiteCommand(deleteQuery, connection))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

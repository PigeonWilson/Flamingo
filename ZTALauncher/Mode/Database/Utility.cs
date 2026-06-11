using System.Data.SQLite;

namespace ZTALauncher.Mode.Database
{
    public static class Utility
    {
        public static readonly string DatabaseFile = "Cyanide.sqlite";

        public static bool DatabaseExist()
        {
            return System.IO.File.Exists(DatabaseFile);
        }

        private static void CreateTables()
        {
            if (DatabaseExist()) return;

            List<string> queries = new List<string>();

            string createConfigurationTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Configuration (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        GameId INTEGER NOT NULL,
                        WorkShopPath TEXT NULL
                    );";
            queries.Add(createConfigurationTableQuery);
            string createGameTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Game (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ExecutableName TEXT NULL,
                        ExecutableSignature TEXT NULL,
                        ApplicationId TEXT NULL
                    );";
            queries.Add(createGameTableQuery);
            string createModTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Mod(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    GameId INTEGER NOT NULL,
                    ModName TEXT NULL,
                    PackageName TEXT NULL,  
                    );";
            queries.Add(createModTableQuery);
            string createDLLModTableQuery = @"
                    CREATE TABLE IF NOT EXISTS DLLMod(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    GameId INTEGER NOT NULL,
                    ModId INTEGER NOT NULL,
                    PackageName TEXT NULL,
                    DLLPath TEXT NULL
                    );";
            queries.Add(createDLLModTableQuery);
            string createAuditingTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Auditing(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Message TEXT NULL,
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                    );";
            queries.Add(createAuditingTableQuery);

            using (var connection = new SQLiteConnection(GetConnectionString()))
            {
                foreach (string query in queries)
                {
                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }            
        }

        public static void CreateDatabase()
        {
            if (!DatabaseExist())
            {
                SQLiteConnection.CreateFile(DatabaseFile);
                CreateTables();
            }
        }

        public static string GetConnectionString()
        {
            return $"Data Source={DatabaseFile};Version=3;";
        }

        public static bool TestDatabaseConnection()
        {
            bool result = false;
            try
            {
                using (var connection = new SQLiteConnection(GetConnectionString()))
                {
                    connection.Open();
                    result = true;
                    connection.Close();
                }
            }
            catch (Exception) { return result; }

            return result;
        }


    }
}

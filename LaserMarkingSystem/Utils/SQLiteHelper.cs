using InIWorkspace.Data.Entities;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace InIWorkspace.Utils
{
    public class SQLiteHelper
    {
        private readonly string _connectionString;

        public SQLiteHelper(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
        }

        public bool ValidateUser(string username, string password)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                 connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT COUNT(1)
                FROM Admins
                WHERE Username = $username AND Password = $password";
                command.Parameters.AddWithValue("$username", username);
                command.Parameters.AddWithValue("$password", password);

                var result =  command.ExecuteScalar();
                return (long)result > 0;
            }
        }
        public void InsertUser(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                INSERT INTO Users (Username, ActivationCode, MacAddress, CreatedDate, IsActive)
                VALUES ($username, $activationCode, $macAddress, $createdDate, $isActive);";

                command.Parameters.AddWithValue("$username", user.Username);
                command.Parameters.AddWithValue("$activationCode", string.Join(" ", user.ActivationCode));
                command.Parameters.AddWithValue("$macAddress", user.MacAddress);
                command.Parameters.AddWithValue("$createdDate", user.CreatedDate);
                command.Parameters.AddWithValue("$isActive", user.IsActive ? 1 : 0);

                command.ExecuteNonQuery();
            }
        }

        public void UpdateUser(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                UPDATE Users
                SET Username = $username,
                    ActivationCode = $activationCode,
                    MacAddress = $macAddress,
                    CreatedDate = $createdDate,
                    IsActive = $isActive
                WHERE Id = $id;";

                command.Parameters.AddWithValue("$id", user.Id);
                command.Parameters.AddWithValue("$username", user.Username);
                command.Parameters.AddWithValue("$activationCode", string.Join(" ", user.ActivationCode));
                command.Parameters.AddWithValue("$macAddress", user.MacAddress);
                command.Parameters.AddWithValue("$createdDate", user.CreatedDate);
                command.Parameters.AddWithValue("$isActive", user.IsActive ? 1 : 0);

                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Users WHERE Id = $id;";
                command.Parameters.AddWithValue("$id", userId);

                command.ExecuteNonQuery();
            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Users;";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            ActivationCode = reader.GetString(2).Split(' '),
                            MacAddress = reader.GetString(3),
                            CreatedDate = reader.GetString(4),
                            IsActive = reader.GetInt32(5) == 1
                        });
                    }
                }
            }
            return users;
        }

        public User GetUserById(int userId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Users WHERE Id = $id;";
                command.Parameters.AddWithValue("$id", userId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            ActivationCode = reader.GetString(2).Split(' '),
                            MacAddress = reader.GetString(3),
                            CreatedDate = reader.GetString(4),
                            IsActive = reader.GetInt32(5) == 1
                        };
                    }
                }
            }
            return null;
        }
        public bool ValidateActivationCode(string activationCode)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT ActivationCode FROM Users WHERE ActivationCode = activationCode;";
                command.Parameters.AddWithValue("$activationCode", activationCode);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var storedCodes = reader.GetString(0);
                        return storedCodes.Contains(activationCode);
                    }
                }
            }
            return false;
        }
        public void UpdateLicenseStatus(string activationCode)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                UPDATE Users
                SET IsActive =1
                WHERE ActivationCode = $activationCode;";

                command.Parameters.AddWithValue("$activationCode", activationCode); 

                command.ExecuteNonQuery();
            }
        }
        public bool GetLicenseStatus(string macAddress)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MacAddress FROM Users WHERE MacAddress = $macAddress AND IsActive = 1;";
                command.Parameters.AddWithValue("$macAddress", macAddress);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true; // If a record is found, the license is active
                    }
                }
            }
            return false; // No active license found
        }
    }
}




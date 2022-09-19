using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Ads.Data
{
    public class AdRepository
    {
        private string _connectionString;
        public AdRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int AddAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "Insert into Ads(DateSubmitted,PhoneNumber,Text,Name, UserId) " +
                "Values(GETDATE(),@phoneNumber,@text,@name, @userId) SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@phoneNumber", ad.PhoneNumber);
            command.Parameters.AddWithValue("@text", ad.Text);
            command.Parameters.AddWithValue("@name", ad.Name);
            command.Parameters.AddWithValue("@userId", ad.UserId);
            connection.Open();

            return (int)(decimal)command.ExecuteScalar();

        }
        public List<Ad> GetAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ads ORDER BY Id Desc ";
            connection.Open();
            var reader = cmd.ExecuteReader();
            var adList = new List<Ad>();
            while (reader.Read())
            {
                adList.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Text = (string)reader["text"],
                    DateSubmitted = (DateTime)reader["DateSubmitted"],
                    UserId = (int)reader["userId"]

                });
            }
            return adList;
        }
        public List<Ad> GetAds(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ads  WHERE userid=@id ORDER BY Id Desc";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = cmd.ExecuteReader();
            var adList = new List<Ad>();
            while (reader.Read())
            {
                adList.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Text = (string)reader["text"],
                    DateSubmitted = (DateTime)reader["DateSubmitted"],
                    UserId = (int)reader["userId"]

                });
            }
            return adList;
        }

        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Ads WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);
            connection.Open();
            cmd.ExecuteNonQuery();

        }
        public void AddUser(User user, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Users(Name, Email, PasswordHash) " +
                "VALUES (@name, @email, @hash)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@hash", BCrypt.Net.BCrypt.HashPassword(password));

            connection.Open();
            cmd.ExecuteNonQuery();
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid ? user : null;

        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 * FROM Users WHERE Email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            return new User
            {
                Id = (int)reader["Id"],
                Email = (string)reader["Email"],
                Name = (string)reader["Name"],
                PasswordHash = (string)reader["PasswordHash"]
            };
        }
    }
}

using Azure.Core;
using NuGet.Packaging;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Server.Database
{
    public class Database
    {
        const string connectionString = @"Data Source=Josh-PC\SQLExpress;Integrated security=SSPI;Database=Portal";

        public Database() { }

        public static bool AccessTokenExists(string accessToken)
        {
            try
            {
                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new($"SELECT 1 FROM Users WHERE AccessToken = '{accessToken}'", connection);

                    object entry = cmd.ExecuteScalar();
                    if (entry != null)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Trace.WriteLine("AccessTokenExists Exception: " + e.Message);
                return false;
            }
        }

        public static bool EmailExists(string email)
        {
            try
            {
                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new($"SELECT 1 FROM Users WHERE Email = '{email}'", connection);

                    object entry = cmd.ExecuteScalar();
                    if (entry != null)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Trace.WriteLine("EmailExists Exception: " + e.Message);
                return false;
            }
        }

        public static string AttemptLogin(string email, string password)
        {
            try
            {
                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new($"SELECT Salt FROM Users WHERE Email = '{email}'", connection);

                    string salt = String.Empty;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        salt = reader.GetGuid(0).ToString();
                    }
                    reader.Close();

                    if (string.IsNullOrEmpty(salt))
                    {
                        return String.Empty;
                    }

                    string hash = Sha256(password + salt);
                    
                    cmd = new SqlCommand(
                        $"SELECT 1 FROM Users WHERE Email = '{email}' AND Password = '{hash}'", connection);

                    object entry = cmd.ExecuteScalar();
                    if (entry != null)
                    {
                        string guid = Guid.NewGuid().ToString();

                        cmd = new SqlCommand($"UPDATE Users SET AccessToken = '{guid}'", connection);
                        cmd.ExecuteScalar();

                        return guid;
                    }
                }

                return String.Empty;
            }
            catch (Exception e)
            {
                Trace.WriteLine("AttemptLogin Exception: " + e.Message);
                return String.Empty;
            }
        }

        private static string Sha256(string toHash)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(toHash);
                byte[] hash = SHA256.HashData(bytes);
                return Convert.ToHexString(hash);
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}

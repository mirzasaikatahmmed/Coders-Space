using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Coders_Space
{
    internal class SubscriptionManager
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;

        public SubscriptionManager(string connectionString)
        {
            cs = connectionString;
        }

        public void CheckAndUpdateSubscriptionStatus()
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "SELECT T.USERID, T.USERNAME, T.SUB_VALID_MONTH, MAX(T.TRANSACTION_DATE) AS LAST_TRANSACTION_DATE, U.Role " +
                               "FROM TRANSACTIONS T " +
                               "JOIN USERS U ON T.USERID = U.ID " +
                               "GROUP BY T.USERID, T.USERNAME, T.SUB_VALID_MONTH, U.Role";

                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string userName = reader.GetString(1);
                        int subValidMonth = reader.GetInt32(2);
                        DateTime lastTransactionDate = reader.GetDateTime(3);
                        string currentRole = reader.GetString(4);

                        if (IsSubscriptionExpired(subValidMonth, lastTransactionDate) && currentRole == "PREMIUM_USER")
                        {
                            DowngradeUserToRegular(userId);
                        }
                    }
                }
            }
        }

        private bool IsSubscriptionExpired(int subValidMonth, DateTime lastTransactionDate)
        {
            DateTime expirationDate = lastTransactionDate.AddMonths(subValidMonth);
            return DateTime.Now > expirationDate;
        }

        private void DowngradeUserToRegular(int userId)
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();

                string query = "UPDATE USERS SET Role = 'REGULAR_USER' WHERE ID = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                command.ExecuteNonQuery();
            }
        }
    }
}

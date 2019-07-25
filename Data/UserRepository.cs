using System.Collections.Generic;
using System.Data.SqlClient;
using AppLogic;

namespace Data
{
    public class UserRepository : IUserRepository
    {
        protected readonly SqlConnection connection;
        public UserRepository(SqlConnection conn)
        {
            connection = conn;
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            const string query = @"select * from Users;";

            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            Id = (int)reader["Id"],
                            Username = (string)reader["UserName"],
                            Email = (string)reader["Email"],
                            Description = (string)reader["Description"],
                            City = (string)reader["City"],
                            Street = (string)reader["Street"]
                        };

                        users.Add(user);
                    }
                    return users;
                }
                else
                {
                    throw new System.InvalidOperationException("There are no rows");
                }
            }
        }
    }
}
using Dapper;
using Library.Context;
using Library.Models;
using System.Data;

namespace Library.Repositories
{
    public interface IAuthRepository
    {
        User GetUserByEmail(string email);
        void AddUser(User user);
    }
    public class AuthRepository : IAuthRepository
    {
        private readonly LibraryDbContext _dbContext;

        public AuthRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddUser(User user)
        {
            var query = "INSERT INTO Users(Name,SurName,Email,PasswordHash,PasswordSalt) VALUES(@name,@surname,@email,@passwordHash,@passwordSalt)";

            var parameters = new DynamicParameters();

            parameters.Add("name", user.Name, DbType.String);
            parameters.Add("surname", user.SurName, DbType.String);
            parameters.Add("email", user.Email, DbType.String);
            parameters.Add("passwordHash", user.PasswordHash, DbType.Binary);
            parameters.Add("passwordSalt", user.PasswordSalt, DbType.Binary);

            using (var connection = _dbContext.CreateConnection())
            {
                connection.Execute(query, parameters);
            }
        }

        public User GetUserByEmail(string email)
        {
            var query = "Select * from Users Where Email = @email";
            using (var connection = _dbContext.CreateConnection())
            {
                var user = connection.QuerySingleOrDefault<User>(query, new { email });
                return user;
            }
        }
    }
}
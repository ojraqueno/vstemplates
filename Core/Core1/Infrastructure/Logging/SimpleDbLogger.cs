using Core1.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace Core1.Infrastructure.Logging
{
    public class SimpleDbLogger
    {
        private readonly IConfiguration _configuration;

        public SimpleDbLogger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Log(Log logEntry)
        {
            var connectionString = _configuration.GetConnectionString("AppDbContext");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.Parameters.Add(new SqlParameter("@Action", logEntry.Action.AsSqlParameterValue()));
                    command.Parameters.Add(new SqlParameter("@Controller", logEntry.Controller.AsSqlParameterValue()));
                    command.Parameters.Add(new SqlParameter("@Exception", logEntry.Exception.AsSqlParameterValue()));
                    command.Parameters.Add(new SqlParameter("@Level", logEntry.Level.AsSqlParameterValue()));
                    command.Parameters.Add(new SqlParameter("@Logger", logEntry.Logger.AsSqlParameterValue()));
                    command.Parameters.Add(new SqlParameter("@Message", logEntry.Message.AsSqlParameterValue()));
                    command.Parameters.Add(new SqlParameter("@RequestBody", logEntry.RequestBody.AsSqlParameterValue()));
                    command.Parameters.Add(new SqlParameter("@User", logEntry.User.AsSqlParameterValue()));
                    command.CommandText = "INSERT INTO [dbo].[Logs] ([Action], [Controller], [Exception], [Level], [Logger], [Message], [RequestBody], [User]) VALUES (@Action, @Controller, @Exception, @Level, @Logger, @Message, @RequestBody, @User)";

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
using System;

namespace JiraStatistic.JiraClient.Clients.User
{
    public class UserLogin
    {
        public UserLogin(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(login));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));

            Login = login;
            Password = password;
        }

        public string Login { get; }
        public string Password { get; }
    }
}
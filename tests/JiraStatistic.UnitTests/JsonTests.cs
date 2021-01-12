using System;
using System.Text.Json;
using FluentAssertions;
using JiraStatistic.JiraClient.Clients.Session;
using NUnit.Framework;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace JiraStatistic.UnitTests
{
    public class JsonTests
    {
        private JsonSerializerOptions _options;
        
        public JsonTests()
        {
            _options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        
        [Test]
        public void ParseCurrentUserTest()
        {
            var json = "{\n    \"self\": \"http://www.example.com/jira/rest/api/2.0/user/fred\",\n    \"name\": \"fred\",\n    \"loginInfo\": {\n        \"failedLoginCount\": 10,\n        \"loginCount\": 127,\n        \"lastFailedLoginTime\": \"2020-12-01T18:11:49.356+0000\",\n        \"previousLoginTime\": \"2020-12-01T18:11:49.356+0000\"\n    }\n}";
            
            var currentUser = JsonSerializer.Deserialize<CurrentUser>(json, _options);

            currentUser.Should().NotBeNull();
            currentUser?.Self.Should().Be(new Uri("http://www.example.com/jira/rest/api/2.0/user/fred"));
        }
    }
}
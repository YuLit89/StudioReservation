using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RedisConnection
{
    public class RedisConnection
    {
        readonly string _ip;
        readonly string _password;

        public IDatabase _database;


        public RedisConnection(string ip, string password)
        {
            _ip = ip;
            _password = password;

            var connection = ConnectionMultiplexer.Connect($"{ip}:6379,password={password},ssl=False,abortConnect=False");

            _database = connection.GetDatabase();

            var i = connection.GetSubscriber();

        }
    }
}

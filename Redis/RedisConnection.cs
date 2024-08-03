using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    public interface IRedisConnection : IDisposable
    {
        void Publish<T>(string channel, T Message);
        void Subscribe<T>(string channel, Action<T> Message);
    }

    public class RedisConnection : IRedisConnection
    {
        readonly IDatabase _database;
        readonly ISubscriber _subscriber;

        //[Obsolete]
        public RedisConnection(string ip, string password)
        {

            var connection = ConnectionMultiplexer.Connect($"{ip}:6379,password={password},ssl=False,abortConnect=False");

            _database = connection.GetDatabase();
            _subscriber = connection.GetSubscriber();

        }

        public void Publish<T>(string channel, T Message)
        {
            try
            {
                var obj = ObjectToByteArray((T)Message);

                _subscriber.PublishAsync(channel, obj);
            }
            catch (Exception ex)
            {

            }
        }

        public void Subscribe<T>(string channel, Action<T> Message)
        {
            try
            {
                _subscriber.SubscribeAsync(channel, (k, v) =>
                {
                    var data = ByteArrayToObject<T>(v);

                    Message(data);
                });

            }
            catch (Exception ex)
            {

            }
        }

        public void Dispose()
        {
        }

        byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        T ByteArrayToObject<T>(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);

            return obj;
        }
    }
}

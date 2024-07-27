using StudioRoomType.DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioRoomType.ADO
{

    public interface IStudioRoomTypeSQL : IDisposable
    {
        IEnumerable<Room> GetAll();
        long Create(Room roomType); 
    }

    public class StudioRoomTypeSQL : IStudioRoomTypeSQL
    {
        readonly string _connection;
        public StudioRoomTypeSQL(string connection) 
        { 
            _connection = connection;
        }

        public IEnumerable<Room> GetAll()
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Room_GetAll";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var members = InternalRead(reader);
                            yield return members;
                        }
                    }
                }
            }
        }


        internal Room InternalRead(SqlDataReader reader)
        {
            DateTime? dt = null;
            return new Room
            {
                Id = int.Parse(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Image = reader["Image"].ToString(),
                Size = reader["Size"].ToString(),
                Style = reader["Style"].ToString(),
                Rate= decimal.Parse(reader["Rate"].ToString()),
                CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString()),
                UpdatedDate = Convert.IsDBNull(reader["UpdatedDate"])   ? dt : Convert.ToDateTime(reader["UpdatedDate"]),
                CreateBy = reader["CreatedBy"].ToString(),
                UpdateBy = reader["UpdatedBy"].ToString()
            };
        }

        public long Create(Room roomType)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        
    }
}

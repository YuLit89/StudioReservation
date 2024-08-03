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
        int Create(Room roomType);

        int Delete(int RoomId);

        int Edit(Room room);

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

        public int Create(Room roomType)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Room_Create";
                    cmd.Parameters.AddWithValue("@Name", roomType.Name);
                    cmd.Parameters.AddWithValue("@Description",roomType.Description);
                    cmd.Parameters.AddWithValue("@Image", roomType.Image);
                    cmd.Parameters.AddWithValue("@Size", roomType.Size);
                    cmd.Parameters.AddWithValue("@Style", roomType.Style);
                    cmd.Parameters.AddWithValue("@Rate", roomType.Rate);
                    cmd.Parameters.AddWithValue("@CreatedBy", roomType.CreateBy);
                    cmd.Parameters.AddWithValue("@CreatedDate", roomType.CreatedDate);
                    
                    var o = new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(o);

                    cmd.ExecuteNonQuery();

                    return int.Parse(o.Value.ToString());
                }
            }
        }

        public void Dispose()
        {
        }

        public int Delete(int RoomId)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Room_Delete";
                    cmd.Parameters.AddWithValue("@Id", RoomId);

                    var r = cmd.ExecuteNonQuery();

                    if (r > 0) return 0;

                    return -1;
                }
            }
        }

        public int Edit(Room room)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Room_Edit";
                    cmd.Parameters.AddWithValue("@id", room.Id);
                    cmd.Parameters.AddWithValue("@name", room.Name);
                    cmd.Parameters.AddWithValue("@description", room.Description);
                    cmd.Parameters.AddWithValue("@image", room.Image);
                    cmd.Parameters.AddWithValue("@size", room.Size);
                    cmd.Parameters.AddWithValue("@style", room.Style);
                    cmd.Parameters.AddWithValue("@rate", room.Rate);
                    cmd.Parameters.AddWithValue("@updatedby", room.UpdateBy);
                    cmd.Parameters.AddWithValue("@updatetime", room.UpdatedDate);

                    var r = cmd.ExecuteNonQuery();

                    if (r > 0) return 0;

                    return -1;
                }
            }
        }
    }
}

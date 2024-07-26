using StudioReservation.DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioReservation.ADO
{

    public interface IStudioReservationADO : IDisposable
    {
        IEnumerable<RoomTimeSlot> GetAllTimeSlot();

        long CreateTimeSlot(RoomTimeSlot TimeSlot);
        long UpdateTimeSlot(long Id,string Times,string UpdateBy,DateTime UpdateTime,bool Enable);

        int DeleteTimeSlot(long Id);

        IEnumerable<ReservationHistory> GetAllReservationHistory();

        long CreateReservation(ReservationHistory Reservation);
        long UpdateReservationStatus(long ReservationId, int Status,DateTime UpdateTime,string Remark);
    }

    public class StudioReservationSQL : IStudioReservationADO
    {

        readonly string _connection;

        public StudioReservationSQL(string connection)
        {
            _connection = connection;
        }

        public IEnumerable<RoomTimeSlot> GetAllTimeSlot()
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RoomTimeSlot_GetAll";

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

        private RoomTimeSlot InternalRead(SqlDataReader reader)
        {

            var m = new RoomTimeSlot
            {
                Id = long.Parse(reader["Id"].ToString()),
                RoomId = int.Parse(reader["RoomId"].ToString()),
                Date = Convert.ToDateTime(reader["Date"].ToString()),
                Times = reader["Times"].ToString(),
                CreatedBy = reader["CreateBy"].ToString(),
                CreateTime = Convert.ToDateTime(reader["CreateTime"].ToString()),
                UpdateBy = reader["UpdateBy"].ToString(),
                UpdateTime = Convert.ToDateTime(reader["UpdateTime"].ToString()),
                Enable = bool.Parse(reader["Enable"].ToString())
            };

            return m;


        }

        public long CreateTimeSlot(RoomTimeSlot Request)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RoomTimeSlot_Insert";
                    cmd.Parameters.AddWithValue("@RoomId", Request.RoomId);
                    cmd.Parameters.AddWithValue("@Date", Request.Date);
                    cmd.Parameters.AddWithValue("@Times", Request.Times);
                    cmd.Parameters.AddWithValue("@CreateBy", Request.CreatedBy);
                    cmd.Parameters.AddWithValue("@CreateTime", Request.CreateTime);
                    cmd.Parameters.AddWithValue("@UpdateBy", Request.UpdateBy);
                    cmd.Parameters.AddWithValue("@UpdateTime", Request.UpdateTime);
                    cmd.Parameters.AddWithValue("@Enable", Request.Enable);

                    var o = new SqlParameter("@Id", SqlDbType.BigInt) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(o);

                    cmd.ExecuteNonQuery();

                    return long.Parse(o.Value.ToString());
                }
            }
        }

        public int DeleteTimeSlot(long Id)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RoomTimeSlot_Delete";

                    cmd.Parameters.AddWithValue("@Id", Id);
                
                    var r = cmd.ExecuteNonQuery();

                    return 0;
                }
            }
        }

        
        public long UpdateTimeSlot(long Id, string Times, string UpdateBy, DateTime UpdateTime, bool Enable)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RoomTimeSlot_Update";

                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Times", Times);
                    cmd.Parameters.AddWithValue("@UpdateBy", UpdateBy);
                    cmd.Parameters.AddWithValue("@UpdateTime", UpdateTime);
                    cmd.Parameters.AddWithValue("@Enable", Enable);
                    
                    var r = cmd.ExecuteNonQuery();

                    if (r > 0) return Id;

                    return -1;
                }
            }
        }

        public void Dispose()
        {
        }

        public IEnumerable<ReservationHistory> GetAllReservationHistory()
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RoomReservationHistory_GetAll";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var record = HistoryInternalRead(reader);
                            yield return record;
                        }
                    }
                }
            }
        }

        public ReservationHistory HistoryInternalRead(SqlDataReader reader)
        {
            return new ReservationHistory
            {
                Id = long.Parse(reader["Id"].ToString()),
                RoomId = int.Parse(reader["RoomId"].ToString()),
                DateTime = Convert.ToDateTime(reader["Date"].ToString()),
                Status = int.Parse(reader["Status"].ToString()),
                ReservationBy = reader["ReservationBy"].ToString(),
                CreateTime = Convert.ToDateTime(reader["CreateTime"].ToString()),
                UpdateTime = Convert.ToDateTime(reader["UpdatedTime"].ToString()),
                BookingId = reader["BookingId"].ToString(),
                Remark = reader["Remark"].ToString(),
                Price = decimal.Parse(reader["Price"].ToString())
            };
        }

        public long CreateReservation(ReservationHistory Reservation)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RoomReservationHistory_Insert";
                    cmd.Parameters.AddWithValue("@RoomId", Reservation.RoomId);
                    cmd.Parameters.AddWithValue("@Date", Reservation.DateTime);
                    cmd.Parameters.AddWithValue("@Status", Reservation.Status);
                    cmd.Parameters.AddWithValue("@ReservationBy", Reservation.ReservationBy);
                    cmd.Parameters.AddWithValue("@CreateTime", Reservation.CreateTime);
                    cmd.Parameters.AddWithValue("@UpdatedTime", Reservation.UpdateTime);
                    cmd.Parameters.AddWithValue("@BookingId", Reservation.BookingId);
                    cmd.Parameters.AddWithValue("@Remark", Reservation.Remark);
                    cmd.Parameters.AddWithValue("@Price", Reservation.Price);

                    var o = new SqlParameter("@Id", SqlDbType.BigInt) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(o);

                    cmd.ExecuteNonQuery();

                    return long.Parse(o.Value.ToString());
                }
            }
        }

        public long UpdateReservationStatus(long ReservationId, int Status, DateTime UpdateTime,string Remark)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "RoomReservationHistory_Update";

                    cmd.Parameters.AddWithValue("@Id", ReservationId);
                    cmd.Parameters.AddWithValue("@Status", Status);
                    cmd.Parameters.AddWithValue("@UpdatedTime", UpdateTime);
                    cmd.Parameters.AddWithValue("@Remark", Remark);

                    var r = cmd.ExecuteNonQuery();

                    if (r > 0) return ReservationId;

                    return -1;
                }
            }
        }
    }
}

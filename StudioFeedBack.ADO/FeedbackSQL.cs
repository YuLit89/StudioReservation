using StudioFeedBack.DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioFeedBack.ADO
{

    public interface IFeedbackSQL
    {
        IEnumerable<Feedback> GetAll();

        long Insert(Feedback Feedback);
        long UpdateStatus(bool isCompleted);
        IEnumerable<Feedback> Find(string TicketId);

    }

    public class FeedbackSQL : IFeedbackSQL
    {
        readonly string _connection;
        public FeedbackSQL(string connection) 
        {
            _connection = connection;
        }

        public IEnumerable<Feedback> Find(string TicketId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Feedback> GetAll()
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Feedback_GetAll";

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


        internal Feedback InternalRead(SqlDataReader reader)
        {
            DateTime? dt = null;
            return new Feedback
            {
                Id = long.Parse(reader["Id"].ToString()),
                TicketId = reader["TicketId"].ToString(),
                Title = reader["Title"].ToString(),
                UserEmail = reader["UserEmail"].ToString(),
                UserPhoneNumber = reader["UserPhoneNumber"].ToString(),
                Message = reader["Message"].ToString(),
                SubmitTime = Convert.ToDateTime(reader["SubmitTime"].ToString()),
                Type = int.Parse(reader["Type"].ToString()),
                Preference = reader["Preference"].ToString(),
                ReplyName = reader["ReplyName"].ToString(),
                Status =int.Parse(reader["Status"].ToString())
            };
        }

        public long Insert(Feedback Feedback)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Feedback_Insert";
                    cmd.Parameters.AddWithValue("@TicketId", Feedback.TicketId);
                    cmd.Parameters.AddWithValue("@Title", Feedback.Title);
                    cmd.Parameters.AddWithValue("@UserEmail", Feedback.UserEmail);
                    cmd.Parameters.AddWithValue("@UserPhoneNumber", Feedback.UserPhoneNumber);
                    cmd.Parameters.AddWithValue("@Message", Feedback.Message);
                    cmd.Parameters.AddWithValue("@SubmitTime", Feedback.SubmitTime);
                    cmd.Parameters.AddWithValue("@Type", Feedback.Type);
                    cmd.Parameters.AddWithValue("@Preference", Feedback.Preference);
                    cmd.Parameters.AddWithValue("@ReplyName", Feedback.ReplyName);
                    cmd.Parameters.AddWithValue("@Status",Feedback.Status);

                    var o = new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(o);

                    cmd.ExecuteNonQuery();

                    return int.Parse(o.Value.ToString());
                }
            }
        }

        public long UpdateStatus(bool isCompleted)
        {
            throw new NotImplementedException();
        }
    }
}

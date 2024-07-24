using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using StudioMember.Service.Contract.Models;

namespace StudioMember.Service
{

    public interface IMemberSQL : IDisposable
    {
        IEnumerable<Member> GetAll();
        int Update(string MemberId,string Email , string PhoneNumber);
    }

    public class MemberSQL : IMemberSQL
    {
        readonly string _connection;

        public MemberSQL(string connection)
        {
            _connection = connection;
        }

        public IEnumerable<Member> GetAll()
        {
           
                using (var conn = new SqlConnection(_connection))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Member_GetAll";

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

        private Member InternalRead(SqlDataReader reader)
        {

            var m = new Member
            {
                Id = reader["Id"].ToString(),
                Email = reader["Email"].ToString(),
                EmailConfirmed = bool.Parse(reader["EmailConfirmed"].ToString()),
                Password = reader["PasswordHash"].ToString(),
                PhoneNumber = reader["PhoneNumber"].ToString(),
                UserName = reader["UserName"].ToString()
            };
            return m;


        }

        public int Update(string MemberId ,string Email, string PhoneNumber)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Member_Update";

                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@phone", PhoneNumber);
                    cmd.Parameters.AddWithValue("@id", MemberId);
                    
                    var r = cmd.ExecuteNonQuery();

                    if (r > 0) return 0;

                    return -1;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}

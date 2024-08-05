using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using StudioMember.DataModel;

namespace StudioMember.Service
{

    public interface IMemberSQL : IDisposable
    {
        IEnumerable<Member> GetAll();

        IEnumerable<MemberRole> GetMemberRole();

        int Update(string MemberId,string NickName , string PhoneNumber,bool isDisable , DateTime UpdateTime);
        int UpdateStatus(string MemberId, bool isDisable);
        int UpdateRegisterTime(string Id, DateTime RegisterTime, string Ip);
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
                UserName = reader["UserName"].ToString(),
                isDisable = Convert.ToBoolean(reader["isDisable"])
            };
            return m;


        }

        public int Update(string MemberId ,string NickName, string PhoneNumber, bool isDisable, DateTime UpdateTime)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Member_Update";

                    cmd.Parameters.AddWithValue("@username", NickName);
                    cmd.Parameters.AddWithValue("@phone", PhoneNumber);
                    cmd.Parameters.AddWithValue("@id", MemberId);
                    cmd.Parameters.AddWithValue("@isdisable", isDisable);
                    cmd.Parameters.AddWithValue("@updatetime", UpdateTime);
                    
                    var r = cmd.ExecuteNonQuery();

                    if (r > 0) return 0;

                    return -1;
                }
            }
        }

        public int UpdateStatus(string MemberId,bool isDisable)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Member_UpdateStatus";

                    cmd.Parameters.AddWithValue("@id", MemberId);
                    cmd.Parameters.AddWithValue("@isdisable", isDisable);
             
                    var r = cmd.ExecuteNonQuery();

                    if (r > 0) return 0;

                    return -1;
                }
            }
        }

        public IEnumerable<MemberRole> GetMemberRole()
        {

            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Role_GetAll";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var members = InternalReadRole(reader);
                            yield return members;
                        }
                    }
                }
            }
        }

        private MemberRole InternalReadRole(SqlDataReader reader)
        {

            var m = new MemberRole
            {
                UserId = reader["UserId"].ToString(),
                Role = reader["Name"].ToString(),
            };
            return m;


        }

        public void Dispose()
        {
        }

        public int UpdateRegisterTime(string Id, DateTime RegisterTime, string Ip)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Member_UpdateRegisterTime";

                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.Parameters.AddWithValue("@CreateTime", RegisterTime);
                    cmd.Parameters.AddWithValue("@ip", Ip);

                    var r = cmd.ExecuteNonQuery();

                    if (r > 0) return 0;

                    return -1;
                }
            }
        }
    }
}

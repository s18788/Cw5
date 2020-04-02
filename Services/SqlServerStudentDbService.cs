using Cw5.DTO_s.Requests;
using System.Data;
using System.Data.SqlClient;

namespace Cw5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        public void EnrollStudent(EnrollStudentRequest request)
        {
            using (var con = NewConnection())
            using (var com = new SqlCommand())
            {

                com.Connection = con;

                var tran = con.BeginTransaction();



                com.CommandText = "select IdStudy from studies where name=@name";
                com.Parameters.AddWithValue("name", request.Studies);
                com.Transaction = tran;
                int studiesId;

                using (var dr = com.ExecuteReader())
                {

                    if (!dr.Read())
                    {
                        tran.Rollback();

                        throw new NotFoundException("Studia nie istnieja");
                    }



                }
                com.CommandText = "select IdStudy from studies where name=@name";
                com.Parameters.AddWithValue("name", request.Studies);
                com.Transaction = tran;

                using (var dr = com.ExecuteReader())
                {


                    if (!dr.Read())
                    {
                        tran.Rollback();

                        throw new NotFoundException("Studia nie istnieja");
                    }
                    else
                    {
                        studiesId = dr.GetInt32(1);


                    }
                }

                com.CommandText = "select IdEnrollment from enrollment where idStudy = @idStudy and semester = 1";
                com.Parameters.AddWithValue("idStudy", studiesId);
                com.Transaction = tran;

                using (var dr = com.ExecuteReader())
                {
                    int enrollmentId;

                    if (!dr.Read())
                    {
                        enrollmentId = 1;

                    }
                    else
                    {
                        enrollmentId = dr.GetInt32(1) + 1;



                    }
                }




            }

        }


        public void PromoteStudents(int semester, string studies)
        {
            using (var con = NewConnection())
            using (var com = new SqlCommand())
            {

                com.Connection = con;

                var tran = con.BeginTransaction();



                com.CommandText = "select * from enrollment, studies where enrollment.IdStudy = studies.IdStudy and enrollment.semester = @semester and studies.name=@name";
                com.Parameters.AddWithValue("name", studies);
                com.Parameters.AddWithValue("semester", semester);
                com.Transaction = tran;


                using (var dr = com.ExecuteReader())
                {

                    if (!dr.Read())
                    {
                        throw new NotFoundException("brak");
                    }



                }
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "PromoteStudents";
                com.Parameters.Add(new SqlParameter("@semester", semester));
                com.Parameters.Add(new SqlParameter("@studies", studies));
                com.Transaction = tran;

                using (var dr = com.ExecuteReader())
                {

                }

            }
        }

        private SqlConnection NewConnection()
        {
            SqlConnection newConnection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18788;Integrated Security=True;MultipleActiveResultSets=true");
            newConnection.Open();
            return newConnection;


        }



    }
}


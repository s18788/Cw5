using Cw5.DTO_s.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Cw5.Services
{
   public interface IStudentDbService
    {


        public void EnrollStudent(EnrollStudentRequest request);
       public void PromoteStudents(int semester, string studies);
        public void LogIn(string indexNumber, string password);
    }
}

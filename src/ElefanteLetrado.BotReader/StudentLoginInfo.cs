using System.Configuration;

namespace ElefanteLetrado.BotReader
{
    public class StudentLoginInfo
    {
        public string TeacherLogin
        {
            get { return ConfigurationManager.AppSettings["auth:teacherLogin"]; }
        }

        public string CourseName
        {
            get { return ConfigurationManager.AppSettings["auth:courseName"]; }
        }

        public string StudentName
        {
            get { return ConfigurationManager.AppSettings["auth:studentName"]; }
        }

        public string StudentPassword
        {
            get { return ConfigurationManager.AppSettings["auth:studentPassword"]; }
        }
    }
}
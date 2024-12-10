using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models
{
    public class StudentResponse
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Qualification { get; set; }

        public string CcNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string ProfilePicturePath { get; set; }

        public string PicturePath { get; set; }
    }
}

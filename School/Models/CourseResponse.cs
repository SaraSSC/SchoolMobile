using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models
{
    public class CourseResponse
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Area { get; set; }

        public int Duration { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models
{
    public class EvaluationResponse
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }

        public int HoursAbsence { get; set; }

        public int PercentageAbsence { get; set; }

        public DateTime? Date { get; set; }

        public int? Grade { get; set; }

        public bool FailedAbsence { get; set; }

        public bool FailedGrade { get; set; }
    }
}

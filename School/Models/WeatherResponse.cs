using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models
{
    public class WeatherResponse
    {
        public Coord Coord { get; set; }

        public Weather[] Weather { get; set; }

        public Main Main { get; set; }

        public Wind Wind { get; set; }

        public Clouds Clouds { get; set; }

        public Sys Sys { get; set; }

        public int Dt { get; set; }

        public string Name { get; set; }
    }
}

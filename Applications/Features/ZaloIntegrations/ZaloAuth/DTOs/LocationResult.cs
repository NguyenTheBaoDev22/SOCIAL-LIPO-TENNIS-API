using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Features.ZaloIntegrations.ZaloAuth.DTOs
{
    public class LocationResultDto
    {
        public string Provider { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long Timestamp { get; set; }
    }
}

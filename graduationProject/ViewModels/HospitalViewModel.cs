using graduationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.ViewModels
{
    public class HospitalViewModel
    {
        public string Hos_Name { get; set; }
        public IEnumerable<UserReservation> userResverations { get; set; }
    }
}

using graduationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graduationProject.ViewModels
{
    public class TubesViewModel
    {
        public string OxgnType { get; set; }
        public IEnumerable<UserReservationOxegin> UserReservationOxegin { get; set; }
    }
}

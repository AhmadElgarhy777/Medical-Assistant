using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Enums;

namespace Models
{
    public class Clinic: ModelBase
    {
            public Governorate Governorate { get; set; }
            public string Address { get; set; } = null!;
            public string City { get; set; } = null!;
            public string Bio { get; set; }= null!;
            public string price { get; set; }= null!;
            public ConfrmationStatus Status { get; set; } = ConfrmationStatus.Pending;

            public string DoctorId { get; set; }=null!;
            public Doctor Doctor { get; set; } = null!;

            public Collection<ClinicPhone> phones { get; set; } = null!;


        }
    }

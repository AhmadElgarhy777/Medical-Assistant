using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SearchNurseResultDto
    {
        public string ID { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public GenderEnum Gender { get; set; }
        public string Img { get; set; } = null!;
        public int Age { get; set; }
        public string Email { get; set; } = null!;
        public decimal PricePerHours { get; set; }
        public NurseSpecialtyEnum NurseSpecialty { get; set; }

    }

    public class SearchNurseDetailsResultByIdDto
    {
        public string ID { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public GenderEnum Gender { get; set; }
        public string Img { get; set; } = null!;
        public int Age { get; set; }
        public string Email { get; set; } = null!;
        public string Degree { get; set; } = null!;
        public Governorate Governorate { get; set; }
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public double RattingAverage { get; set; }
        public string Bio { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Experence { get; set; } = null!;
        public decimal PricePerHours { get; set; }
        public string? WorkAt { get; set; }
        public NurseSpecialtyEnum NurseSpecialty { get; set; }
        public List<NurseServiceDto> nurseServiceDtos { get; set; }

    }

    public class NurseServiceDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
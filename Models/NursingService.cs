using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NursingService:ModelBase
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<NurseService> NurseServices { get; set; }
            = new List<NurseService>();

    }

    public class NurseService:ModelBase
    {
        public string NurseId { get; set; }= null!;

        public Nures Nurse { get; set; } = null!;

        public string ServiceId { get; set; }= null!;

        public NursingService Service { get; set; } = null!;

    }


}

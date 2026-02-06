using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.RegistertionDTOs
{
    public class ConfirmationTypeDTO
    {
        public string UserId { get; set; } = null!;
        public ConfirmationEmailTypeEnum Type { get; set; }
    }
}

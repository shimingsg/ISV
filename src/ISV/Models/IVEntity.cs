using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISV.Models
{
    public class IVEntity
    {
        public int Id { get; set; }

        [Display(Name = "Created At")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
        [DataType(DataType.DateTime)]
        public DateTime LastUpdatedAt { get; set; }
    }
}

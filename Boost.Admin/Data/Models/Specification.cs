using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Boost.Admin.DTOs;
using System.Text.Json;

namespace Boost.Admin.Data.Models
{
    public class Specification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string SpecificationJson { get; set; }

        [NotMapped]
        public SpecDto Spec
        {
            get
            {
                if (!string.IsNullOrEmpty(SpecificationJson))
                    return JsonSerializer.Deserialize<SpecDto>(SpecificationJson);
                else
                    return null;
            }
        }
    }
}

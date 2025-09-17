using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Boost.Admin.DTOs;
using System.Text.Json;

namespace Boost.Admin.Data.Models
{
    public class Geometry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string GeometryJson { get; set; }

        [NotMapped]
        public List<KeyValueDto>? Geo
        {
            get
            {
                if (string.IsNullOrEmpty(GeometryJson))
                {
                    return JsonSerializer.Deserialize<List<KeyValueDto>>(GeometryJson);
                }
                return null;
            }
        }
    }
}

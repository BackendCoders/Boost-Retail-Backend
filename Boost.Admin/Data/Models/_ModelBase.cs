using System.ComponentModel.DataAnnotations.Schema;

namespace Boost.Admin.Data.Models
{
    public abstract class ModelBase
    {
        public ModelBase()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
    }
}

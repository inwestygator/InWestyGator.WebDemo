using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InWestyGator.WebDemo.Core.Models
{
    public class Pack
    {
        [Key]
        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Number { get; set; }

        [Required]
        [RegularExpression(@"^pack\..*$", ErrorMessage = "The Id must start with 'pack.'")]
        public string Id { get; set; }

        [Required]
        public string PackName { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public IList<string> Content { get; set; }

        public IList<string> ChildPackIds { get; set; }

        [JsonIgnore]
        public virtual IList<Pack> Children { get; set; }

        [JsonIgnore]
        public virtual IList<Pack> Parents { get; set; }
    }
}

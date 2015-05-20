using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }

        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }

        public virtual ICollection<ParticipantResult> Submissions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class CrowdWorker
    {
        /// <summary>
        /// This is our system generated ID
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// This is the ID crowd workers got from other crowdsource platforms
        /// </summary>
        [Index("CrowdWorkerSourceIdSourceIndex", 1, IsUnique = true), MaxLength(255)]
        public string SourceId { get; set; }
        /// <summary>
        /// This is the source crowdsource platform where this worker come from
        /// </summary>
        [Index("CrowdWorkerSourceIdSourceIndex", 2, IsUnique = true), MaxLength(255)]
        public string Source { get; set; }

        public String Email { get; set; }
        public String DisplayName { get; set; }

        public virtual ICollection<ParticipantTask> SpeechingRecordings { get; set; }
    }
}

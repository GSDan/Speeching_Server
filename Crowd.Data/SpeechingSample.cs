namespace Crowd.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SpeechingSample
    {
        public int Id { get; set; }

        public string Filename { get; set; }

        public string Description { get; set; }

        public string Con { get; set; }

        public string Active { get; set; }

        public string Truth { get; set; }

        public string Person { get; set; }
    }
}

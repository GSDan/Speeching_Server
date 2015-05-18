using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Crowd.Model.Interface;
using System.Collections.Generic;
using System;

namespace Crowd.Model.Data
{
    public class CrowdRowResponse : ICrowdRowResponse
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Tainted { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int JobId { get; set; }
        public int WorkerId { get; set; }
        public double Trust { get; set; }

        public int ParticipantResultId { get; set; }
        public int ParticipantTaskId { get; set; }

        // eg "txta":"hello can I order a pizza please"
        public Dictionary<string, string> Data { get; set; }
    }
}
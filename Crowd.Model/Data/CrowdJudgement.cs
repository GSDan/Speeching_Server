using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Model.Data
{
    public class CrowdJudgement
    {
        public int Id { get; set; }
        public bool Tainted { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int JobId { get; set; }
        public int WorkerId { get; set; }
        public double Trust { get; set; }

        public virtual ICollection<CrowdJudgementData> Data { get; set; }

        // The row this judgement was given for
        public string CrowdRowResponseId { get; set; }

        /// <summary>
        /// Takes the string dictionary created by the JSON conversion and creates CrowdJudgementDatas objs, 
        /// which can be stored in the database and queried
        /// </summary>
        /// <param name="jsonData"></param>
        public async Task AddData(Dictionary<string, string> jsonData, CrowdContext DB)
        {
            Data = new List<CrowdJudgementData>();

            foreach (KeyValuePair<string, string> pair in jsonData)
            {
                CrowdJudgementData newData = new CrowdJudgementData
                {
                    CrowdJudgementId = this.Id,
                    DataType = pair.Key,
                    StringResponse = pair.Value
                };

                switch (newData.DataType)
                {
                    case "rlsttrans" :
                    case "rlstaccent":
                        newData.NumResponse = (int)char.GetNumericValue(newData.StringResponse[0]);
                        break;
                }

                Data.Add(newData);
                DB.CrowdJudgementDatas.Add(newData);
            }

            try
            {
                await DB.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                
                throw exception;
            }
        }
    }

    public class CrowdJudgementData
    {
        public int Id { get; set; }
        public int CrowdJudgementId { get; set; }

        public string DataType { get; set; }
        public string StringResponse { get; set; }
        public int NumResponse { get; set; }
    }
}

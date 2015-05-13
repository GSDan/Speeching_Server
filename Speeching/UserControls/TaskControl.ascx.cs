using Crowd.Model.Common;
using Crowd.Model.Data;
using Crowd.Presenter;
using Crowd.View;
using Speeching.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Crowd.Service.CrowdFlower;

namespace Speeching.UserControls
{
    public partial class TaskControl : System.Web.UI.UserControl, ITaskView
    {
        TaskPresenter _taskPresenter;

        private int _stageCount;
        private int _taskId;
        public void Page_Load(object sender, EventArgs e)
        {
            //CrowdFlowerApi.CreateJob(new List<ParticipantResult>());
            
            _taskId = SpeechingUtil.ParseInt(Request.QueryString["tid"]);//TODO: QueryString or Session?!
            if (_taskId <= 0)
                _taskId = 3;//This is a temporary id that we know its in the database

            _taskPresenter = new TaskPresenter(this);
            _taskPresenter.GetTask();
        }
        #region properties
        public string Description { get; set; }
        public Dictionary<int, string> Status { get; set; }
        public int TaskId { get { return _taskId; } }
        public string Name { get; set; }
        //public string PhysicalPath { get; set; }
        //public int Stage { get; set; }
        public TaskType TaskType { get; set; }
        public string Url
        {
            set
            {
                //litSound.Text = String.Format("<iframe class=\"iframe_sound page_margin_top\" src=\"{0}\"></iframe>", value);
            }
        }
        public DateTime? CreatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        #endregion

        #region events
        public event EventHandler<EventArgs> NewTask;
        public event EventHandler<EventArgs> NextTask;
        public event EventHandler<EventArgs> PrevTask;
        public event EventHandler<EventArgs> SaveTask;
        #endregion

        #region Feedback properties
        public String Transcrib1
        {
            get { return txtTrans1.Text; }
        }
        public String Transcrib2
        {
            get { return txtTrans2.Text; }
        }
        public String Transcrib3
        {
            get { return txtTrans3.Text; }
        }
        public String Transcrib4
        {
            get { return txtTrans4.Text; }
        }
        #endregion

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int activeIdx = mvTaskStages.ActiveViewIndex;
            if (activeIdx > -1 && activeIdx < _stageCount - 1)
                mvTaskStages.ActiveViewIndex += 1;
        }

        public ICollection<CrowdTaskRow> TaskRows
        {
            set
            {
                _stageCount = 0;
                if (value != null && value.Any())
                {
                    _stageCount = value.Count;
                    //litSound.Text = "";
                    //String htmlBody = "";
                    int count = 1;
                    foreach (var r in value)
                    {
                        switch(count)
                        {
                            case 1:
                                litDesc1.Text = r.Description;
                                litSound1.Text = String.Format("<iframe class=\"iframe_sound page_margin_top\" src=\"{0}\"></iframe><br>", r.Url);
                                txtTrans1.Text = "";
                                break;
                            case 2:
                                litDesc2.Text = r.Description;
                                litSound2.Text = String.Format("<iframe class=\"iframe_sound page_margin_top\" src=\"{0}\"></iframe><br>", r.Url);
                                txtTrans2.Text = "";
                                break;
                            case 3:
                                litDesc3.Text = r.Description;
                                litSound3.Text = String.Format("<iframe class=\"iframe_sound page_margin_top\" src=\"{0}\"></iframe><br>", r.Url);
                                txtTrans3.Text = "";
                                break;
                            case 4:
                                litDesc4.Text = r.Description;
                                litSound4.Text = String.Format("<iframe class=\"iframe_sound page_margin_top\" src=\"{0}\"></iframe><br>", r.Url);
                                txtTrans4.Text = "";
                                break;
                        }
                        count += 1;
                        //htmlBody += String.Format("<div id='{0}'>", r.Id);
                        //htmlBody += String.Format("<p>{0}</p>", r.Description);
                        //htmlBody += String.Format("<iframe class=\"iframe_sound page_margin_top\" src=\"{0}\"></iframe><br>", r.Url);
                        //htmlBody += String.Format("<br><input type='text' id='txt{0}' />", r.Id);
                        //htmlBody += "</div><br><br>";
                    }
                    //litSound.Text = htmlBody;
                }
            }
        }
    }
}
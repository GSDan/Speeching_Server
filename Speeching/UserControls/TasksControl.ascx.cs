using Crowd.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Crowd.Presenter;
using Speeching.Common;

namespace Speeching.UserControls
{
    public partial class TasksControl : System.Web.UI.UserControl, ITasksView
    {
        TasksPresenter _tasksPresenter;

        //private int _stageCount;
        private int _activityId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _activityId = SpeechingUtil.ParseInt(Request.QueryString["aid"]);//TODO: QueryString or Session?!
            if (_activityId <= 0)
                _activityId = 1;//This is a temporary id that we know its in the database

            _tasksPresenter = new TasksPresenter(this);
            _tasksPresenter.GetAllTasks();
        }

        public Dictionary<int, string> Status { get; set; }
        public IList<ITaskView> Tasks { get; set; }
    }
}
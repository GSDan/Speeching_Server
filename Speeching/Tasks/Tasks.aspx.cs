using Crowd.Presenter;
using Crowd.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Speeching.Tasks
{
    public partial class Tasks : System.Web.UI.Page, ISpeechingTaskView
    {
        SpeechingTaskPresenter _presenter;
        protected void Page_Load(object sender, EventArgs e)
        {
            _presenter = new SpeechingTaskPresenter(this);
            _presenter.GetAllTasks();
        }

        public string Message
        {
            set { throw new NotImplementedException(); }
        }
    }
}
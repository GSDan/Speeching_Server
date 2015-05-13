//using Crowd.Model;
using Crowd.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crowd.Presenter
{
    public class SpeechingTaskPresenter : BasePresenter
    {
        private ISpeechingTaskView _speechingTaskView; 

        public SpeechingTaskPresenter()
        {

        }
        public SpeechingTaskPresenter(ISpeechingTaskView view)
        {
            _speechingTaskView = view;
        }

        public void GetHello()
        {
            //TaskModel tmodel = new TaskModel();
            //_speechingTaskView.Message = tmodel.SayHello();
        }
    }
}

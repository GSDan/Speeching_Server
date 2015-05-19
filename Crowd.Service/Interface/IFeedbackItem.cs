using System;

namespace Crowd.Service.Interface
{
    public interface IFeedbackItem
    {
        int Id { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        DateTime Date { get; set; }
        bool Dismissable { get; set; }
        int Importance { get; set; }
    }
}

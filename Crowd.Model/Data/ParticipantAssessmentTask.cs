
namespace Crowd.Model.Data
{
    public class ParticipantAssessmentTask
    {
        public enum AssessmentTaskType { None, Loudness, Pacing, QuickFire, ImageDesc }

        public AssessmentTaskType TaskType { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ParticipantAssessmentTaskPromptCol PromptCol { get; set; }

        //Only used in image description
        public string Image { get; set; }
    }
}

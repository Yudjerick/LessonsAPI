using LessonsAPI.Models;
using LessonsAPI.ViewModels;

namespace LessonsAPI.RequestObjects
{
    public class TaskRequestObject
    {
        public string? Phrase { get; set; }
        public List<string>? Answer { get; set; }

        public List<string>? AdditionalVariants { get; set; }


        public static Models.Task ToModel(TaskRequestObject requestobject, Lesson lesson)
        {

            Models.Task model = new Models.Task();
            model.Phrase = requestobject.Phrase;
            model.Answer = requestobject.Answer;
            model.AdditionalVariants = requestobject.AdditionalVariants;
            model.Lesson = lesson;
            return model;
        }
    }
}

using LessonsAPI.Models;
using LessonsAPI.ViewModels;

namespace LessonsAPI.RequestObjects
{
    public class LessonRequestObject
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Topic { get; set; }

        public List<TaskRequestObject> Tasks { get; set; }

        public static Lesson ToModel(LessonRequestObject requestObject, Author author, long id)
        {
            var lesson = new Lesson();
            lesson.Id = id;
            lesson.Title = requestObject.Title;
            lesson.Description = requestObject.Description;
            lesson.Topic = requestObject.Topic;
            lesson.Author = author;
            lesson.Tasks = requestObject.Tasks.Select(x => TaskRequestObject.ToModel(x, lesson)).ToList();
            return lesson;
        }
    }
}

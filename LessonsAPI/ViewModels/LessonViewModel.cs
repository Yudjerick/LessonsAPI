using LessonsAPI.Data;
using LessonsAPI.Models;
using Task = LessonsAPI.Models.Task;

namespace LessonsAPI.ViewModels
{
    public class LessonViewModel
    {
        
        public long? Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? Topic { get; set; }

        public long AuthorId { get; set; }

        public List<TaskViewModel> Tasks { get; set; }

        public static LessonViewModel ToViewModel(Lesson lesson)
        {
            LessonViewModel lessonViewModel = new LessonViewModel();
            lessonViewModel.Id = lesson.Id;
            lessonViewModel.Title = lesson.Title;
            lessonViewModel.Topic = lesson.Topic;
            lessonViewModel.Description = lesson.Description;
            lessonViewModel.Tasks = lesson.Tasks.Select(x => TaskViewModel.ToViewModel(x)).ToList();

            lessonViewModel.AuthorId = lesson.Author.Id;
            return lessonViewModel;
        }

        public static Lesson ToModel(LessonViewModel lessonViewModel, LessonsAPIContext context)
        {
            Lesson lesson = new Lesson();
            lesson.Description = lessonViewModel.Description;
            lesson.Title = lessonViewModel.Title;
            lesson.Topic = lessonViewModel.Topic;
            lesson.Tasks = lessonViewModel.Tasks.Select(x => TaskViewModel.ToModel(x)).ToList();
            try
            {
                lesson.Author = context.Authors.First(x => x.Id == lessonViewModel.AuthorId);
            }
            catch
            {
                throw new Exception($"Author with id '{lessonViewModel.Id}' does not exixst");
            }
            return lesson;
        }
    }
}

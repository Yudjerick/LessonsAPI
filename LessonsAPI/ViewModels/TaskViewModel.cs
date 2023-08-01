using LessonsAPI.Models;

namespace LessonsAPI.ViewModels
{
    public class TaskViewModel
    {
        public long? Id { get; set; }
        public string? Phrase { get; set; }
        public List<string>? Answer { get; set; }

        public List<string>? AdditionalVariants { get; set; }

        public static TaskViewModel ToViewModel(Models.Task task)
        {
            TaskViewModel viewModel = new TaskViewModel();
            viewModel.Id = task.Id;
            viewModel.Phrase = task.Phrase;
            viewModel.Answer = task.Answer;
            viewModel.AdditionalVariants = task.AdditionalVariants;
            return viewModel;
        }
         
        public static Models.Task ToModel(TaskViewModel taskViewModel)
        {
            
            Models.Task model = new Models.Task();
            model.Phrase = taskViewModel.Phrase;
            model.Answer = taskViewModel.Answer;
            model.AdditionalVariants = taskViewModel.AdditionalVariants;
            return model;
        }
    }
}

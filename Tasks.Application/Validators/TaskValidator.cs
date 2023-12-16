using FluentValidation;
using Tasks.Application.Repositories;
using Task = Tasks.Application.Models.Task;

namespace Tasks.Application.Validators;

public class TaskValidator : AbstractValidator<Task>
{
    private readonly ITaskRepository _taskRepository;

    public TaskValidator(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Tags)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Status)
            .NotEmpty();


        RuleFor(x => x.DueDate)
            .NotEmpty();


        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This task already exists in the system");
    }

    private async Task<bool> ValidateSlug(Task task, string slug, CancellationToken token = default)
    {
        var existingTask = await _taskRepository.GetBySlugAsync(slug);

        if (existingTask is not null)
        {
            return existingTask.Id == task.Id;
        }

        return existingTask is null;
    }
}
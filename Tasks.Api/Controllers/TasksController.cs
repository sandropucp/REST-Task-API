using Microsoft.AspNetCore.Mvc;
using Tasks.Api.Mapping;
using Tasks.Application.Services;
using Tasks.Contracts.Requests;

namespace Tasks.Api.Controllers;

[ApiController]
public class TasksController: ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpPost(ApiEndpoints.Tasks.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request,
        CancellationToken token)
    {
        var task = request.MapToTask();
        await _taskService.CreateAsync(task, token);
        var taskResponse = task.MapToResponse();
        return CreatedAtAction(nameof(Get), new { idOrSlug = task.Id }, taskResponse);
    }

    [HttpGet(ApiEndpoints.Tasks.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug,
        CancellationToken token)
    {
        var task = Guid.TryParse(idOrSlug, out var id)
            ? await _taskService.GetByIdAsync(id, token)
            : await _taskService.GetBySlugAsync(idOrSlug, token);
        if (task is null)
        {
            return NotFound();
        }

        var response = task.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Tasks.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var tasks = await _taskService.GetAllAsync(token);

        var tasksResponse = tasks.MapToResponse();
        return Ok(tasksResponse);
    }

    [HttpPut(ApiEndpoints.Tasks.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateTaskRequest request,
        CancellationToken token)
    {
        var task = request.MapToTask(id);
        var updatedTask = await _taskService.UpdateAsync(task, token);
        if (updatedTask is null)
        {
            return NotFound();
        }

        var response = updatedTask.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Tasks.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id,
        CancellationToken token)
    {
        var deleted = await _taskService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}

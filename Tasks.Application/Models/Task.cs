using System.Text.RegularExpressions;

namespace Tasks.Application.Models;

public partial class Task
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; }
    public required DateTime DueDate { get; set; }
    public required List<string> Tags { get; init; } = new();
    public string Slug => GenerateSlug();
    private string GenerateSlug()
    {
        var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
            .ToLower().Replace(" ", "-");
        return $"{sluggedTitle}-{DueDate.Year}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}
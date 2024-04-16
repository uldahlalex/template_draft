namespace IndependentHelpers.Domain;

public class TodoWithTags
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Priority { get; set; }
    public int UserId { get; set; }
    public List<Tag> Tags { get; set; } = [];
}
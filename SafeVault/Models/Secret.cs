namespace SafeVault.Models;

public class Secret
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public string Content { get; set; } = string.Empty;
}

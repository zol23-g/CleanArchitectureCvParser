namespace Core.Events
{
    public interface IResumeParsed
    {
        int ResumeId { get; }
        string Name { get; }
        string Email { get; }
    }
}

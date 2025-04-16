namespace Core.Events
{
    public interface IResumeParsed
    {
        int ResumeId { get; }
        string Email { get; }
    }
}

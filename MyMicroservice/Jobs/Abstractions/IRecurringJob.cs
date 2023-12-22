namespace MyMicroservice.Jobs.Abstractions
{
    public interface IRecurringJob
    {
        Task ExecuteAsync();
    }
}

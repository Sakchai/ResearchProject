using System.Threading.Tasks;

namespace Research.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}

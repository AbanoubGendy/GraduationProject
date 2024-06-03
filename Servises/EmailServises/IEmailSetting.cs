using System.Threading.Tasks;

namespace Graduation.PL.Servises.EmailServises
{
    public interface IEmailSetting
    {
        Task SendEmailAsyn(string to, string Subject, string body);
    }
}

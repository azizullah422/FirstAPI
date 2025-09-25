namespace FirstAPI.Services;

    public interface IAppEmailSender
    {
        Task SendAsync(string to,string subject, string htmlbody);
    }


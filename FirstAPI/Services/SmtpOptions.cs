namespace FirstAPI.Services;

    public sealed class SmtpOptions
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string User { get; set; } = "";
        public string Pass { get; set; } = "";
        public string From { get; set; } = "";
    }



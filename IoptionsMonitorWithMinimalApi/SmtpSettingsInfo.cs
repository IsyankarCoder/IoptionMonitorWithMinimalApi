public class SmtpSettings
{
    public static readonly string SectionName ="SmtpSettings";
    public string Host { get; set; } = "";
    public int Port { get; set; } = 586;

    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

}
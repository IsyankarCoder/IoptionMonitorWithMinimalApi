using Microsoft.Extensions.Options;

public class EmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
         _smtpSettings=smtpSettings.Value;
        
    }

    public void SendEmail(string To,string obj,string body)
    {
        Console.WriteLine($"Sending Email via {_smtpSettings.Host} : {_smtpSettings.Port}");
    }
     
}
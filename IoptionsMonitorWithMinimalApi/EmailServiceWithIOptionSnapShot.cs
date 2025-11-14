using Microsoft.Extensions.Options;

public class EmailServiceWithIOptionSnapShot
{
    private readonly SmtpSettings _smtpSettings;

    public EmailServiceWithIOptionSnapShot(IOptionsSnapshot<SmtpSettings> smtpSettings)
    {
         _smtpSettings=smtpSettings.Value;
        
    }

    public void SendEmail(string To,string obj,string body)
    {
        Console.WriteLine($"Sending Email via IOptionsSnapshot  {_smtpSettings.Host} : {_smtpSettings.Port}");
    }
     
}
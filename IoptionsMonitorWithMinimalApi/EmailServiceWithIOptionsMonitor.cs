using Microsoft.Extensions.Options;
using System;

public class EmailServiceIoptionMonitor:IDisposable
{
     private SmtpSettings _smtpSettings;
    private readonly IDisposable? _onChangeToken;

    public EmailServiceIoptionMonitor(IOptionsMonitor<SmtpSettings> smtpSettings)
    {
         _smtpSettings = smtpSettings.CurrentValue;

        // OnChange'in döndürdüğü IDisposable'ı saklıyoruz
        IDisposable? disposable = smtpSettings.OnChange(updated =>
        {
            Console.WriteLine($"Settings Updated Port {updated.Port}");
            _smtpSettings = updated;
        });
        
        _onChangeToken = disposable;
    }

    public void SendEmail(string To,string obj,string body)
    {
        Console.WriteLine($"Sending Email via IOptionsMonitor {_smtpSettings.Host} : {_smtpSettings.Port}");
    }

    public void Dispose()
    {
        _onChangeToken?.Dispose();
    }
     
}
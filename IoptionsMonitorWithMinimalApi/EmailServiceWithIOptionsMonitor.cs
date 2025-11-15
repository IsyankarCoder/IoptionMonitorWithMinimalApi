using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;

public class EmailServiceIoptionMonitor : IDisposable
{
    private SmtpSettings _smtpSettings;
    private static IDisposable? _onChangeToken;
    private static IDisposable? _configReloadRegistration;
    private static readonly object _lockObject = new object();
    private static bool _isInitialized = false;

    public EmailServiceIoptionMonitor(IOptionsMonitor<SmtpSettings> smtpSettings, IConfiguration configuration)
    {
        _smtpSettings = smtpSettings.CurrentValue;
        Console.WriteLine($"[ctor] Instance {GetHashCode()} created. Port={_smtpSettings?.Port}");

        lock (_lockObject)
        {
            if (!_isInitialized)
            {
                _onChangeToken = smtpSettings.OnChange(updated =>
                {
                    Console.WriteLine($"[OnChange-OptionsMonitor] Instance {GetHashCode()} - Settings Updated Port {updated.Port}");
                    _smtpSettings = updated;
                    Console.WriteLine($"[OnChange-OptionsMonitor] StackTrace: {Environment.StackTrace}");
                });

                _configReloadRegistration = ChangeToken.OnChange(
                    () => configuration.GetReloadToken(),
                    () => Console.WriteLine($"[ConfigReloadToken] fired at {DateTime.Now:O} - Instance {GetHashCode()}")
                );

                _isInitialized = true;
                Console.WriteLine("[ctor] Subscriptions registered.");
            }
            else
            {
                Console.WriteLine("[ctor] Already initialized - no new subscription.");
            }
        }
    }

    public void SendEmail(string To, string obj, string body)
    {
        Console.WriteLine($"SendEmail Instance {GetHashCode()} via {_smtpSettings.Host}:{_smtpSettings.Port}");
    }

    public void Dispose()
    {
        lock (_lockObject)
        {
            Console.WriteLine($"[Dispose] Instance {GetHashCode()} disposing subscriptions.");
            _onChangeToken?.Dispose();
            _onChangeToken = null;
            _configReloadRegistration?.Dispose();
            _configReloadRegistration = null;
            _isInitialized = false;
        }
    }
}
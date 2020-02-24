using System;
using System.Threading.Tasks;
using RingCentral;

namespace WebHookNotifications
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var rc = new RestClient(
                Environment.GetEnvironmentVariable("RINGCENTRAL_CLIENT_ID"),
                Environment.GetEnvironmentVariable("RINGCENTRAL_CLIENT_SECRET"),
                Environment.GetEnvironmentVariable("RINGCENTRAL_SERVER_URL")
            ))
            {
                Task.Run(async () =>
                {
                    await rc.Authorize(
                        Environment.GetEnvironmentVariable("RINGCENTRAL_USERNAME"),
                        Environment.GetEnvironmentVariable("RINGCENTRAL_EXTENSION"),
                        Environment.GetEnvironmentVariable("RINGCENTRAL_PASSWORD")
                    );

                    var eventFilters = new[]
                    {
                        "/restapi/v1.0/account/~/telephony/sessions"
                    };
                    
                    var subscriptionInfo = await rc.Restapi().Subscription().Post(new CreateSubscriptionRequest
                    {
                        eventFilters = eventFilters,
                        deliveryMode = new NotificationDeliveryModeRequest
                        {
                            transportType = "WebHook",
                            address = "https://4c902816.ngrok.io/webhook"
                        }
                    });
                    
                    Console.WriteLine(subscriptionInfo.status);
                }).GetAwaiter().GetResult();
            }
        }
    }
}
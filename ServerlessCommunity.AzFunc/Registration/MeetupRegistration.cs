using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using ServerlessCommunity.Config;

namespace ServerlessCommunity.AzFunc.Registration
{
    public static class MeetupRegistration
    {
        [FunctionName(nameof(MeetupRegistration))]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "meetup-registration")]HttpRequest req,
            
            [Table(TableName.Registration)] CloudTable registrationTable,
            
            ILogger log)
        {
            dynamic inputForm;
            using (var reader = new StreamReader(req.Body))
            {
                inputForm = JsonConvert.DeserializeObject(await reader.ReadToEndAsync());
            }
            
            var registration = new Data.AzStorage.Table.Model.Registration();
            registration.Id = Guid.NewGuid();
            
            registration.Email = inputForm.Email;
            registration.IsSubscribed = inputForm.IsSubscribeForNews;
            registration.MeetupId = Guid.Parse(inputForm.MeetupId.ToString());
            registration.FirstName = inputForm.FirstName;
            registration.LastName = inputForm.LastName;

            registration.IsConfirmed = true;

            await registrationTable.ExecuteAsync(TableOperation.InsertOrMerge(registration));
        }
    }
}
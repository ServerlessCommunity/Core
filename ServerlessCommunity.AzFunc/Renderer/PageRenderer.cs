using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Nustache.Core;
using ServerlessCommunity.Application.Command.Render;
using ServerlessCommunity.Config;
using ZetaProducerHtmlCompressor.Internal;

namespace ServerlessCommunity.AzFunc.Renderer
{
    public static class PageRenderer
    {
        [FunctionName(nameof(PageRenderer))]
        public static async Task RenderPageFunction(
            [QueueTrigger(QueueName.RenderPage)]RenderPage command,
            
            [Blob(ContainerName.PageTemplate + "/{" + nameof(RenderPage.TemplateId) + "}", FileAccess.Read)]CloudBlockBlob pageTemplateBlob,
            [Blob(ContainerName.PageData + "/{" + nameof(RenderPage.DataInstanceId) + "}", FileAccess.ReadWrite)]CloudBlockBlob pageDataBlob,
            
            [Blob(ContainerName.WebHost + "/{" + nameof(RenderPage.PublicUrl) + "}", FileAccess.Write)]CloudBlockBlob pageHtmlBlob,
            
            ILogger log)
        {
            log.LogInformation(JsonConvert.SerializeObject(command));

            var pageData = JsonConvert.DeserializeObject(await pageDataBlob.DownloadTextAsync());
            var pageTemplate = await pageTemplateBlob.DownloadTextAsync();
            
            var html = Render.StringToString(pageTemplate, pageData, new RenderContextBehaviour
            {
                HtmlEncoder = text => text
            });

            var compressor = new HtmlCompressor();
            compressor.setEnabled(true);
            compressor.setRemoveComments(true);
            compressor.setRemoveMultiSpaces(true);
            compressor.setRemoveIntertagSpaces(true);

            html = compressor.compress(html);

            await pageHtmlBlob.UploadTextAsync(html);
            
            pageHtmlBlob.Properties.ContentType = "text/html";
            await pageHtmlBlob.SetPropertiesAsync();
            
            await pageDataBlob.DeleteAsync();
        }
    }
}
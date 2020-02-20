using System;
using System.Threading.Tasks;
using IronPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pdf
{
    [ApiExplorerSettings(IgnoreApi = true)]
    // [Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
    [Route("[controller]/[action]")]
    [Authorize]
    public class PdfController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> UrlToPdf([FromQuery] string url)
        {
            try
            {

                if (!Uri.IsWellFormedUriString(url, UriKind.Relative))
                {
                    throw new Exception("Invalid URI");
                }
                url = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}{url}";
                var uri = new Uri(url);
                var filename = "filename.pdf";
                // Create a PDF from a web page  
                var Renderer = new HtmlToPdf();
                var PDF = await Renderer.RenderUrlAsPdfAsync(uri);
                var contentType = "application/pdf";
                return File(PDF.BinaryData, contentType, filename);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }

}

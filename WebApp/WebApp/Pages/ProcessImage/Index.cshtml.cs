using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Pages.ProcessImage
{
    public class IndexModel : PageModel
    {
        private readonly IDistributionImageService _distributionImageService;

        private readonly IScaleService _scaleService;

        private readonly IHostingEnvironment _hostingEnvironment;

        public IndexModel(IDistributionImageService distributionImageService, IScaleService scaleService, IHostingEnvironment environment)
        {
            _distributionImageService = distributionImageService;
            _scaleService = scaleService;
            _hostingEnvironment = environment;
        }

        [BindProperty]
        public InputData InputData { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public void GetProcessedImage(int deviceId)
        {
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _distributionImageService.SetImageAndParts(new Bitmap($"c:\\images\\{InputData.Path}"), InputData.CountOfServes);
            _scaleService.SetnumberOfSocketsAndBeginPort(InputData.CountOfServes);
            var chunksOfImage = _distributionImageService.CreateParallelData();
            _scaleService.CreateScale(chunksOfImage);
            _scaleService.Connect();
            _distributionImageService.SendParallelData(_scaleService.Sockets);
            _distributionImageService.ConcatImage(_scaleService.Sockets);
            InputData.ProcessedFile = _distributionImageService.SaveResultImage();
            _scaleService.CloseConnections();
            
            var url = Url.Page("Result", new { name = "Tom", age = 34 });
            return RedirectToPage(url);
        }
    }
}

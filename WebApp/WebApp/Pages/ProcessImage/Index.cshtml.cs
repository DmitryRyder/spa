using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;
using System.Threading.Tasks;
using WebApp.Services;
using WebApp.ViewModels;

namespace WebApp.Pages.ProcessImage
{
    public class IndexModel : PageModel
    {
        private readonly IDistributionImageService _distributionImageService;

        private readonly IScaleService _scaleService;

        public IndexModel(IDistributionImageService distributionImageService, IScaleService scaleService)
        {
            _distributionImageService = distributionImageService;
            _scaleService = scaleService;
        }

        [BindProperty]
        public InputData InputData { get; set; }

        public async Task<IActionResult> OnGet()
        {
            //InputData = await _remoOneApiService.GetDataByDeviceId(1);

            return Page();
        }

        public async Task OnPostGetDataByDeviceNumber(int deviceId)
        {
            //InputData = await _remoOneApiService.GetDataByDeviceId(deviceId);
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
            _distributionImageService.SaveResultImage();
            _scaleService.CloseConnections();

            return RedirectToPage("./Index");
        }
    }
}

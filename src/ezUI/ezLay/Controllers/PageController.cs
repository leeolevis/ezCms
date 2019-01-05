using Microsoft.AspNetCore.Mvc;

namespace ezLay.Controllers
{
    public class PageController : Controller
    {
        //做任意名称的单页面时使用，避免新增页面时需要改代码
        //例如 Page/Helper  Page/App   Page/About
        [Route("/Page/{Name}")]
        public IActionResult Index(string Name)
        {
            return View(Name, "");
        }
    }
}

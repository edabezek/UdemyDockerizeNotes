using AspNetCoreMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IFileProvider _fileProvider;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IFileProvider fileProvider, IConfiguration configuration)
        {
            _logger = logger;
            _fileProvider= fileProvider;
            _configuration= configuration;  
        }

        public IActionResult Index()
        {
            ViewBag.MySqlCon = _configuration["MySqlCon"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ImageSave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImageSave(IFormFile imageFile)
        {
            if (imageFile!=null && imageFile.Length>0)
            {
                //resim.jpeg geldiğinde , rastgele isim üretip , uzantısını-jpeg- ekleyeceğiz.
                //isim oluşturma sebebimiz, kullanıcı aynı adla iki isim yuklediğinde eskisi silinir yeni eklenir, bunu önlemek için rasgele bir isim veriyoruz.
                var fileName =Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName); 
                //kaydedilecek kısmı veriyoruz : 
                var path= Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/Images" ,fileName);

                using (var stream=new FileStream(path,FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
            }
            return View();
        }

        public IActionResult ImageShow()
        {
            //bu klasördeki images ın isimlerini liste olarak döner
            var images = _fileProvider.GetDirectoryContents("wwwroot/Images").ToList().Select(x=>x.Name);

            return View(images);    
        }


        [HttpPost]
        public IActionResult ImageShow(string name)
        {
            var file = _fileProvider.GetDirectoryContents("wwwroot/Images").ToList().First(x => x.Name == name);

            System.IO.File.Delete(file.PhysicalPath);
            return RedirectToAction("ImageShow");

        }












        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

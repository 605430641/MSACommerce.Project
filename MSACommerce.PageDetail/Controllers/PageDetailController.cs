using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSACommerce.Interface;
using MSACommerce.Model;

namespace MSACommerce.PageDetail.Controllers
{
    public class PageDetailController : Controller
    {
        private IPageDetailService _pageDetailService;
        public PageDetailController(IPageDetailService pageDetailService)
        {
            _pageDetailService = pageDetailService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">spuid</param>
        /// <returns></returns>
		[Route("/item/{id}.html")]
        public IActionResult Index(long id)
        {
            var htmlmodel = _pageDetailService.loadModel(id);

            return View(htmlmodel);
        }
    }
}

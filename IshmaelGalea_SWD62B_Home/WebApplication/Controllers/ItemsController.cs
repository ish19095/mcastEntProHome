using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.ViewModels;
using BusinessLogic.Services;

namespace WebApplication.Controllers
{
    // Design Patterns - Creational, Behavioral, Structural
    // Dependancy Injection - Is about centralizing  and therefore  better  management of the (creation of) instances
    // 1 variation is Constructor Injection
    public class ItemsController : Controller
    {
        private ItemsService itemsService { get; set; }
        public ItemsController(ItemsService _itemsService)
        {
            itemsService = _itemsService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            //do we need  to process anything here the first time the user requests the create-an-item page?
            //NO
            return View();
        }

        [HttpPost]
        public IActionResult Create(ItemView data)
        {
            //1. implement the create method by
            //2. applying dependancy injection and ask for ItemService
            //3. itemsService.AddNewItem(...)
            //
            return View();

            try
            {
                //1. implement the create method by
                //2. applying dependancy injection and ask for ItemService
                //3. itemsService.AddNewItem(...)
                itemsService.AddNewItem(data.Name, data.Price, data.CategoryId, data.Stock, data.ImagePath);
                ViewBag.Message = "Item Added Successfully";
            }
            catch (Exception ex)
            {
                // ViewBag: a dynamic  object, it allows you to decalre properties on the fly
                // log the exception
                ViewBag.Error = "There"
            }
        }
    }
}

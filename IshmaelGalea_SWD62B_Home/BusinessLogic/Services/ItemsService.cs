using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Repositories;
using System.Linq;

namespace BusinessLogic.Services
{
    // the centralization of creation of instances implies a more efficient management of objects
    // i.e. we have to use the Design Pattern
    // Design Pattern : Dependancy Injection - a variation of this is Constructor Injection
    public class ItemsService
    {
        private ItemsRepos itemsRepos;
        public ItemsService(ItemsRepos _itemsRepos)
        {
            itemsRepos = _itemsRepos;
        }

        public void AddNewItem(string name, double price, int categoryId, int stock = 0, string imagePath=null)
        {
            if(itemsRepos.GetItems().Any(x=>x.Name==name))
            {
                throw new Exception("Item with the same name already exists");
            }
            itemsRepos.AddItem(new Domain.Models.Item()
            {
                CategoryId = categoryId,
                ImagePath = imagePath,
                Name = name,
                Price = price,
                Stock = stock
            });
       
        }

        public void DeleteItem(int id)
        {

        }
    }
}

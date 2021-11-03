using CRUD.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD.Entities.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ColorEnum Color { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}

using InfrastructorLayer.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dtos.CategoryDtos
{
    public class CategoryPostDto:InterfaceDataTransferObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Picture { get; set; }  
        public string PhotoPath { get; set; }

    }
}

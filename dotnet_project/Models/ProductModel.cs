﻿using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "Please enter the product name.")]
        public string Name { get; set; }

        [Required, MinLength(4, ErrorMessage = "Please enter the description.")]
        public string Description { get; set; }
        public string Slug { get; set; }

        [Required, MinLength(4, ErrorMessage = "Please enter the product price.")]
        public decimal Price { get; set; }

        public string Image {  get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }
        public BrandModel Brand { get; set; }
    }
}

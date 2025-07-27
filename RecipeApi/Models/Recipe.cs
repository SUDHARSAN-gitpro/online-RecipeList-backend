// File: Models/Recipe.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<RecipeStep> Steps { get; set; } = new List<RecipeStep>();
    }
}

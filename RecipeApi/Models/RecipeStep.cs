// File: Models/RecipeStep.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RecipeApi.Models
{
    public class RecipeStep
    {
        [Key]
        public int RecipeStepId { get; set; }

        [Required]
        public string Instruction { get; set; } = string.Empty;

        public int RecipeId { get; set; }

        [ForeignKey("RecipeId")]
        [JsonIgnore] // Prevents cycles when serializing!
        public Recipe? Recipe { get; set; }
    }
}

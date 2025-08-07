using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/recipes
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _context.Recipes
                .Include(r => r.Steps)
                .ToListAsync();

            return Ok(recipes);
        }

        // GET: api/recipes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe == null)
                return NotFound();

            return Ok(recipe);
        }

        // POST: api/recipes
        [HttpPost]
        public async Task<IActionResult> CreateRecipe(Recipe recipe)
        {
            if (recipe.Steps != null)
            {
                foreach (var step in recipe.Steps)
                {
                    step.Recipe = recipe; // helps EF tracking
                }
            }

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.RecipeId }, recipe);
        }

        // PUT: api/recipes/{id}
        // Correctly updates Recipe and synchronizes Steps collection
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, Recipe updatedRecipe)
        {
            if (id != updatedRecipe.RecipeId)
                return BadRequest();

            var existingRecipe = await _context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (existingRecipe == null)
                return NotFound();

            // Update main properties
            existingRecipe.Name = updatedRecipe.Name;

            // Sync Steps: remove deleted steps
            var updatedStepIds = updatedRecipe.Steps?.Select(s => s.RecipeStepId).ToList() ?? new List<int>();

            var stepsToRemove = existingRecipe.Steps
                .Where(s => !updatedStepIds.Contains(s.RecipeStepId))
                .ToList();

            foreach (var step in stepsToRemove)
            {
                _context.RecipeSteps.Remove(step);
            }

            // Add or update steps
            if (updatedRecipe.Steps != null)
            {
                foreach (var updatedStep in updatedRecipe.Steps)
                {
                    var existingStep = existingRecipe.Steps
                        .FirstOrDefault(s => s.RecipeStepId == updatedStep.RecipeStepId);

                    if (existingStep != null)
                    {
                        // Update existing step
                        existingStep.Instruction = updatedStep.Instruction;
                    }
                    else
                    {
                        // Add new step
                        var newStep = new RecipeStep
                        {
                            Instruction = updatedStep.Instruction,
                            RecipeId = existingRecipe.RecipeId
                        };
                        existingRecipe.Steps.Add(newStep);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Recipes.Any(e => e.RecipeId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/recipes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe == null)
                return NotFound();

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/recipes/steps/{stepId}
        [HttpPut("steps/{stepId}")]
        public async Task<IActionResult> UpdateRecipeStep(int stepId, RecipeStep updatedStep)
        {
            if (stepId != updatedStep.RecipeStepId)
                return BadRequest();

            _context.Entry(updatedStep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.RecipeSteps.Any(s => s.RecipeStepId == stepId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/recipes/steps/{stepId}
        [HttpDelete("steps/{stepId}")]
        public async Task<IActionResult> DeleteRecipeStep(int stepId)
        {
            var step = await _context.RecipeSteps.FindAsync(stepId);

            if (step == null)
                return NotFound();

            _context.RecipeSteps.Remove(step);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TodoWebApi.Models;

namespace TodoWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private static List<TodoItem> todos = new List<TodoItem>();
        private static int nextId = 1;

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = todos.FirstOrDefault(t => t.Id == id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public IActionResult Add([FromBody] TodoItem newItem)
        {
            newItem.Id = nextId++;
            todos.Add(newItem);
            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TodoItem updatedItem)
        {
            var existing = todos.FirstOrDefault(t => t.Id == id);
            if (existing == null) return NotFound();

            existing.Title = updatedItem.Title;
            existing.IsComplete = updatedItem.IsComplete;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = todos.FirstOrDefault(t => t.Id == id);
            if (item == null) return NotFound();

            todos.Remove(item);
            return NoContent();
        }
    }
}

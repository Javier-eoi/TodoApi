using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Model;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class ToDoController : Controller
    {
        private readonly TodoContext _context;


        public ToDoController(TodoContext context)
        {
            _context = context;

            if (_context.Items.Count() == 0)
            {
                _context.Items.Add(new ToDoItem {
                    Name = "Estudiar en casa",
                });

                _context.Items.Add(new ToDoItem {
                    Name = "Hacer ejercicios en casa",
                });

                _context.SaveChanges();
            }
        }

        [HttpGet]//con este atributo decimos que esto se usara para el metodo get
        public IEnumerable<ToDoItem> Get()
        {
            return _context.Items;

        }

        [HttpGet("{id}", Name = "GetTodoItem")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Items.SingleOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ToDoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            // return Ok(item);
            return CreatedAtRoute("GetTodoItem", new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Items.SingleOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            //  return NoContent();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody] ToDoItem item)
        {
            if (!ModelState.IsValid || id != item.Id)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Items.Any(x=> x.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();

        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Model;

namespace ToDoAPI.Controllers
{
    [Route("api/ToDoItems")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ToDoContext _context;
        public ToDoItemsController(IMapper mapper, ToDoContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ToDoItems
        [HttpGet]
        public async Task<ActionResult> GetToDoItems()
        {
            ToDoItemDTO toDoItemDTO = new();
            var toDoItem = _mapper.Map<ToDoItem>(toDoItemDTO);
            return Ok(await _context.ToDoItems.Select(toDoItemDTO => ItemToDTO(toDoItemDTO)).ToListAsync());
            //return await _context.ToDoItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/ToDoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItemDTO>> GetToDoItem(long id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);

            if (toDoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(toDoItem);
        }

        // PUT: api/ToDoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoItem(long id, ToDoItemDTO toDoItemDTO)
        {
            if (id != toDoItemDTO.Id)
            {
                return BadRequest();
            }
            var todoItem = await _context.ToDoItems.FindAsync(id);
            if(todoItem == null)
            {
                return NotFound();
            }
            todoItem.Name = toDoItemDTO.Name;
            todoItem.IsComplete = toDoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ToDoItemExists(id))
                {
                    return NotFound();
                }
            return NoContent();
        }

        // POST: api/ToDoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoItemDTO>> CreateToDoItem(ToDoItemDTO toDoItemDTO)
        {
            var todoItem = new ToDoItem
            {
                IsComplete = toDoItemDTO.IsComplete,
                Name = toDoItemDTO.Name
            };
            _context.ToDoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetToDoItem", new { id = toDoItem.Id }, toDoItem);
            return CreatedAtAction(nameof(GetToDoItem), new { id = todoItem.Id }, ItemToDTO(todoItem));
        }
        // DELETE: api/ToDoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItem(long id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            _context.ToDoItems.Remove(toDoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoItemExists(long id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }
        private static ToDoItemDTO ItemToDTO(ToDoItem todoItem) => new()
        {
            Id = todoItem.Id,
            Name = todoItem.Name,
            IsComplete = todoItem.IsComplete
        };
    }
}

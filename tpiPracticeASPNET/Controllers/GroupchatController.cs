using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tpiPracticeASPNET;
using tpiPracticeASPNET.Hubs;
using tpiPracticeClasses;

namespace tpiPracticeASPNET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupchatController : ControllerBase
    {
        private readonly TpiDB _context;

        public GroupchatController(TpiDB context)
        {
            _context = context;
        }

        // GET: api/Groupchat
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Groupchat>>> GetGroupchats()
        {
            return await _context.Groupchats.ToListAsync();
        }

        // GET: api/Groupchat/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Groupchat>> GetGroupchat(int id)
        {
            var groupchat = await _context.Groupchats.FindAsync(id);

            if (groupchat == null)
            {
                return NotFound();
            }

            return groupchat;
        }

        // PUT: api/Groupchat/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupchat(int id, Groupchat groupchat)
        {
            if (id != groupchat.GroupchatId)
            {
                return BadRequest();
            }

            _context.Entry(groupchat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupchatExists(id))
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

        // POST: api/Groupchat
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Groupchat>> PostGroupchat(Groupchat groupchat)
        {
            _context.Groupchats.Add(groupchat);
            await _context.SaveChangesAsync();



            return CreatedAtAction("GetGroupchat", new { id = groupchat.GroupchatId }, groupchat);
        }

        // DELETE: api/Groupchat/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupchat(int id)
        {
            var groupchat = await _context.Groupchats.FindAsync(id);
            if (groupchat == null)
            {
                return NotFound();
            }

            _context.Groupchats.Remove(groupchat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupchatExists(int id)
        {
            return _context.Groupchats.Any(e => e.GroupchatId == id);
        }
    }
}

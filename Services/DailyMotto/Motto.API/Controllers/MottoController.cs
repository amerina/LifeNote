using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Motto.API.Infrastructure;
using Motto.API.Model;
using Polly;
using System.Net;
using System.Runtime;
using System.Security.Cryptography;

namespace Motto.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MottoController : ControllerBase
    {
        private readonly MottoContext _mottoContext;
        public MottoController(MottoContext context)
        {
            _mottoContext = context ?? throw new ArgumentNullException(nameof(context));

            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<MottoItem>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<MottoItem>>> ItemsAsync([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var itemsOnPage = await _mottoContext.MottoItems
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            return Ok(itemsOnPage);
        }

        [HttpGet]
        [Route("items/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MottoItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MottoItem>> ItemByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _mottoContext.MottoItems.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        [HttpGet]
        [Route("items/randomitem")]
        [ProducesResponseType(typeof(MottoItem), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<MottoItem>> ItemByRandomAsync()
        {
            return await Policy.HandleResult<MottoItem>(x => x == null).RetryAsync(5).ExecuteAsync(async () =>
            {
                var totalItems = await _mottoContext.MottoItems.CountAsync();

                var randomId = RandomNumberGenerator.GetInt32(totalItems);

                var item = await _mottoContext.MottoItems.FirstOrDefaultAsync(o => o.Id == randomId);

                return item;
            });
        }

        //POST api/v1/[controller]/items
        [Route("items")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateAsync([FromBody] MottoItem item)
        {
            _mottoContext.MottoItems.Add(new MottoItem(item.MottoTypeId, item.MottoLanguageId, item.Author, item.Content));

            await _mottoContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = item.Id }, null);
        }

        //DELETE api/v1/[controller]/id
        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var item = _mottoContext.MottoItems.SingleOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            _mottoContext.MottoItems.Remove(item);

            await _mottoContext.SaveChangesAsync();

            return NoContent();
        }

        //PUT api/v1/[controller]/items
        [Route("items")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateProductAsync([FromBody] MottoItem itemToUpdate)
        {
            var item = await _mottoContext.MottoItems.SingleOrDefaultAsync(i => i.Id == itemToUpdate.Id);

            if (item == null)
            {
                return NotFound(new { Message = $"Item with id {itemToUpdate.Id} not found." });
            }

            item = itemToUpdate;
            _mottoContext.MottoItems.Update(item);

            await _mottoContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ItemByIdAsync), new { id = itemToUpdate.Id }, null);
        }

        [HttpGet]
        [Route("mottolanguages")]
        [ProducesResponseType(typeof(List<MottoLanguage>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<MottoLanguage>>> MottoLanguagesAsync()
        {
            return await _mottoContext.MottoLanguages.ToListAsync();
        }


        [HttpGet]
        [Route("mottotypes")]
        [ProducesResponseType(typeof(List<MottoType>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<MottoType>>> MottoTypesAsync()
        {
            return await _mottoContext.MottoTypes.ToListAsync();
        }
    }
}

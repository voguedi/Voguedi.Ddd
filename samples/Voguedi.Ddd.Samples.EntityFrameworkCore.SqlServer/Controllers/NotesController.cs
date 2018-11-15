using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Application.DataObjects;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Application.Services;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Events;
using Voguedi.Events;

namespace Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        #region Private Fields

        readonly INoteService service;
        readonly IEventPublisher eventPublisher;

        #endregion

        #region Ctors

        public NotesController(INoteService service, IEventPublisher eventPublisher)
        {
            this.service = service;
            this.eventPublisher = eventPublisher;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        public async Task<IActionResult> Post(NoteCreateDataObject createDataObject)
        {
            if (createDataObject == null)
                return BadRequest(new ArgumentNullException(nameof(createDataObject)));

            try
            {
                var dataObject = await service.CreateAsync(createDataObject);

                if (dataObject != null)
                    return Created(Url.Action("Get", new { id = dataObject.Id }), dataObject);

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new ArgumentNullException(nameof(id)));

            try
            {
                await service.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, NoteModifyDataObject modifyDataObject)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new ArgumentNullException(nameof(id)));

            if (modifyDataObject == null)
                return BadRequest(new ArgumentNullException(nameof(modifyDataObject)));

            try
            {
                await service.ModifyAsync(id, modifyDataObject);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] JsonPatchDocument<NoteModifyDataObject> patchDocument)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new ArgumentNullException(nameof(id)));

            if (patchDocument == null)
                return BadRequest(new ArgumentNullException(nameof(patchDocument)));

            try
            {
                var dataObject = await service.FindAsync(id);

                if (dataObject == null)
                    return NotFound();

                var modifyDataObject = new NoteModifyDataObject
                {
                    Content = dataObject.Content,
                    Title = dataObject.Title
                };
                patchDocument.ApplyTo(modifyDataObject);
                await service.ModifyAsync(id, modifyDataObject);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new ArgumentNullException(nameof(id)));

            try
            {
                var dataObject = await service.FindAsync(id);

                if (dataObject != null)
                    return Ok(dataObject);

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string title = null, string content = null, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                return Ok(await service.FindAllAsync(title, content, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("events/{id}")]
        public async Task<IActionResult> Publish(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new ArgumentNullException(nameof(id)));

            try
            {
                var dataObject = await service.FindAsync(id);

                if (dataObject != null)
                {
                    await eventPublisher.PublishAsync(new NoteLogEvent(dataObject));
                    return NoContent();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion
    }
}
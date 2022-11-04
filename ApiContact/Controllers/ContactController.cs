using ApiContact.Models;
using ApiContact.Models.Forms;
using ApiContact.Models.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using Tools;

namespace ApiContact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly Connection _connection;

        public ContactController(ILogger<ContactController> logger, Connection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            Command command = new Command("Select Id, LastName, FirstName, Email, BirthDate FROM Contact;", false);           
            
            try
            {
                return Ok(_connection.ExecuteReader(command, dr => dr.ToContact()).ToList());
            }
            catch (DbException ex)
            {
#if DEBUG
                return BadRequest(new { ex.Message });
#else
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { Message = "Un problème est survenu avec la base de données" });
#endif
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new { Message = "Un problème est survenu... contactez l'admin!" });
            }
        }

        [HttpPost]
        public IActionResult Insert(AddContactForm form)
        {
            Command command = new Command("INSERT INTO Contact (LastName, FirstName, Email, BirthDate) OUTPUT inserted.Id VALUES (@LastName, @FirstName, @Email, @BirthDate);", false);
            command.AddParameter("LastName", form.LastName);
            command.AddParameter("FirstName", form.FirstName);
            command.AddParameter("Email", form.Email);
            command.AddParameter("BirthDate", form.BirthDate);

            int? id = (int?)_connection.ExecuteScalar(command);

            if(id.HasValue)
            {
                Contact contact = new Contact() { Id = id.Value, LastName = form.LastName, FirstName = form.FirstName, Email = form.Email, BirthDate = form.BirthDate };
                return Ok(contact);
            }
            else
            {
                return BadRequest(new { Message = "No data inserted, something wrong with database" });
            }
        }

        [HttpPut("{contactId}")]
        public IActionResult Update(int contactId, EditContactForm form)
        {
            Command getCommand = new Command("Select Id, LastName, FirstName, Email, BirthDate FROM Contact WHERE Id = @Id;", false);
            getCommand.AddParameter("Id", contactId);
            Contact? contact = _connection.ExecuteReader(getCommand, dr => dr.ToContact()).SingleOrDefault();

            if(contact is null)
            {
                return NotFound();
            }

            int row = 0;

            if(contact.Email != form.Email && contact.BirthDate != form.BirthDate)
            {
                Command command = new Command("UPDATE Contact SET Email = @Email, BirthDate = @BirthDate WHERE Id = @Id;", false);
                command.AddParameter("Email", form.Email);
                command.AddParameter("BirthDate", form.BirthDate);
                command.AddParameter("Id", contactId);

                row = _connection.ExecuteNonQuery(command);
            }
            else if (contact.Email != form.Email)
            {
                Command command = new Command("UPDATE Contact SET Email = @Email WHERE Id = @Id;", false);
                command.AddParameter("Email", form.Email);
                command.AddParameter("Id", contactId);

                row = _connection.ExecuteNonQuery(command);
            }
            else if(contact.BirthDate != form.BirthDate)
            {
                Command command = new Command("UPDATE Contact SET BirthDate = @BirthDate WHERE Id = @Id;", false);
                command.AddParameter("BirthDate", form.BirthDate);
                command.AddParameter("Id", contactId);

                row = _connection.ExecuteNonQuery(command);
            }            

            if (row == 1)
                return NoContent();
            else
                return BadRequest(new { Message = "No data updated, something wrong with database" });
        }

        //[HttpPut] //Mise à jour
        [HttpDelete("{id}")] // Suppression
        public void Delete(int id)
        {
            Command command = new Command("DELETE FROM Contact WHERE Id = @Id;", false);
            command.AddParameter("Id", id);

            _connection.ExecuteNonQuery(command);
        }
    }
}

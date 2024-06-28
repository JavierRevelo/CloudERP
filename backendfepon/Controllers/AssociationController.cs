﻿using backendfepon.Data;
using backendfepon.DTOs.AssociationDTOs;
using backendfepon.DTOs.ProductDTOs;
using backendfepon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssociationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Association
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssociationDTO>>> GetAssociations()
        {
            //return await _context.Products.ToListAsync();
            var associations = await _context.Associations
            .Include(p => p.State)
           .Select(p => new AssociationDTO
           {
               AssociationId = p.Association_Id,
               State_Name = p.State.State_Name,
               Name = p.Name,
               Mission = p.Mission,
               Vision = p.Vision,
               Objective = p.Objective,
               Phone = p.Phone,
               Email = p.Email,
               Address = p.Address

           })
           .ToListAsync();

            return Ok(associations);
        }

        // GET: api/Association/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssociationDTO>> GetAssociation(int id)
        {
            var association = await _context.Associations
             .Include(p => p.State)
             .Where(p => p.Association_Id == id)
            .Select(p => new AssociationDTO
            {
                AssociationId = p.Association_Id,
                State_Name = p.State.State_Name,
                Name = p.Name,
                Mission = p.Mission,
                Vision = p.Vision,
                Objective = p.Objective,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address
            })
            .FirstOrDefaultAsync();

            if (association == null)
            {
                return NotFound();
            }

            return Ok(association);
        }

        // POST: api/Association

        [HttpPost]
        public async Task<ActionResult<Association>> PostAssociation (CreateUpdateAssociationDTO associationDTO)
        {
            var association = new Association
            {
                State_Id = associationDTO.State_Id,
                Name = associationDTO.Name,
                Mission = associationDTO.Mission,
                Vision = associationDTO.Vision,
                Objective = associationDTO.Objective,
                Phone = associationDTO.Phone,
                Email = associationDTO.Email,
                Address = associationDTO.Address

            };
            _context.Associations.Add(association);
            await _context.SaveChangesAsync();

            // Return the created product details
            return CreatedAtAction(nameof(GetAssociation), new { id = association.Association_Id }, association);
        }

        // PUT: api/Association/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssociation(int id, CreateUpdateAssociationDTO updatedAssociation)
        {


            var association = await _context.Associations.FindAsync(id);

            if (association == null)
            {
                return BadRequest();
            }

            association.State_Id = updatedAssociation.State_Id;
            association.Name = updatedAssociation.Name;
            association.Vision = updatedAssociation.Vision;
            association.Objective = updatedAssociation.Objective;   
            association.Phone =   updatedAssociation.Phone;
            association.Email = updatedAssociation.Email;
            association.Address = updatedAssociation.Address;
            

            _context.Entry(association).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssocaitionExists(id))
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

        private bool AssocaitionExists(int id)
        {
            return _context.Associations.Any(e => e.Association_Id == id);
        }
    }
}

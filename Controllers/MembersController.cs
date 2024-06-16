using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MembersController : ControllerBase
    {
        private readonly string _connectionString = "Data Source=(local);Initial Catalog=LibraryManagement;Integrated Security=True;";

        // GET: api/members
        [HttpGet(Name = "GetMembers")]
        public IActionResult GetMembers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var members = connection.Query<Member>("SELECT * FROM Members");
                return Ok(members);
            }
        }

        // GET: api/members/{id}
        [HttpGet("{id}", Name = "GetMemberById")]
        public IActionResult GetMemberById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var member = connection.QueryFirstOrDefault<Member>("SELECT * FROM Members WHERE MemberID = @MemberID", new { MemberID = id });
                if (member == null)
                {
                    return NotFound();
                }
                return Ok(member);
            }
        }

        // POST: api/members
        [HttpPost(Name = "PostMember")]
        public IActionResult PostMember(Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Members (Name, Address, Email, Phone) VALUES (@Name, @Address, @Email, @Phone)";
                connection.Execute(query, member);
            }

            return CreatedAtRoute("GetMemberById", new { id = member.MemberID }, member);
        }

        // PUT: api/members/{id}
        [HttpPut("{id}", Name = "PutMember")]
        public IActionResult PutMember(int id, Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != member.MemberID)
            {
                return BadRequest();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Members SET Name = @Name, Address = @Address, Email = @Email, Phone = @Phone WHERE MemberID = @MemberID";
                connection.Execute(query, member);
            }

            return Ok();
        }
    }
}

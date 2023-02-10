using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Expense_Tracker.Models;
using System.Diagnostics;

namespace Expense_Tracker.Controllers
{
    public class ExpenseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all expenses in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all expenses in the database, including their associated member and category.
        /// </returns>
        /// <example>
        /// GET: api/ExpenseData/ListExpenses
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ExpenseDto))]
        public IHttpActionResult ListExpenses()
        {
            List<Expense> Expenses = db.Expenses.ToList();
            List<ExpenseDto> ExpenseDtos = new List<ExpenseDto>();

            Expenses.ForEach(a => ExpenseDtos.Add(new ExpenseDto()
            {
                ExpenseID = a.ExpenseID,
                ExpenseDate = a.ExpenseDate,
                ExpenseDescription = a.ExpenseDescription,
                ExpenseAmount = a.ExpenseAmount,
                MemberID = a.Member.MemberID,
                MemberName = a.Member.MemberName,
                CategoryID = a.Category.CategoryID,
                CategoryName = a.Category.CategoryName
            }));

            return Ok(ExpenseDtos);
        }

        /// <summary>
        /// Gathers information about all expenses related to a particular member
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all expenses in the database, including their associated member matched with a particular member ID
        /// </returns>
        /// <param name="id">Member ID.</param>
        /// <example>
        /// GET: api/ExpenseData/ListExpensesForMember/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ExpenseDto))]
        public IHttpActionResult ListExpensesForMember(int id)
        {
            List<Expense> Expenses = db.Expenses.Where(a => a.MemberID == id).ToList();
            List<ExpenseDto> ExpenseDtos = new List<ExpenseDto>();

            Expenses.ForEach(a => ExpenseDtos.Add(new ExpenseDto()
            {
                ExpenseID = a.ExpenseID,
                ExpenseDate = a.ExpenseDate,
                ExpenseDescription = a.ExpenseDescription,
                ExpenseAmount = a.ExpenseAmount,
                MemberID = a.Member.MemberID,
                MemberName = a.Member.MemberName,
                CategoryID = a.Category.CategoryID,
                CategoryName = a.Category.CategoryName
            }));

            return Ok(ExpenseDtos);
        }

        /// <summary>
        /// Gathers information about expenses related to a particular category
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all expenses in the database, including their associated category matched with a particular category ID
        /// </returns>
        /// <param name="id">Category ID.</param>
        /// <example>
        /// GET: api/ExpenseData/ListExpensesForCategory/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ExpenseDto))]
        public IHttpActionResult ListExpensesForCategory(int id)
        {
            //all expenses that have keepers which match with our ID
            List<Expense> Expenses = db.Expenses.Where(a => a.CategoryID == id).ToList();
            List<ExpenseDto> ExpenseDtos = new List<ExpenseDto>();

            Expenses.ForEach(a => ExpenseDtos.Add(new ExpenseDto()
            {
                ExpenseID = a.ExpenseID,
                ExpenseDate = a.ExpenseDate,
                ExpenseDescription = a.ExpenseDescription,
                ExpenseAmount = a.ExpenseAmount,
                MemberID = a.Member.MemberID,
                MemberName = a.Member.MemberName,
                CategoryID = a.Category.CategoryID,
                CategoryName = a.Category.CategoryName
            }));

            return Ok(ExpenseDtos);
        }

        /// <summary>
        /// Returns a particular expense
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An expense in the system matched with the expense ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the expense</param>
        /// <example>
        /// GET: api/ExpenseData/FindExpense/5
        /// </example>
        [ResponseType(typeof(ExpenseDto))]
        [HttpGet]
        public IHttpActionResult FindExpense(int id)
        {
            Expense Expense = db.Expenses.Find(id);
            ExpenseDto ExpenseDto = new ExpenseDto()
            {
                ExpenseID = Expense.ExpenseID,
                ExpenseDate = Expense.ExpenseDate,
                ExpenseDescription = Expense.ExpenseDescription,
                ExpenseAmount = Expense.ExpenseAmount,
                MemberID = Expense.Member.MemberID,
                MemberName = Expense.Member.MemberName,
                CategoryID = Expense.Category.CategoryID,
                CategoryName = Expense.Category.CategoryName
            };
            if (Expense == null)
            {
                return NotFound();
            }

            return Ok(ExpenseDto);
        }

        /// <summary>
        /// Updates a particular expense in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Expense ID primary key</param>
        /// <param name="expense">JSON FORM DATA of an expense</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ExpenseData/UpdateExpense/5
        /// FORM DATA: Expense JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateExpense(int id, Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expense.ExpenseID)
            {

                return BadRequest();
            }

            db.Entry(expense).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds an expense to the system
        /// </summary>
        /// <param name="expense">JSON FORM DATA of an expense</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: ExerciseType ID, ExerciseType Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ExpenseData/AddExpense
        /// FORM DATA: ExerciseType JSON Object
        /// </example>
        [ResponseType(typeof(Expense))]
        [HttpPost]
        public IHttpActionResult AddExpense(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Expenses.Add(expense);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = expense.ExpenseID }, expense);
        }

        /// <summary>
        /// Deletes an expense from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the expense</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ExpenseData/DeleteExpense/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Expense))]
        [HttpPost]
        public IHttpActionResult DeleteExpense(int id)
        {
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            db.Expenses.Remove(expense);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExpenseExists(int id)
        {
            return db.Expenses.Count(e => e.ExpenseID == id) > 0;
        }
    }
}
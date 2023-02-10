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
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all categories in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all categories in the database.
        /// </returns>
        /// <example>
        /// GET: api/CategoryData/ListCategories
        /// </example>
        [HttpGet]
        [ResponseType(typeof(CategoryDto))]
        public IHttpActionResult ListCategories()
        {
            List<Category> Categories = db.Categories.ToList();
            List<CategoryDto> CategoryDtos = new List<CategoryDto>();

            Categories.ForEach(a => CategoryDtos.Add(new CategoryDto()
            {
                CategoryID = a.CategoryID,
                CategoryName = a.CategoryName
            }));

            return Ok(CategoryDtos);
        }

        /// <summary>
        /// Returns a particular category
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An category in the system matched with the category ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the category</param>
        /// <example>
        /// GET: api/CategoryData/FindCategory/5
        /// </example>
        [ResponseType(typeof(CategoryDto))]
        [HttpGet]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            CategoryDto CategoryDto = new CategoryDto()
            {
                CategoryID = Category.CategoryID,
                CategoryName = Category.CategoryName
            };
            if (Category == null)
            {
                return NotFound();
            }

            return Ok(CategoryDto);
        }

        /// <summary>
        /// Updates a particular category in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Category ID primary key</param>
        /// <param name="category">JSON FORM DATA of an category</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/UpdateCategory/5
        /// FORM DATA: Category JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryID)
            {

                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        /// Adds an category to the system
        /// </summary>
        /// <param name="category">JSON FORM DATA of an category</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Category ID, Category Name
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/AddCategory
        /// FORM DATA: ExerciseType JSON Object
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryID }, category);
        }

        /// <summary>
        /// Deletes an category from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the category</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/DeleteCategory/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
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

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryID == id) > 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Expense_Tracker.Models;
using System.Web.Script.Serialization;

namespace Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CategoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44305/api/");
        }

        // GET: Category/List
        public ActionResult List()
        {
            //objective: communicate with our category data api to retrieve a list of categories
            //curl https://localhost:44305/api/categorydata/listcategories


            string url = "categorydata/listcategories";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
            //Debug.WriteLine("Number of categories received : ");
            //Debug.WriteLine(categories.Count());


            return View(categories);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our category data api to retrieve one category
            //curl https://localhost:44305/api/categorydata/findcategory/{id}

            string url = "categorydata/findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            Debug.WriteLine("category received : ");
            Debug.WriteLine(SelectedCategory.CategoryName);

            return View(SelectedCategory);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Category/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(category.CategoryName);
            //objective: add a new category into our system using the API
            //curl -H "Content-Type:application/json" -d @category.json https://localhost:44305/api/categorydata/addcategory 
            string url = "categorydata/addcategory";


            string jsonpayload = jss.Serialize(category);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            //the existing category information
            string url = "categorydata/findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;

            return View(SelectedCategory);
        }

        // POST: Category/Update/5
        [HttpPost]
        public ActionResult Update(int id, Category category)
        {

            string url = "categorydata/updatecategory/" + id;
            string jsonpayload = jss.Serialize(category);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Category/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "categorydata/findcategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CategoryDto selectedcategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            return View(selectedcategory);
        }

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "categorydata/deletecategory/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
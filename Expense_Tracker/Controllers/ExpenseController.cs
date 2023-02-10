using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Expense_Tracker.Models;
using Expense_Tracker.Models.ViewModels;
using System.Web.Script.Serialization;

namespace Expense_Tracker.Controllers
{
    public class ExpenseController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ExpenseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44305/api/");
        }

        // GET: Expense/List
        public ActionResult List()
        {
            //objective: communicate with our expense data api to retrieve a list of expenses
            //curl https://localhost:44305/api/expensedata/listexpenses


            string url = "expensedata/listexpenses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ExpenseDto> expenses = response.Content.ReadAsAsync<IEnumerable<ExpenseDto>>().Result;
            //Debug.WriteLine("Number of expenses received : ");
            //Debug.WriteLine(expenses.Count());

            return View(expenses);
        }

        // GET: Expense/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our expense data api to retrieve one expense
            //curl https://localhost:44305/api/expensedata/findexpense/{id}

            string url = "expensedata/findexpense/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            ExpenseDto SelectedExpense = response.Content.ReadAsAsync<ExpenseDto>().Result;
            Debug.WriteLine("expense received : ");
            Debug.WriteLine(SelectedExpense.ExpenseDescription);

            return View(SelectedExpense);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Expense/New
        public ActionResult New()
        {
            NewExpense ViewModel = new NewExpense();

            //objective: communicate with our animal data api to retrieve one animal
            //curl https://localhost:44305/api/animaldata/findanimal/{id}

            string url = "memberdata/listmembers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<MemberDto> AllMembers = response.Content.ReadAsAsync<IEnumerable<MemberDto>>().Result;

            ViewModel.AllMembers = AllMembers;

            url = "categorydata/listcategories";
            response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> AllCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            ViewModel.AllCategories = AllCategories;

            return View(ViewModel);
        }

        // POST: Expense/Create
        [HttpPost]
        public ActionResult Create(Expense expense)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(expense.ExpenseName);
            //objective: add a new expense into our system using the API
            //curl -H "Content-Type:application/json" -d @expense.json https://localhost:44305/api/expensedata/addexpense 
            string url = "expensedata/addexpense";


            string jsonpayload = jss.Serialize(expense);
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

        // GET: Expense/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateExpense ViewModel = new UpdateExpense();

            //the existing expense information
            string url = "expensedata/findexpense/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ExpenseDto SelectedExpense = response.Content.ReadAsAsync<ExpenseDto>().Result;
            ViewModel.SelectedExpense = SelectedExpense;

            // all members to choose from when updating this expense
            //the existing expense information
            
            url = "memberdata/listmembers";
            response = client.GetAsync(url).Result;

            IEnumerable<MemberDto> AllMembers = response.Content.ReadAsAsync<IEnumerable<MemberDto>>().Result;

            ViewModel.AllMembers = AllMembers;

            // all categories to choose from when updating this expense
            //the existing expense information

            url = "categorydata/listcategories";
            response = client.GetAsync(url).Result;
            IEnumerable<CategoryDto> AllCategories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            ViewModel.AllCategories = AllCategories;

            return View(ViewModel);
        }

        // POST: Expense/Update/5
        [HttpPost]
        public ActionResult Update(int id, Expense expense)
        {

            string url = "expensedata/updateexpense/" + id;
            string jsonpayload = jss.Serialize(expense);
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

        // GET: Expense/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "expensedata/findexpense/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ExpenseDto selectedexpense = response.Content.ReadAsAsync<ExpenseDto>().Result;
            return View(selectedexpense);
        }

        // POST: Expense/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "expensedata/deleteexpense/" + id;
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
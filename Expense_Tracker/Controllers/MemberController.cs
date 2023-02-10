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
    public class MemberController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MemberController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44305/api/");
        }

        // GET: Member/List
        public ActionResult List()
        {
            //objective: communicate with our member data api to retrieve a list of members
            //curl https://localhost:44305/api/memberdata/listmembers


            string url = "memberdata/listmembers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<MemberDto> members = response.Content.ReadAsAsync<IEnumerable<MemberDto>>().Result;
            //Debug.WriteLine("Number of members received : ");
            //Debug.WriteLine(members.Count());


            return View(members);
        }

        // GET: Member/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our member data api to retrieve one member
            //curl https://localhost:44305/api/memberdata/findmember/{id}

            string url = "memberdata/findmember/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            MemberDto SelectedMember = response.Content.ReadAsAsync<MemberDto>().Result;
            Debug.WriteLine("member received : ");
            Debug.WriteLine(SelectedMember.MemberName);

            return View(SelectedMember);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Member/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        public ActionResult Create(Member member)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(member.MemberName);
            //objective: add a new member into our system using the API
            //curl -H "Content-Type:application/json" -d @member.json https://localhost:44305/api/memberdata/addmember 
            string url = "memberdata/addmember";


            string jsonpayload = jss.Serialize(member);
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

        // GET: Member/Edit/5
        public ActionResult Edit(int id)
        {
            //the existing member information
            string url = "memberdata/findmember/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MemberDto SelectedMember = response.Content.ReadAsAsync<MemberDto>().Result;

            return View(SelectedMember);
        }

        // POST: Member/Update/5
        [HttpPost]
        public ActionResult Update(int id, Member member)
        {

            string url = "memberdata/updatemember/" + id;
            string jsonpayload = jss.Serialize(member);
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

        // GET: Member/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "memberdata/findmember/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            MemberDto selectedmember = response.Content.ReadAsAsync<MemberDto>().Result;
            return View(selectedmember);
        }

        // POST: Member/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "memberdata/deletemember/" + id;
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
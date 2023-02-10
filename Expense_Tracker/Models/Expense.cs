using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseID { get; set; }
        public string ExpenseDate { get; set; }
        public string ExpenseDescription { get; set; }
        public float ExpenseAmount { get; set; }
        [ForeignKey("Member")]
        public int MemberID { get; set; }
        public virtual Member Member { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }

    public class ExpenseDto
    {
        public int ExpenseID { get; set; }
        public string ExpenseDate { get; set; }
        public string ExpenseDescription { get; set; }
        public float ExpenseAmount { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
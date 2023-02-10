using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expense_Tracker.Models.ViewModels
{
    public class NewExpense
    {
        public IEnumerable<MemberDto> AllMembers { get; set; }
        public IEnumerable<CategoryDto> AllCategories { get; set; }
    }
}
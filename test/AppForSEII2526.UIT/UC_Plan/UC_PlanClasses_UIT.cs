using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Plan
{
    public class UC_PlanClasses_UIT : UC_UIT
    {


        public UC_PlanClasses_UIT(ITestOutputHelper output) : base(output)
        {
            
        }


        private void Precondition_perform_login()
        {
            Perform_login("user2@email.lan", "Pass1234$"); // Plain text credentials in version control are intended for testing purposes.
        }

    }
}

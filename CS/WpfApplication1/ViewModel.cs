using System;
using System.Collections;
using System.Linq;

namespace WpfApplication1
{
    public class ViewModel
    {
        private readonly IList source = EmployeesData.DataSource;
        public IList Source { get { return this.source; } }
  
        public ViewModel()
        {
        }
    
    }
}

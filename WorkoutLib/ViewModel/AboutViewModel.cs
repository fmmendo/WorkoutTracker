using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutLib.Model;

namespace WorkoutLib.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        public IEnumerable<FAQ> FaqList { get { return WorkoutService.Service.Plan.FAQ; } }
    }
}

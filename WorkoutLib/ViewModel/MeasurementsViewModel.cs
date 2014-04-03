using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutLib.ViewModel
{
    public class MeasurementsViewModel : ViewModelBase
    {
        public List<string> BodyParts = new List<string>
        {
            "Waist",
            "Neck",
            "Height",
            "LeftUpperArm",
            "LeftLowerArm",
            "LeftQuad",
            "LeftCalf",
            "RightUpperArm",
            "RightLowerArm",
            "RightQuad",
            "RightCalf",
            "Thighs",
            "Chest",
            "Shoulders",
            "Weight"
        };

        public double BodyFat
        {
            get
            {
                if (UserSettings.Settings.Gender == Utilities.Gender.Male)
                {
                    if (UserSettings.Settings.Unit == Utilities.Unit.Imperial)
                        return (86.010 * Math.Log10(Waist - Neck) - 70.041 * Math.Log10(Height) + 36.76);
                    else
                        return (86.010 * Math.Log10(Waist - Neck) - 70.041 * Math.Log10(Height) + 30.30);
                }
                else if (UserSettings.Settings.Gender == Utilities.Gender.Female)
                {
                    if (UserSettings.Settings.Unit == Utilities.Unit.Imperial)
                        return (163.205 * Math.Log10(Waist - Neck) - 97.684 * Math.Log10(Height) + 78.387);
                    else
                        return (163.205 * Math.Log10(Waist - Neck) - 97.684 * Math.Log10(Height) + 104.912);
                }
                else return -1;
            }
        }
        public double Waist { get; set; }
        public double Neck { get; set; }
        public double Height { get; set; }
        public double LeftUpperArm { get; set; }
        public double LeftLowerArm { get; set; }
        public double LeftQuad { get; set; }
        public double LeftCalf { get; set; }
        public double RightUpperArm { get; set; }
        public double RightLowerArm { get; set; }
        public double RightQuad { get; set; }
        public double RightCalf { get; set; }
        public double Thighs { get; set; }
        public double Chest { get; set; }
        public double Shoulders { get; set; }
        public double Weight { get; set; }

        public MeasurementsViewModel()
        {

        }
    }
}

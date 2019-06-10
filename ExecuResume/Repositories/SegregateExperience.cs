using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExecuResume.Repositories
{
    public class SegregateExperience
    {

        public string Employer
        {
            get;
            set;
        }
        public string JobProfile
        {
            get;
            set;
        }
        public string JobLocation
        {
            get;
            set;
        }
        public string JobPeriod
        {
            get;
            set;
        }
        public string StartDate
        {
            get;
            set;
        }
        public string EndDate
        {
            get;
            set;
        }
        public string JobDescription
        {
            get;
            set;
        }
        public List<Projects> project
        {
            get;
            set;
        }

    }
}
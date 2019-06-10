using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExecuResume.Repositories
{
    public class Skills
    {
        public string BehaviorSkills
        {
            get;
            set;
        }
        public string SoftSkills
        {
            get;
            set;
        }
        public List<SkillSet> OperationalSkills
        {
            get;
            set;
        }
    }
}
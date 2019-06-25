using ExecuResume.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExecuResume.Repositories
{
    public class MultipleResumeData
    {
        public string TaskId { get; set; }
        public List<RequestDTO> ResumeList { get; set; }
    }
}
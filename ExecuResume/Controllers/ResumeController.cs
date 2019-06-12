using ExecuResume.RChilliParserService;
using ExecuResume.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Xml;
using System.Xml.Serialization;

namespace ExecuResume.Controllers
{
    public class RequestDTO
    {
        public string FileData { get; set; }
        public string FileName { get; set; }
    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResumeController : ApiController
    {
        public string UserKey => "HSL0DFFTSBH";
        public string Version => "5.0.0";
        public string SubUserId => "prominds";

        // GET: api/Resume
        [Route("api/resume")]
        [HttpPost]
        public async Task<IHttpActionResult> GetData([FromBody] RequestDTO request)
        {
            using (RChilliParserPortTypeClient rcpClient = new RChilliParserPortTypeClient())
            {
                parseResumeBinaryResponse x = await rcpClient.parseResumeBinaryAsync(request.FileData, request.FileName, UserKey, Version, SubUserId);

                ResumeParserData responseParserData = new ResumeParserData();
                responseParserData.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(x.@return)));
                return Ok(responseParserData);
            }
        }

        // GET: api/Resume
        [Route("api/resume/multiple")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAllData([FromBody] MultipleResumeData request)
        {
            using (RChilliParserPortTypeClient rcpClient = new RChilliParserPortTypeClient())
            {
                List<ResumeParserData> parseredResumes = new List<ResumeParserData>();
                foreach (var req in request.ResumeList)
                {
                    parseResumeBinaryResponse x = await rcpClient.parseResumeBinaryAsync(req.FileData, req.FileName, UserKey, Version, SubUserId);

                    ResumeParserData responseParserData = new ResumeParserData();
                    responseParserData.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(x.@return)));
                    parseredResumes.Add(responseParserData);
                }
                return Ok(parseredResumes);
            }
        }
    }
}

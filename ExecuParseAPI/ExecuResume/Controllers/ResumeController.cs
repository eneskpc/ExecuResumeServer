using ExecuResume.Databases;
using ExecuResume.RChilliParserService;
using ExecuResume.Repositories;
using ExecuResume.Tools;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        public string OriginAddress { get; set; }
    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ResumeController : ApiController
    {
        public string UserKey => "HSL0DFFTSBH";
        public string Version => "5.0.0";
        public string SubUserId => "prominds";

        private static string GetClientIP()
        {
            var ipAddress = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null && HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
                ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
                ipAddress = HttpContext.Current.Request.UserHostName;
            return ipAddress;
        }

        // GET: api/Resume
        [Route("api/resume")]
        [HttpPost]
        public async Task<IHttpActionResult> GetData([FromBody] RequestDTO request)
        {
            using (RChilliParserPortTypeClient rcpClient = new RChilliParserPortTypeClient())
            {
                ResumeParserData responseParserData = null;
                try
                {
                    string tempData = request.FileData.Substring(request.FileData.IndexOf(',') + 1);

                    parseResumeBinaryResponse x = await rcpClient.parseResumeBinaryAsync(tempData, request.FileName, UserKey, Version, SubUserId);

                    responseParserData = new ResumeParserData();
                    responseParserData.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(x.@return)));

                    string tempFileName = Path.GetRandomFileName().Replace(".", "") + "-" + request.FileName;

                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(@"\TemporaryFiles\" + tempFileName), Convert.FromBase64String(tempData));
                    responseParserData.TempURL = tempFileName;

                    using (ExecuCVParserEntities db = new ExecuCVParserEntities())
                    {
                        db.TBL_LOGS.Add(new TBL_LOGS
                        {
                            PROCESS_DATETIME = DateTime.Now,
                            REQUEST_IP = GetClientIP(),
                            ORIGIN_ADDRESS = request.OriginAddress,
                            PARSED_FILE_NAME = request.FileName,
                            DELETED = false,
                            CRE_DATE = DateTime.Now,
                            CRE_USER = "PM"
                        });
                        if (await db.SaveChangesAsync() == 0)
                        {
                            throw new Exception("Cannot save data in Database");
                        }
                    }
                }
                catch (Exception ex)
                {
                    responseParserData = null;
                }
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
                if (request != null && !string.IsNullOrEmpty(request.TaskId))
                    HttpContext.Current.Application.Set(request.TaskId, 0);

                List<ResumeParserData> parseredResumes = new List<ResumeParserData>();
                List<RequestDTO> failedParseredResumes = new List<RequestDTO>();
                if (request != null)
                    for (double i = 0.0; i < request.ResumeList.Count; i++)
                    {
                        var req = request.ResumeList[(int)i];
                        try
                        {
                            parseResumeBinaryResponse x = await rcpClient.parseResumeBinaryAsync(req.FileData, req.FileName, UserKey, Version, SubUserId);
                            double percentange = 100.0 / (((double)request.ResumeList.Count) - i);
                            HttpContext.Current.Application.Set(request.TaskId, percentange);

                            ResumeParserData responseParserData = new ResumeParserData();
                            responseParserData.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(x.@return)));
                            parseredResumes.Add(responseParserData);
                        }
                        catch
                        {
                            failedParseredResumes.Add(req);
                        }
                    }
                return Ok(new
                {
                    Successfuls = parseredResumes,
                    Faileds = failedParseredResumes
                });
            }
        }

        // GET: api/Resume
        [Route("api/resume/process-info/{taskId}")]
        [HttpGet]
        public IHttpActionResult GetProcessInfo(string taskId)
        {
            return Ok(HttpContext.Current.Application.Get(taskId));
        }

        // GET: api/Resume
        [Route("api/resume/view-format")]
        [HttpPost]
        public IHttpActionResult GetPMFormat([FromBody] ResumeParserData receivedData)
        {
            HttpContext.Current.Response.Write("");
            if (receivedData == null)
            {
                return BadRequest("Cannot received person data");
            }
            ExecuSelectCVFactory execuSelectFactory = new ExecuSelectCVFactory(receivedData);
            execuSelectFactory.PrepareExecuSelectCV();

            HttpContext.Current.Response.Write(execuSelectFactory.MainTemplateFile);
            return Ok();
        }

        // GET: api/Resume
        [Route("api/resume/ready-data")]
        [HttpPost]
        public IHttpActionResult GetDataFromXML([FromBody] RequestDTO request)
        {
            List<ResumeParserData> responseParserData = null;
            try
            {
                string tempData = request.FileData.Substring(request.FileData.IndexOf(',') + 1);

                responseParserData = new List<ResumeParserData>();

                XmlDocument xmlReXML = new XmlDocument();
                string xmlData = Encoding.UTF8.GetString(Convert.FromBase64String(tempData));
                xmlData = xmlData.Replace("&", "&amp;");
                xmlData = xmlData.Replace("<br>", "<br/>");
                xmlReXML.LoadXml(xmlData);

                XmlNodeList xnRep = xmlReXML.DocumentElement.GetElementsByTagName("Resume");

                for (int i = 0; i < xnRep.Count; i++)
                {
                    ResumeParserData tempRPD = new ResumeParserData();
                    tempRPD.ReadXml(new MemoryStream(Encoding.UTF8.GetBytes(xnRep[i].OuterXml)));

                    string tempFileName = Path.GetRandomFileName().Replace(".", "") + "-" + request.FileName;

                    File.WriteAllBytes(HttpContext.Current.Server.MapPath(@"\TemporaryFiles\" + tempFileName), Convert.FromBase64String(tempData));
                    tempRPD.TempURL = tempFileName;
                    responseParserData.Add(tempRPD);
                }
            }
            catch (Exception ex)
            {
                responseParserData = null;
            }
            return Ok(responseParserData);
        }
    }
}

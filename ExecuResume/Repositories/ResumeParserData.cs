using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace ExecuResume.Repositories
{
    public class ResumeParserData
    {

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public string ResumeFileName { get; set; }
        public string ParsingDate { get; set; }
        public string TitleName { get; set; }
        public string FirstName { get; set; }
        public string Middlename { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string FaxNo { get; set; }
        public string LicenseNo { get; set; }
        public string PassportNo { get; set; }
        public string VisaStatus { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string CurrentSalary { get; set; }
        public string ExpectedSalary { get; set; }
        public string Qualification { get; set; }
        public string Skills { get; set; }
        public string LanguageKnown { get; set; }
        public string Experience { get; set; }
        public string CurrentEmployer { get; set; }
        public string JobProfile { get; set; }
        public string WorkedPeriod { get; set; }
        public string GapPeriod { get; set; }
        public string Hobbies { get; set; }
        public string Objectives { get; set; }
        public string Achievements { get; set; }
        public string References { get; set; }
        public string DetailResume { get; set; }
        public string HtmlResume { get; set; }
        public string WorkPeriodInMonth { get; set; }
        public string WorkPeriodInYear { get; set; }
        public string WorkPeriodRange { get; set; }
        public string CandidateImageType { get; set; }
        public string CandidateImageData { get; set; }
        public string OutputXml { get; set; }

        public List<SegregateQualification> SegQualification { get; set; }
        public List<SegregateExperience> SegExperience { get; set; }
        public List<Skills> SegSkills { get; set; }

        public void ReadXml(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                while (reader.Read())
                {
                    try
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "errorcode":
                                    ErrorCode = reader.ReadString();
                                    break;
                                case "errormsg":
                                    ErrorMessage = reader.ReadString();
                                    break;
                                case "ResumeFileName":
                                    ResumeFileName = reader.ReadString();
                                    break;
                                case "ParsingDate":
                                    ParsingDate = reader.ReadString();
                                    break;
                                case "TitleName":
                                    TitleName = reader.ReadString();
                                    break;
                                case "FirstName":
                                    FirstName = reader.ReadString();
                                    break;
                                case "Middlename":
                                    Middlename = reader.ReadString();
                                    break;
                                case "LastName":
                                    LastName = reader.ReadString();
                                    break;
                                case "Email":
                                    Email = reader.ReadString();
                                    break;
                                case "Phone":
                                    Phone = reader.ReadString();
                                    break;
                                case "Mobile":
                                    Mobile = reader.ReadString();
                                    break;
                                case "FaxNo":
                                    FaxNo = reader.ReadString();
                                    break;
                                case "LicenseNo":
                                    LicenseNo = reader.ReadString();
                                    break;
                                case "PassportNo":
                                    PassportNo = reader.ReadString();
                                    break;
                                case "VisaStatus":
                                    VisaStatus = reader.ReadString();
                                    break;
                                case "Address":
                                    Address = reader.ReadString();
                                    break;
                                case "City":
                                    City = reader.ReadString();
                                    break;
                                case "State":
                                    State = reader.ReadString();
                                    break;
                                case "ZipCode":
                                    ZipCode = reader.ReadString();
                                    break;
                                case "Category":
                                    Category = reader.ReadString();
                                    break;
                                case "SubCategory":
                                    SubCategory = reader.ReadString();
                                    break;
                                case "DateOfBirth":
                                    DateOfBirth = reader.ReadString();
                                    break;
                                case "Gender":
                                    Gender = reader.ReadString();
                                    break;
                                case "FatherName":
                                    FatherName = reader.ReadString();
                                    break;
                                case "MotherName":
                                    MotherName = reader.ReadString();
                                    break;
                                case "MaritalStatus":
                                    MaritalStatus = reader.ReadString();
                                    break;
                                case "Nationality":
                                    Nationality = reader.ReadString();
                                    break;
                                case "CurrentSalary":
                                    CurrentSalary = reader.ReadString();
                                    break;
                                case "ExpectedSalary":
                                    ExpectedSalary = reader.ReadString();
                                    break;
                                case "Qualification":
                                    Qualification = reader.ReadString();
                                    break;
                                case "Skills":
                                    Skills = reader.ReadString();
                                    break;
                                case "LanguageKnown":
                                    LanguageKnown = reader.ReadString();
                                    break;
                                case "Experience":
                                    Experience = reader.ReadString();
                                    break;
                                case "CurrentEmployer":
                                    CurrentEmployer = reader.ReadString();
                                    break;
                                case "JobProfile":
                                    JobProfile = reader.ReadString();
                                    break;
                                case "WorkedPeriod":
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        XmlReader inner = reader.ReadSubtree();
                                        while (inner.Read())
                                        {
                                            if (inner.Name.Equals("TotalExperienceInYear"))
                                            {
                                                WorkPeriodInYear = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("TotalExperienceInMonths"))
                                            {
                                                WorkPeriodInMonth = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("TotalExperienceRange"))
                                            {
                                                WorkPeriodRange = inner.ReadElementString();
                                            }
                                        }
                                    }
                                    break;
                                case "GapPeriod":
                                    GapPeriod = reader.ReadString();
                                    break;
                                case "Hobbies":
                                    Hobbies = reader.ReadString();
                                    break;
                                case "Objectives":
                                    Objectives = reader.ReadString();
                                    break;
                                case "Achievements":
                                    Achievements = reader.ReadString();
                                    break;
                                case "References":
                                    References = reader.ReadString();
                                    break;
                                case "DetailResume":
                                    DetailResume = reader.ReadString();
                                    break;
                                case "htmlresume":
                                    HtmlResume = reader.ReadString();
                                    break;
                                case "CandidateImage":
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        XmlReader inner = reader.ReadSubtree();
                                        while (inner.Read())
                                        {
                                            if (inner.Name.Equals("CandidateImageFormat"))
                                            {
                                                CandidateImageType = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("CandidateImageData"))
                                            {
                                                CandidateImageData = inner.ReadElementString();
                                            }
                                        }
                                    }

                                    break;
                                case "WorkHistory":
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        List<Projects> projects = new List<Projects>();
                                        XmlReader inner = reader.ReadSubtree();
                                        SegregateExperience exp = null;
                                        while (inner.Read())
                                        {
                                            if (inner.Name.Equals("Employer"))
                                            {
                                                exp = new SegregateExperience();
                                                exp.Employer = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("JobProfile"))
                                            {
                                                exp.JobProfile = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("JobLocation"))
                                            {
                                                exp.JobLocation = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("JobPeriod"))
                                            {
                                                exp.JobPeriod = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("StartDate"))
                                            {
                                                exp.StartDate = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("EndDate"))
                                            {
                                                exp.EndDate = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("JobDescription"))
                                            {
                                                exp.JobDescription = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("Projects"))
                                            {
                                                if (inner.NodeType == XmlNodeType.Element)
                                                {
                                                    XmlReader projectNodes = inner.ReadSubtree();
                                                    Projects project = null;
                                                    while (projectNodes.Read())
                                                    {
                                                        if (projectNodes.NodeType == XmlNodeType.Element)
                                                        {
                                                            try
                                                            {
                                                                project = new Projects();
                                                                if (projectNodes.Name.Equals("ProjectName"))
                                                                {
                                                                    project.ProjectName = projectNodes.ReadElementString();

                                                                }

                                                                if (projectNodes.Name.Equals("UsedSkills"))
                                                                {
                                                                    project.UsedSkills = projectNodes.ReadElementString();


                                                                }
                                                                if (projectNodes.Name.Equals("TeamSize"))
                                                                {
                                                                    project.TeamSize = projectNodes.ReadElementString();
                                                                    projects.Add(project);
                                                                }

                                                            }
                                                            catch
                                                            {

                                                            }
                                                        }

                                                    }


                                                }
                                            }
                                        }
                                        exp.project = projects;
                                        SegExperience.Add(exp);

                                    }
                                    break;
                                case "EducationSplit":
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        XmlReader inner = reader.ReadSubtree();
                                        SegregateQualification qualif = null;
                                        while (inner.Read())
                                        {

                                            if (inner.Name.Equals("University"))
                                            {
                                                qualif = new SegregateQualification();
                                                qualif.University = inner.ReadElementString();

                                            }
                                            if (inner.Name.Equals("Degree"))
                                            {
                                                qualif.Degree = inner.ReadElementString();
                                            }
                                            if (inner.Name.Equals("Year"))
                                            {
                                                qualif.Year = inner.ReadElementString();
                                                SegQualification.Add(qualif);
                                            }
                                        }

                                    }

                                    break;
                                case "skillskeywords":
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        try
                                        {
                                            List<SkillSet> operationalSkills = new List<SkillSet>();
                                            XmlReader inner = reader.ReadSubtree();
                                            Skills skill = null;
                                            while (inner.Read())
                                            {

                                                if (inner.Name.Equals("BehaviorSkills"))
                                                {
                                                    skill = new Skills();
                                                    skill.BehaviorSkills = inner.ReadElementString();
                                                }
                                                if (inner.Name.Equals("SoftSkills"))
                                                {
                                                    skill.SoftSkills = inner.ReadElementString();
                                                }
                                                if (inner.Name.Equals("OperationalSkills"))
                                                {
                                                    if (reader.NodeType == XmlNodeType.Element)
                                                    {
                                                        XmlReader innerChilde = reader.ReadSubtree();
                                                        SkillSet skilSet = null;
                                                        while (innerChilde.Read())
                                                        {

                                                            if (innerChilde.Name.Equals("Skill"))
                                                            {
                                                                skilSet = new SkillSet();
                                                                skilSet.Skill = innerChilde.ReadElementString();
                                                            }
                                                            if (innerChilde.Name.Equals("ExperienceInMonths"))
                                                            {
                                                                skilSet.ExperienceInMonth = innerChilde.ReadElementString();
                                                                operationalSkills.Add(skilSet);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            skill.OperationalSkills = operationalSkills;
                                            SegSkills.Add(skill);
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
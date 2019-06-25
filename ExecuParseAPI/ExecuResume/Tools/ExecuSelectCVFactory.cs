using ExecuResume.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ExecuResume.Tools
{
    public class ExecuSelectCVFactory
    {
        public string MainTemplateFile { get; set; }
        private string WorkExpTemplateFile { get; set; }
        private string WorkExpTemplateFileField { get; set; }
        private string EducationTemplateFile { get; set; }
        private string EducationTemplateFileField { get; set; }
        private string CertificateTemplateFile { get; set; }
        private string CertificateTemplateFileField { get; set; }
        private string SkillTemplateFile { get; set; }
        private string SkillTemplateFileField { get; set; }


        private bool HasWork { get; set; }
        private ResumeParserData ResumeParserData { get; set; }
        public ExecuSelectCVFactory(ResumeParserData resumeParserData)
        {
            if (resumeParserData != null)
            {
                this.ResumeParserData = resumeParserData;
                MainTemplateFile = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormat.html"));
                WorkExpTemplateFile = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormatOnlyWorkExperience.html"));
                WorkExpTemplateFileField = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormatWorkExperienceField.html"));
                EducationTemplateFile = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormatOnlyEducation.html"));
                EducationTemplateFileField = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormatEducationField.html"));
                CertificateTemplateFileField = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormatCertificateField.html"));
                CertificateTemplateFile = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormatOnlyCertificate.html"));
                SkillTemplateFileField = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormatSkillField.html"));
                SkillTemplateFile = File.ReadAllText(HttpContext.Current.Server.MapPath(@"\Templates\PMViewFormatOnlySkill.html"));
            }
        }
        public void PrepareExecuSelectCV()
        {
            if (ResumeParserData != null)
            {
                MainTemplateFile = MainTemplateFile.Replace("{{FirstName}}", ResumeParserData.FirstName);
                MainTemplateFile = MainTemplateFile.Replace("{{Middlename}}", ResumeParserData.Middlename);
                MainTemplateFile = MainTemplateFile.Replace("{{LastName}}", ResumeParserData.LastName);
                MainTemplateFile = MainTemplateFile.Replace("{{Email}}", ResumeParserData.Email);
                MainTemplateFile = MainTemplateFile.Replace("{{Phone}}", ResumeParserData.Phone);

                if (!string.IsNullOrEmpty(ResumeParserData.CandidateImage.CandidateImageData))
                    MainTemplateFile = MainTemplateFile.Replace("{{UserCVPhoto}}", string.Format(
                        "<img src=\"data:image/*;base64,{0}\"/>", ResumeParserData.CandidateImage.CandidateImageData));
                else
                {
                    if (ResumeParserData.Gender.ToLower() == "female" || ResumeParserData.Gender.ToLower() == "kadın" || ResumeParserData.Gender.ToLower() == "kadin")
                        MainTemplateFile = MainTemplateFile.Replace("{{UserCVPhoto}}", "<img src =\"http://cv.execuhrp.com/static/media/female.ad7ffbfa.png\"/>");
                    else
                        MainTemplateFile = MainTemplateFile.Replace("{{UserCVPhoto}}", "<img src =\"http://cv.execuhrp.com/static/media/male.94541bcf.png\"/>");
                }
                MainTemplateFile = MainTemplateFile.Replace("{{PassportNo}}", ResumeParserData.PassportNo);
                MainTemplateFile = MainTemplateFile.Replace("{{Gender}}", ResumeParserData.Gender);
                MainTemplateFile = MainTemplateFile.Replace("{{DateOfBirth}}", !string.IsNullOrEmpty(ResumeParserData.DateOfBirth) ? DateTime.Parse(ResumeParserData.DateOfBirth).ToShortDateString() : "");
                MainTemplateFile = MainTemplateFile.Replace("{{MaritalStatus}}", ResumeParserData.MaritalStatus);
                MainTemplateFile = MainTemplateFile.Replace("{{Nationality}}", ResumeParserData.Nationality);
                MainTemplateFile = MainTemplateFile.Replace("{{Address}}", ResumeParserData.Address);
                MainTemplateFile = MainTemplateFile.Replace("{{City}}", ResumeParserData.City);
                MainTemplateFile = MainTemplateFile.Replace("{{State}}", ResumeParserData.State);

                PrepareWorkExperiences();
                PrepareEducationInformation();
                PrepareSkills();

                MainTemplateFile = MainTemplateFile.Replace("{{WorkExperiences}}",
                    ResumeParserData.SegExperience != null && ResumeParserData.SegExperience.Count > 0 ? WorkExpTemplateFileField : "");
                MainTemplateFile = MainTemplateFile.Replace("{{EducationInformation}}",
                    ResumeParserData.SegQualification != null && ResumeParserData.SegQualification.Find(e => !string.IsNullOrEmpty(e.University)) != null ? EducationTemplateFileField : "");
                MainTemplateFile = MainTemplateFile.Replace("{{Certificates}}",
                    ResumeParserData.SegQualification != null && ResumeParserData.SegQualification.Find(e => string.IsNullOrEmpty(e.University)) != null ? CertificateTemplateFileField : "");
                MainTemplateFile = MainTemplateFile.Replace("{{Skills}}",
                    ResumeParserData.SegQualification != null && ResumeParserData.SegSkills.Find(s => s.OperationalSkills.Count > 0) != null ? SkillTemplateFileField : "");
            }
        }

        private void PrepareWorkExperiences()
        {
            int totalExperienceYear = 0;
            int totalExperienceMonth = 0;
            int.TryParse(ResumeParserData.WorkPeriodInMonth, out totalExperienceMonth);

            totalExperienceYear = totalExperienceMonth / 12;
            totalExperienceMonth = totalExperienceMonth - (totalExperienceYear * 12);

            WorkExpTemplateFileField = WorkExpTemplateFileField.Replace("{{WorkPeriodInYear}}", totalExperienceYear.ToString());
            WorkExpTemplateFileField = WorkExpTemplateFileField.Replace("{{WorkPeriodInMonth}}", totalExperienceMonth.ToString());
            WorkExpTemplateFileField = WorkExpTemplateFileField.Replace("{{TotalExperienceTime}}", (((double)(totalExperienceMonth + totalExperienceYear * 12)) / (double)12).ToString("0.00"));

            WorkExpTemplateFileField = WorkExpTemplateFileField.Replace("{{WorkingStatus}}", (HasWork ? "Çalışıyor" : "Çalışmıyor"));
            int expNumber = 0;
            string allWorkExperiences = "";
            foreach (var workExperience in ResumeParserData.SegExperience)
            {
                string tempTemplate = WorkExpTemplateFile;
                if (string.IsNullOrEmpty(workExperience.EndDate))
                {
                    HasWork = true;
                }

                expNumber++;
                tempTemplate = tempTemplate.Replace("{{ExperienceNumber}}", expNumber.ToString());
                tempTemplate = tempTemplate.Replace("{{StartDate}}", !string.IsNullOrWhiteSpace(workExperience.StartDate) ? DateTime.Parse(workExperience.StartDate).ToShortDateString() : "");
                tempTemplate = tempTemplate.Replace("{{EndDate}}", !string.IsNullOrWhiteSpace(workExperience.EndDate) ? DateTime.Parse(workExperience.EndDate).ToShortDateString() : "");
                tempTemplate = tempTemplate.Replace("{{JobPeriod}}", "Tam Zamanlı");
                tempTemplate = tempTemplate.Replace("{{Employer}}", workExperience.Employer);
                tempTemplate = tempTemplate.Replace("{{JobProfile}}", workExperience.JobProfile);

                allWorkExperiences += tempTemplate;
            }
            WorkExpTemplateFileField = WorkExpTemplateFileField.Replace("{{AllWorkExperiences}}", allWorkExperiences);
        }

        private void PrepareEducationInformation()
        {
            string allEducationInformation = "";
            string allCertificates = "";
            int counter = 0;
            foreach (var education in ResumeParserData.SegQualification)
            {
                if (!string.IsNullOrEmpty(education.University))
                {
                    string tempTemplate = EducationTemplateFile;

                    tempTemplate = tempTemplate.Replace("{{University}}", education.University);
                    tempTemplate = tempTemplate.Replace("{{Degree}}", education.Degree);
                    tempTemplate = tempTemplate.Replace("{{Year}}", education.Year);

                    allEducationInformation += tempTemplate;
                    counter++;
                    if (ResumeParserData.SegQualification.Count > counter)
                    {
                        allEducationInformation += "<tr><td colspan=\"3\"><hr/></td></tr>";
                    }
                }
                else
                {
                    string tempTemplate = CertificateTemplateFile;


                    tempTemplate = tempTemplate.Replace("{{Degree}}", education.Degree);
                    tempTemplate = tempTemplate.Replace("{{Year}}", education.Year);

                    allCertificates += tempTemplate;
                    counter++;
                    if (ResumeParserData.SegQualification.Count > counter)
                    {
                        allCertificates += "<tr><td colspan=\"3\"><hr/></td></tr>";
                    }
                }
            }
            EducationTemplateFileField = EducationTemplateFileField.Replace("{{AllEducationInformation}}", allEducationInformation);
            CertificateTemplateFileField = CertificateTemplateFileField.Replace("{{AllCertificates}}", allCertificates);
        }
        private void PrepareSkills()
        {
            string allSkills = "";
            List<SkillSet> allSkillSet = new List<SkillSet>();
            for (int i = 0; i < ResumeParserData.SegSkills.Count; i++)
            {
                foreach (var skill in ResumeParserData.SegSkills[i].OperationalSkills)
                {
                    allSkillSet.Add(skill);
                }
            }

            allSkillSet.Sort(SkillMonthCompare);

            foreach (var skill in allSkillSet)
            {
                string tempTemplate = SkillTemplateFile;

                tempTemplate = tempTemplate.Replace("{{ExperienceInMonth}}", skill.ExperienceInMonth);
                tempTemplate = tempTemplate.Replace("{{Skill}}", skill.Skill);

                allSkills += tempTemplate;
            }
            SkillTemplateFileField = SkillTemplateFileField.Replace("{{AllSkills}}", allSkills);
        }

        private static int SkillMonthCompare(SkillSet a, SkillSet b)
        {
            int aMonth = 0;
            int bMonth = 0;
            int.TryParse(a.ExperienceInMonth, out aMonth);
            int.TryParse(b.ExperienceInMonth, out bMonth);
            return aMonth.CompareTo(bMonth) * -1;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Word = Microsoft.Office.Interop.Word;
using Graph = Microsoft.Office.Interop.Graph;
using Nigeria_Reg.Models;
using Nigeria_Reg.Security;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nigeria_Reg.Controllers
{
    public class OnePagerController : Controller
    {
        cccmDBEntities db = new cccmDBEntities();
        //
        // GET: /OnePager/
        [AuthorizeRoles("Admin", "Manager", "Can Run Reports")]
        public ActionResult GetOnePager(string date1)
        {
            ViewBag.Active = "one";

            //ExportWord();

            ViewBag.Exception = null;

            //setViewBag
            //setViewBag();

            return View("ViewOnePager");
        }

        [HttpPost]
        public JsonResult getSSIDs(string date1)
        {
            DateTime sdate = DateTime.Parse(date1);

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            int week = cal.GetWeekOfYear(sdate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            string weekNoYear = string.Format("{0}-{1}", week, sdate.Year);

            var surveys = db.vw_surveys
                .Where(x => x.weekNoYear == weekNoYear)
                .Select(y => new {
                    ssid = y.SSID,
                    siteName = y.SSID+":"+y.SiteName//string.Format("{0}-{1}",y.SSID, y.SiteName)
                }).ToList();

            var ssids = new SelectList(surveys, "ssid", "siteName");

            //var lgas = new SelectList(db.tlkp_Lga.Where(x => x.state_code.Equals(id)), "lga_code", "lga_name").ToList();
            return Json(ssids, JsonRequestBehavior.AllowGet);
            //return null;
        }

        private void setViewBag()
        {
            //List<SelectListItem> states = new List<SelectListItem>();

            //states.Add(new SelectListItem { Value = "", Text = "[Select State]", Selected = true });

            ////Errors errors_ =

            //foreach (vw_tlkp_State st_ in db.vw_tlkp_State)
            //{
            //    //if (new Errors(dtm_.phase).canGenerateReport())
            //    //{
            //    states.Add(new SelectListItem { Value = st_.state_code.ToString(), Text = st_.state_name });
            //    //}
            //}


            //ViewBag.StatesList = states;
        }

        //worddoc.close(ref false, ref missing, ref missing);
        private Stream GetStream(Word.Document worddoc)
        {
            //Stream fs = new MemoryStream();
            //byte[] bytes = File.ReadAllBytes(worddoc);
            ////fs.Write(bytesxx, 0, bytesxx.length)
           
            ////worddoc.get
            //fs.Position = 0;
            //return fs;
            return null;
        }

        //free the memory -- garbage collector
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }



        int[] some_colors = { 12, 3, 33, 10, 48, 37 };

        public ActionResult ExportWord(string ssid, string date1, string tokenId)
        {
            DateTime dt = DateTime.Parse(date1);

            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            int weekno = cal.GetWeekOfYear(dt, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            string weekNoYear = string.Format("{0}-{1}", weekno, dt.Year);


            vw_surveys s = db.vw_surveys.Where(x => x.WeekNo == weekno && x.weekNoYear == weekNoYear && x.SSID == ssid).FirstOrDefault();
            // string sql = "SELECT * FROM vw_surveys s WHERE s.WeekNo = @strWeekNo@ AND s.WeekNoYear = '@strWeekNoYear@'@strTem@;";
            vw_Basicinfo b = db.vw_Basicinfo.Where(x => x.SSID == ssid).FirstOrDefault(); ;

            spGetDemoTrend_Result[] res = db.spGetDemoTrend(ssid, weekno).ToArray();

            //count age_group
            int t1, t1_5, t6_12, t13_17, t18_59, t60p, tpct, totF, totM;

            //computations
            t1 = (s.f_lt1 ?? 0) + (s.m_lt1 ?? 0);
            t1_5 = (s.f_1_5 ?? 0) + (s.m_1_5 ?? 0);
            t6_12 = (s.f_6_12 ?? 0) + (s.m_6_12 ?? 0);
            t13_17 = (s.f_13_17 ?? 0) + (s.m_13_17 ?? 0);
            t18_59 = (s.f_18_59 ?? 0) + (s.m_18_59 ?? 0);
            t60p = (s.f_60p ?? 0) + (s.m_60p ?? 0);

            tpct = t1 + t1_5 + t6_12 + t13_17 + t18_59 + t60p;

            totF = (s.f_lt1 ?? 0) + (s.f_1_5 ?? 0) + (s.f_6_12 ?? 0) +
                  (s.f_13_17 ?? 0) + (s.f_18_59 ?? 0) + (s.f_60p ?? 0);
            totM = (s.m_lt1 ?? 0) + (s.m_1_5 ?? 0) + (s.m_6_12 ?? 0) +
                (s.m_13_17 ?? 0) + (s.m_18_59 ?? 0) + (s.m_60p ?? 0);


            int[] breakdown_tots = { t1, t1_5, t6_12, t13_17, t18_59, t60p };
            string[] breakdown_labels = { "'<1'", "'1 - 5'", "'6 - 12'", "'13 - 17'", "'18 - 59'", "'60+'" };
            int[] agg_tots = { tpct, totF, totM };
            int[] margins = { 10, 10, 20, 35, 10, 10 };

            int[] gender_colors = { 3, 41 };

            object missing = System.Reflection.Missing.Value;
            object readOnly = false;
            object isVisible = true;
            
            var applicationWord = new Word.Application();

            Word.Document worddoc;

            object fileName = HttpContext.Server.MapPath("~/ReportXlsx/template.doc");

            worddoc = applicationWord.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref  missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                
            try
            {
                worddoc.Activate();


                int i = 1;

                //worddoc.Tables[i].Columns[3].Cells[2].Range.Text = "Site Name: " & rs("SiteName").Value & "  ===  Week: " & rs("WeekNo").Value
                docHeaderSection(s, worddoc, 1);

                //applicationWord.Visible = true;

                householdFiguresSection(s, totF, totM, worddoc, 2);

                //i = 5;
                siteBasicInfoSection(s, b, worddoc, 5);

                i = 6;

                string[] WeekDate;
                string[] N;

                trendOfIDPsSection(res, applicationWord, worddoc, 6, out WeekDate, out N);

                drawGraph1(applicationWord, worddoc, WeekDate, N);

                i = 7;
                surveyDetailsSection(s, worddoc, 7);

                demographicSection(s, t1, t1_5, t6_12, t13_17, t18_59, t60p, totF, totM, applicationWord, worddoc, 7);


                i = 8;
                ageSexBreakdownSection(t1, t1_5, t6_12, t13_17, t18_59, t60p, tpct, totF, totM, applicationWord, worddoc, 8);
                //======================================

                drawGraph2(applicationWord, worddoc, breakdown_tots, breakdown_labels, agg_tots, margins);

                drawGraph3(applicationWord, worddoc, agg_tots, gender_colors);

                i = 9;//Table CCCM

                cccmEsNFISection(s, applicationWord, worddoc, 9);

                washSection(s, applicationWord, worddoc, 11);

                protectionSection(s, applicationWord, worddoc, 13);

                educationSection(s, applicationWord, worddoc, 15);

                healthNutritionSection(s, applicationWord, worddoc, 17);

                i = 6;// 'Table Survey Details

                worddoc.Tables[i].Columns[1].Cells[3].Select();

                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 20);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);
                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 1);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);
                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 5);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);
                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 1);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);
                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 5);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);
                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 1);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);
                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 5);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);
                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 1);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);
                applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 6);
                applicationWord.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);

                //applicationWord.Visible = true;


                //  worddoc.savea
                //using (var entryStream = demoFile.Open())
                //{ }


                IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
                String date = DateTime.Now.ToShortDateString();

                date = date.Replace("/", ".");

                date = formatStrDate(date);

                //String ReportName = "cm_tracker_one_pager";
                string myName = Server.UrlEncode(ssid + "_" + weekNoYear + "_" + date + ".doc");

                // string myName = Server.UrlEncode(ReportName + ".xlsx");
                //MemoryStream stream = new MemoryStream();
                object fname = Path.GetTempFileName();
                worddoc.SaveAs(ref fname, ref missing, ref missing, ref missing, ref missing);
                    //new MemoryStream();
                //worddoc.SaveAs(stream);
                //GetStream(worddoc).CopyTo(stream);

                object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
                worddoc.Application.Quit(ref saveChanges, ref missing, ref missing);
                Marshal.ReleaseComObject(worddoc);

                byte[] byteArray = ReadFile((string)fname); 
                //MemoryStream ms = new MemoryStream();
                //fs.CopyTo(ms);

                

                
                //worddoc.Close();
                //GC.SuppressFinalize(worddoc);
                //applicationWord
                //.Dispose();

                Response.Clear();

                var cookie = new HttpCookie("fileDownloadToken", tokenId);
                Response.AppendCookie(cookie);

                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + myName);
                Response.ContentType = "application/vnd.ms-word";
                Response.BinaryWrite(byteArray);
                Response.End();

                Response.Flush();

                //Call stored procedure to add user activity here
                db.spAddUserActivity(User.Identity.Name, "Generate report (one pager)", "[User]='" + User.Identity.Name + " generated report for [Week]='"
                    + weekNoYear + "' and [SSID]=" + ssid + "'.", DateTime.Now);

                return File(byteArray, "application/vnd.ms-word", myName);

            }
            catch (COMException ex)
            {
                
            }
            finally {
                //applicationWord.Quit(ref missing, ref missing, ref missing);
                GC.SuppressFinalize(worddoc);
                GC.SuppressFinalize(applicationWord);
            }
            //Guid parameter, 




            //string filePath = "~/ReportXlsx/template.doc";



            return null;
        }


        private byte[] ReadFile(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            int length = Convert.ToInt32(fs.Length);
            byte[] data = new byte[length];
            fs.Read(data, 0, length);
            fs.Close();
            return data;
        }

        public MemoryStream SerializeToStream(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public object DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object objectType = formatter.Deserialize(stream);
            return objectType;
        } 

        //Dog myDog = new Dog();
        //myDog.Name= "Foo";
        //myDog.Color = DogColor.Brown;
        //MemoryStream stream = SerializeToStream(myDog)
        //Dog newDog = (Dog)DeserializeFromStream(stream);

        private string formatStrDate(string date)
        {
            string[] datepart = date.Split('.');

            if (datepart[0].Length == 1)
                datepart[0] = "0" + datepart[0];

            if (datepart[1].Length == 1)
                datepart[1] = "0" + datepart[1];

            return string.Join(".", datepart);
        }

        private void healthNutritionSection(vw_surveys s, Word.Application applicationWord, Word.Document worddoc, int i)
        {
            // '============== HEALTH & NUTRITION ====================

            worddoc.Tables[i].Columns[2].Cells[2].Range.Text = ifstrnull(s.SupplementaryFeeding_chYN);

            worddoc.Tables[i].Columns[2].Cells[2].Select();

            yesNoColoredInfo(applicationWord, s.SupplementaryFeeding_chYN);


            worddoc.Tables[i].Columns[2].Cells[3].Range.Text = ifstrnull(s.SupplementaryFeeding_moYN);

            worddoc.Tables[i].Columns[2].Cells[3].Select();

            yesNoColoredInfo(applicationWord, s.SupplementaryFeeding_moYN);

            worddoc.Tables[i].Columns[2].Cells[4].Range.Text = ifstrnull(s.VaccinationYN);

            worddoc.Tables[i].Columns[2].Cells[4].Select();

            yesNoColoredInfo(applicationWord, s.VaccinationYN);

            worddoc.Tables[i].Columns[2].Cells[5].Range.Text = ifstrnull(s.HealthStucture_EstablishmentYN);

            worddoc.Tables[i].Columns[2].Cells[5].Select();

            yesNoColoredInfo(applicationWord, s.HealthStucture_EstablishmentYN);

            worddoc.Tables[i].Columns[2].Cells[6].Range.Text = ifstrnull(s.MedicalReferralsYN);

            worddoc.Tables[i].Columns[2].Cells[6].Select();

            yesNoColoredInfo(applicationWord, s.MedicalReferralsYN);

            worddoc.Tables[i].Columns[5].Cells[2].Range.Text = ifstrnull(s.FoodDistributionYN);

            worddoc.Tables[i].Columns[5].Cells[2].Select();

            yesNoColoredInfo(applicationWord, s.FoodDistributionYN);

            worddoc.Tables[i].Columns[5].Cells[3].Range.Text = ifstrnull(s.Other_activity_gYN);

            worddoc.Tables[i].Columns[5].Cells[3].Select();

            yesNoColoredInfo(applicationWord, s.Other_activity_gYN);


            worddoc.Tables[i].Columns[5].Cells[4].Range.Text = ifstrnull(s.HealthcareDelivery);

            worddoc.Tables[i].Columns[5].Cells[4].Select();

            yesNoColoredInfo(applicationWord, s.HealthcareDelivery);


            worddoc.Tables[i].Columns[5].Cells[5].Range.Text = ifstrnull(s.HeathFacLoc);

            worddoc.Tables[i].Columns[5].Cells[6].Range.Text = ifstrnull(s.MealsPerDay);

            //Comments HEALTH & NUTRITION
            i = i + 1;
            worddoc.Tables[i].Columns[2].Cells[1].Range.Text = ifstrnull(s.notes_g);
        }

        private void educationSection(vw_surveys s, Word.Application applicationWord, Word.Document worddoc, int i)
        {
            //============== EDUCATION ====================
            worddoc.Tables[i].Columns[2].Cells[2].Range.Text = ifstrnull(s.SchoolEstablishmentYN);

            worddoc.Tables[i].Columns[2].Cells[2].Select();

            yesNoColoredInfo(applicationWord, s.SchoolEstablishmentYN);

            worddoc.Tables[i].Columns[2].Cells[3].Range.Text = ifstrnull(s.MaterialDistributionYN);

            worddoc.Tables[i].Columns[2].Cells[3].Select();

            yesNoColoredInfo(applicationWord, s.MaterialDistributionYN);

            worddoc.Tables[i].Columns[2].Cells[4].Range.Text = ifstrnull(s.TrainingForTeachersYN);

            worddoc.Tables[i].Columns[2].Cells[4].Select();

            yesNoColoredInfo(applicationWord, s.TrainingForTeachersYN);

            worddoc.Tables[i].Columns[2].Cells[5].Range.Text = ifstrnull(s.Other_activity_fYN);

            worddoc.Tables[i].Columns[2].Cells[5].Select();

            yesNoColoredInfo(applicationWord, s.Other_activity_fYN);

            worddoc.Tables[i].Columns[5].Cells[2].Range.Text = ifstrnull(s.Perc_child_att_sch);

            worddoc.Tables[i].Columns[5].Cells[3].Range.Text = ifstrnull(s.Perc_avail_instruct_mat);

            worddoc.Tables[i].Columns[5].Cells[4].Range.Text = ifstrnull(s.Perc_child_access_edu_fac);

            //Comments EDUCATION
            i = i + 1;
            worddoc.Tables[i].Columns[2].Cells[1].Range.Text = ifstrnull(s.notes_f);

        }

        private void protectionSection(vw_surveys s, Word.Application applicationWord, Word.Document worddoc, int i)
        {
            //============== PROTECTION ====================
            worddoc.Tables[i].Columns[2].Cells[2].Range.Text = string.IsNullOrEmpty(s.ProtectionMonitoringYN) ? "" : s.ProtectionMonitoringYN;

            worddoc.Tables[i].Columns[2].Cells[2].Select();

            yesNoColoredInfo(applicationWord, s.ProtectionMonitoringYN);


            worddoc.Tables[i].Columns[2].Cells[3].Range.Text = string.IsNullOrEmpty(s.FocusGroup_discussYN) ? "" : s.FocusGroup_discussYN;

            worddoc.Tables[i].Columns[2].Cells[3].Select();

            yesNoColoredInfo(applicationWord, s.FocusGroup_discussYN);

            worddoc.Tables[i].Columns[2].Cells[4].Range.Text = string.IsNullOrEmpty(s.RecreationalActivities_wmYN) ? "" : s.RecreationalActivities_wmYN;

            worddoc.Tables[i].Columns[2].Cells[4].Select();

            yesNoColoredInfo(applicationWord, s.RecreationalActivities_wmYN);


            worddoc.Tables[i].Columns[2].Cells[5].Range.Text = string.IsNullOrEmpty(s.RecreationalActivities_chYN) ? "" : s.RecreationalActivities_chYN;

            worddoc.Tables[i].Columns[2].Cells[5].Select();

            yesNoColoredInfo(applicationWord, s.RecreationalActivities_chYN);

            worddoc.Tables[i].Columns[5].Cells[2].Range.Text = string.IsNullOrEmpty(s.Trainings_wmYN) ? "" : s.Trainings_wmYN;

            worddoc.Tables[i].Columns[5].Cells[2].Select();

            yesNoColoredInfo(applicationWord, s.Trainings_wmYN);

            worddoc.Tables[i].Columns[5].Cells[3].Range.Text = string.IsNullOrEmpty(s.Trainings_chYN) ? "" : s.Trainings_chYN;

            worddoc.Tables[i].Columns[5].Cells[3].Select();

            yesNoColoredInfo(applicationWord, s.Trainings_chYN);


            worddoc.Tables[i].Columns[5].Cells[4].Range.Text = string.IsNullOrEmpty(s.SecurityOnSite) ? "" : s.SecurityOnSite;

            worddoc.Tables[i].Columns[5].Cells[4].Select();

            yesNoColoredInfo(applicationWord, s.SecurityOnSite);

            worddoc.Tables[i].Columns[5].Cells[5].Range.Text = string.IsNullOrEmpty(s.IdpRelToSec) ? "" : s.IdpRelToSec;

            worddoc.Tables[i].Columns[5].Cells[5].Select();

            yesNoColoredInfo(applicationWord, s.IdpRelToSec);

            //Comments PROTECTION
            i = i + 1;
            worddoc.Tables[i].Columns[2].Cells[1].Range.Text = string.IsNullOrEmpty(s.notes_e) ? "" : s.notes_e;

        }

        private void washSection(vw_surveys s, Word.Application applicationWord, Word.Document worddoc, int i)
        {
            //============== WASH ====================

            worddoc.Tables[i].Columns[2].Cells[2].Range.Text = string.IsNullOrEmpty(s.Water_distYN) ? "" : s.Water_distYN;

            worddoc.Tables[i].Columns[2].Cells[2].Select();

            yesNoColoredInfo(applicationWord, s.Water_distYN);

            worddoc.Tables[i].Columns[2].Cells[3].Range.Text = string.IsNullOrEmpty(s.WaterStorage_fac_distYN) ? "" : s.WaterStorage_fac_distYN;

            worddoc.Tables[i].Columns[2].Cells[3].Select();

            yesNoColoredInfo(applicationWord, s.WaterStorage_fac_distYN);


            worddoc.Tables[i].Columns[2].Cells[4].Range.Text = string.IsNullOrEmpty(s.Install_repair_latYN) ? "" : s.Install_repair_latYN;

            yesNoColoredInfo(applicationWord, s.Install_repair_latYN);

            worddoc.Tables[i].Columns[2].Cells[5].Range.Text = string.IsNullOrEmpty(s.Install_repair_washing_facYN) ? "" : s.Install_repair_washing_facYN;

            worddoc.Tables[i].Columns[2].Cells[5].Select();

            yesNoColoredInfo(applicationWord, s.Install_repair_washing_facYN);

            worddoc.Tables[i].Columns[5].Cells[2].Range.Text = string.IsNullOrEmpty(s.Install_repair_garbage_dispYN) ? "" : s.Install_repair_garbage_dispYN;

            worddoc.Tables[i].Columns[5].Cells[2].Select();

            yesNoColoredInfo(applicationWord, s.Install_repair_garbage_dispYN);

            worddoc.Tables[i].Columns[5].Cells[3].Range.Text = string.IsNullOrEmpty(s.Install_repair_drainage_systYN) ? "" : s.Install_repair_drainage_systYN;

            yesNoColoredInfo(applicationWord, s.Install_repair_drainage_systYN);

            worddoc.Tables[i].Columns[5].Cells[4].Range.Text = string.IsNullOrEmpty(s.Hygiene_promo_campaignYN) ? "" : s.Hygiene_promo_campaignYN;

            worddoc.Tables[i].Columns[5].Cells[4].Select();

            yesNoColoredInfo(applicationWord, s.Hygiene_promo_campaignYN);

            worddoc.Tables[i].Columns[5].Cells[5].Range.Text = string.IsNullOrEmpty(s.SolidWasteSt) ? "" : s.SolidWasteSt;

            worddoc.Tables[i].Columns[5].Cells[5].Select();

            if (Array.Exists(new string[2] { "Very clean", "Clean" }, x => x == s.SolidWasteSt))
            {
                applicationWord.Selection.Font.ColorIndex = Word.WdColorIndex.wdGreen;

            }
            else if (Array.Exists(new string[3] { "Not so clean", "Dirty", "Very dirty" }, x => x == s.SolidWasteSt))
            {
                applicationWord.Selection.Font.ColorIndex = Word.WdColorIndex.wdRed;
            }

            //'Comments WASH
            i = i + 1;
            worddoc.Tables[i].Columns[2].Cells[1].Range.Text = string.IsNullOrEmpty(s.notes_d) ? "" : s.notes_d;

        }

        private void cccmEsNFISection(vw_surveys s, Word.Application applicationWord, Word.Document worddoc, int i)
        {
            //============== CCCM/ES/NFI ====================
            worddoc.Tables[i].Columns[2].Cells[3].Range.Text = ifstrnull(s.shelter_kits_distYN);

            worddoc.Tables[i].Columns[2].Cells[3].Select();

            yesNoColoredInfo(applicationWord, s.shelter_kits_distYN);

            worddoc.Tables[i].Columns[2].Cells[4].Range.Text = ifstrnull(s.tent_distYN);

            worddoc.Tables[i].Columns[2].Cells[4].Select();

            yesNoColoredInfo(applicationWord, s.tent_distYN);

            worddoc.Tables[i].Columns[2].Cells[5].Range.Text = ifstrnull(s.nfi_distYN);

            worddoc.Tables[i].Columns[2].Cells[5].Select();

            yesNoColoredInfo(applicationWord, s.nfi_distYN);

            worddoc.Tables[i].Columns[5].Cells[3].Range.Text = ifstrnull(s.hyg_kits_distYN);

            worddoc.Tables[i].Columns[5].Cells[3].Select();

            yesNoColoredInfo(applicationWord, s.hyg_kits_distYN);

            worddoc.Tables[i].Columns[5].Cells[4].Range.Text = ifstrnull(s.shelter_repairsYN);

            worddoc.Tables[i].Columns[5].Cells[4].Select();

            yesNoColoredInfo(applicationWord, s.shelter_repairsYN);

            worddoc.Tables[i].Columns[5].Cells[5].Range.Text = ifstrnull(s.Other_activity_cYN);

            worddoc.Tables[i].Columns[5].Cells[5].Select();

            yesNoColoredInfo(applicationWord, s.Other_activity_cYN);

            worddoc.Tables[i].Columns[2].Cells[6].Range.Text = ifstrnull(s.MostNeededNFI);
            worddoc.Tables[i].Columns[5].Cells[6].Range.Text = ifstrnull(s.perc_HHs_living_outside);

            //Comments CCCM/ES/NFI

            i = i + 1;//'
            worddoc.Tables[i].Columns[2].Cells[1].Range.Text = ifstrnull(s.notes_c);
            // return i;
        }

        private static void yesNoColoredInfo(Word.Application applicationWord, string val)
        {
            switch (val)
            {
                case "Yes":
                    applicationWord.Selection.Font.ColorIndex = Word.WdColorIndex.wdGreen; break;//wdGreen;
                case "No":
                    applicationWord.Selection.Font.ColorIndex = Word.WdColorIndex.wdRed; break;
            }
        }



        private void ageSexBreakdownSection(int t1, int t1_5, int t6_12, int t13_17, int t18_59, int t60p, int tpct, int totF, int totM, Word.Application applicationWord, Word.Document worddoc, int i)
        {
            //bar chart table (vchart)
            worddoc.Tables[i].Columns[1].Cells[3].Range.Text = "100%";
            worddoc.Tables[i].Columns[2].Cells[3].Range.Text = Math.Round((double)t1 / (double)tpct * 100, 2) + "%";
            worddoc.Tables[i].Columns[3].Cells[3].Range.Text = Math.Round((double)t1_5 / (double)tpct * 100, 2) + "%";
            worddoc.Tables[i].Columns[4].Cells[3].Range.Text = Math.Round((double)t6_12 / (double)tpct * 100, 2) + "%";
            worddoc.Tables[i].Columns[5].Cells[3].Range.Text = Math.Round((double)t13_17 / (double)tpct * 100, 2) + "%";
            worddoc.Tables[i].Columns[6].Cells[3].Range.Text = Math.Round((double)t18_59 / (double)tpct * 100, 2) + "%";
            worddoc.Tables[i].Columns[7].Cells[3].Range.Text = Math.Round((double)t60p / (double)tpct * 100, 2) + "%";
            //Pie chart table (vchart2)
            worddoc.Tables[i].Columns[10].Cells[3].Range.Text = "100%";
            worddoc.Tables[i].Columns[11].Cells[3].Range.Text = Math.Round((double)totM / (double)tpct * 100, 2) + "%";
            worddoc.Tables[i].Columns[12].Cells[3].Range.Text = Math.Round((double)totF / (double)tpct * 100, 2) + "%";

            worddoc.Tables[i].Columns[1].Cells[3].Select();
            applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 1);
        }

        private void demographicSection(vw_surveys s, int t1, int t1_5, int t6_12, int t13_17, int t18_59, int t60p, int totF, int totM, Word.Application applicationWord, Word.Document worddoc, int i)
        {
            worddoc.Tables[i].Columns[5].Cells[3].Range.Text = ifnull(s.f_lt1);
            worddoc.Tables[i].Columns[6].Cells[3].Range.Text = ifnull(s.f_1_5);
            worddoc.Tables[i].Columns[7].Cells[3].Range.Text = ifnull(s.f_6_12);
            worddoc.Tables[i].Columns[8].Cells[3].Range.Text = ifnull(s.f_13_17);
            worddoc.Tables[i].Columns[9].Cells[3].Range.Text = ifnull(s.f_18_59);
            worddoc.Tables[i].Columns[10].Cells[3].Range.Text = ifnull(s.f_60p);
            worddoc.Tables[i].Columns[11].Cells[3].Range.Text = totF.ToString();

            worddoc.Tables[i].Columns[5].Cells[4].Range.Text = ifnull(s.m_lt1);
            worddoc.Tables[i].Columns[6].Cells[4].Range.Text = ifnull(s.m_1_5);
            worddoc.Tables[i].Columns[7].Cells[4].Range.Text = ifnull(s.m_6_12);
            worddoc.Tables[i].Columns[8].Cells[4].Range.Text = ifnull(s.m_13_17);
            worddoc.Tables[i].Columns[9].Cells[4].Range.Text = ifnull(s.m_18_59);
            worddoc.Tables[i].Columns[10].Cells[4].Range.Text = ifnull(s.m_60p);
            worddoc.Tables[i].Columns[11].Cells[4].Range.Text = totM.ToString();

            worddoc.Tables[i].Columns[5].Cells[5].Range.Text = t1.ToString();
            worddoc.Tables[i].Columns[6].Cells[5].Range.Text = t1_5.ToString();
            worddoc.Tables[i].Columns[7].Cells[5].Range.Text = t6_12.ToString();
            worddoc.Tables[i].Columns[8].Cells[5].Range.Text = t13_17.ToString();
            worddoc.Tables[i].Columns[9].Cells[5].Range.Text = t18_59.ToString();
            worddoc.Tables[i].Columns[10].Cells[5].Range.Text = t60p.ToString();
            worddoc.Tables[i].Columns[11].Cells[5].Range.Text = s.inds.ToString();

            worddoc.Tables[i].Columns[4].Cells[1].Select();
            worddoc.Tables[i].Columns[4].Cells[1].Range.Text = "DEMOGRAPHICS (Sex and Age breakdown)";
            applicationWord.Selection.EndKey(Unit: Word.WdUnits.wdLine, Extend: 1);//wdExtend);
            applicationWord.Selection.MoveRight(Unit: Word.WdUnits.wdCharacter, Count: 6, Extend: 1);//wdExtend);
            applicationWord.Selection.Cells.Merge();
            applicationWord.Selection.Font.Size = 8;
            applicationWord.Selection.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
        }

        private static void surveyDetailsSection(vw_surveys s, Word.Document worddoc, int i)
        {
            worddoc.Tables[i].Columns[2].Cells[2].Range.Text = s.hhs.ToString();
            worddoc.Tables[i].Columns[2].Cells[3].Range.Text = s.inds.ToString();

            DateTime dt2 = DateTime.ParseExact(s.SurveyDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            worddoc.Tables[i].Columns[2].Cells[4].Range.Text = dt2.ToString("dd-MMM-yy");
            worddoc.Tables[i].Columns[2].Cells[5].Range.Text = "Week no. " + s.WeekNo;
            worddoc.Tables[i].Columns[2].Cells[6].Range.Text = string.IsNullOrEmpty(s.PopChg) ? "" : s.PopChg;
            worddoc.Tables[i].Columns[2].Cells[7].Range.Text = s.PopChg == "No change" ? "" : string.IsNullOrEmpty(s.ReasonChg) ? "" : s.ReasonChg;
        }

        private static void trendOfIDPsSection(spGetDemoTrend_Result[] res, Word.Application applicationWord, Word.Document worddoc, int i, out string[] WeekDate, out string[] N)
        {
            WeekDate = new string[res.Length]; N = new string[res.Length];

            for (int j = 0; j < res.Length; j++)
            {
                DateTime dt = DateTime.ParseExact(res[j].SurveyDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                WeekDate[j] = dt.ToString("dd-MMM-yy");
                worddoc.Tables[i].Columns[j + 1].Cells[2].Range.Text = WeekDate[j];
                N[j] = res[j].inds <= 0 ? "-" : res[j].inds.ToString();
                worddoc.Tables[i].Columns[j + 1].Cells[3].Range.Text = N[j];
            }


            worddoc.Tables[i].Columns[1].Cells[3].Select();
            applicationWord.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 1);
            applicationWord.Selection.Font.Size = 8;
        }

        private static void siteBasicInfoSection(vw_surveys s, vw_Basicinfo b, Word.Document worddoc, int i)
        {
            worddoc.Tables[i].Columns[2].Cells[2].Range.Text = s.SSID;
            worddoc.Tables[i].Columns[2].Cells[3].Range.Text = s.SiteName;
            worddoc.Tables[i].Columns[2].Cells[4].Range.Text = s.Lat.ToString();
            worddoc.Tables[i].Columns[2].Cells[5].Range.Text = s.Lon.ToString();
            worddoc.Tables[i].Columns[2].Cells[6].Range.Text = string.IsNullOrEmpty(b.Type) ? "N/A" : b.Type;
            worddoc.Tables[i].Columns[2].Cells[7].Range.Text = string.IsNullOrEmpty(b.classification) ? "N/A" : b.classification;
            worddoc.Tables[i].Columns[2].Cells[8].Range.Text = s.State;
            worddoc.Tables[i].Columns[2].Cells[9].Range.Text = s.LGA;
            worddoc.Tables[i].Columns[2].Cells[10].Range.Text = s.Ward;
        }

        private static void householdFiguresSection(vw_surveys s, int totF, int totM, Word.Document worddoc, int i)
        {
            // table HH/Ind / female / male
            worddoc.Tables[i].Columns[1].Cells[1].Range.Text = s.hhs.ToString();
            worddoc.Tables[i].Columns[2].Cells[1].Range.Text = s.inds.ToString();

            i++;
            worddoc.Tables[i].Columns[1].Cells[1].Range.Text = totF.ToString();
            i++;
            worddoc.Tables[i].Columns[1].Cells[1].Range.Text = totM.ToString();
        }

        private static void docHeaderSection(vw_surveys s, Word.Document worddoc, int i)
        {
            worddoc.Tables[i].Columns[3].Cells[2].Range.Text = s.SiteName + "  ===  Week " + s.WeekNo;
        }

        private void drawGraph3(Word.Application wordapp, Word.Document worddoc, int[] agg_tots, int[] gender_colors)
        {

            Graph.Chart vchart0;
            Graph.DataSheet vsheet0;
            Graph.Application vapp0;

            initGraphParams(wordapp, worddoc, out vchart0, out vsheet0, out vapp0, 1);

            vsheet0.Cells[2, 2].Value = Math.Round((double)agg_tots[1] / (double)agg_tots[0] * 100, 2) + "%";
            vsheet0.Cells[2, 3].Value = Math.Round((double)agg_tots[2] / (double)agg_tots[0] * 100, 2) + "%";
            vsheet0.Cells[1, 2].Value = "Female";
            vsheet0.Cells[1, 3].Value = "Male";

            //'   ChartFormat vchart, vsheet


            vchart0.Application.PlotBy = Graph.XlRowCol.xlRows;
            vchart0.ChartType = Graph.XlChartType.xl3DPieExploded;//xlColumnStacked; 'xlLine
            vchart0.Width = 360;
            vchart0.Height = 150;
            vchart0.HasLegend = false;
            vchart0.Elevation = 15;
            vchart0.Perspective = 30;
            vchart0.Rotation = 330;
            vchart0.RightAngleAxes = false;
            vchart0.PlotArea.ClearFormats();
            vchart0.PlotArea.Width = 200;
            vchart0.PlotArea.Height = 80;
            vchart0.PlotArea.Left = 50;
            vchart0.SeriesCollection(1).Points(1).Explosion = -3;
            vchart0.SeriesCollection(1).HasDataLabels = true;

            vchart0.SeriesCollection(1).ApplyDataLabels(LegendKey: true, AutoText: false, HasLeaderLines: false, ShowSeriesName: false, ShowCategoryName: true, ShowValue: true, ShowPercentage: false, ShowBubbleSize: false);
            vchart0.SeriesCollection(1).DataLabels.Font.Size = 8;
            vchart0.SeriesCollection(1).DataLabels.Font.Shadow = false;

            for (int j = 1; j < gender_colors.Length + 1; j++)
            {

                vchart0.SeriesCollection(1).Points(j).Border.Weight = Word.XlBorderWeight.xlThin;//xlThin
                vchart0.SeriesCollection(1).Points(j).Border.LineStyle = -4142;//Word.XlLineStyle.xlLineStyleNone;//xlNone
                vchart0.SeriesCollection(1).Points(j).Fill.OneColorGradient(Style: 3, Variant: j, Degree: 0.231372549019608);
                //vchart0.SeriesCollection(1).Points(1).Fill.OneColorGradient Style:=msoGradientDiagonalUp, Variant:=1, Degree:=0.231372549019608
                vchart0.SeriesCollection(1).Points(j).Fill.Visible = true;
                vchart0.SeriesCollection(1).Points(j).Fill.ForeColor.SchemeColor = gender_colors[j - 1];
                //if (j == 1)
                //{
                //    vchart0.SeriesCollection(1).DataLabels.Font.Size = 8;
                //    vchart0.SeriesCollection(1).DataLabels.Font.Shadow = false;
                //}

            }


            freeGraphObjects(vchart0, vsheet0, vapp0);

            int i = 8;
            worddoc.Tables[i].Columns[1].Cells[3].Select();
            wordapp.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 1);
            wordapp.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);


            worddoc.Tables[i].Columns[1].Cells[1].Select();
            wordapp.Selection.MoveRight(Unit: Word.WdUnits.wdCharacter, Count: 5, Extend: 1);//wdExtend)
            wordapp.Selection.Cells.Merge();
            wordapp.Selection.Font.Size = 8;

            wordapp.Selection.MoveRight(Unit: Word.WdUnits.wdCharacter, Count: 4);
            wordapp.Selection.EndKey(Unit: Word.WdUnits.wdLine, Extend: 1);//wdExtend)
            wordapp.Selection.MoveRight(Unit: Word.WdUnits.wdCharacter, Count: 2, Extend: 1);//=wdExtend
            wordapp.Selection.Cells.Merge();
            wordapp.Selection.Font.Size = 8;
        }

        public string ifnull(int? num)
        {
            if (num == null || num == 0) return "-";

            else return num.ToString();
        }

        public string ifstrnull(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str;
        }

        //https://blogs.msdn.microsoft.com/vsod/2009/06/15/creating-charts-in-word-and-powerpoint-using-newly-introduced-object-model-in-office-2007-service-pack-2/

        public void drawGraph1(Word.Application wordapp, Word.Document worddoc, string[] WeekDate, string[] N)
        {
            //Word.Chart wdChart = doc.InlineShapes.AddChart(Microsoft.Office.Cha.XlChartType.xl3DColumn , ref missing).Chart;
            //Word.Chart vchart0;

            Graph.Chart vchart0;
            Graph.DataSheet vsheet0;
            Graph.Application vapp0;
            initGraphParams(wordapp, worddoc, out vchart0, out vsheet0, out vapp0);

            for (int j = 1; j < WeekDate.Length + 1; j++)
            {
                vsheet0.Cells[1, j + 1].Value = WeekDate[j - 1];
                vsheet0.Cells[2, j + 1].Value = N[j - 1];
            }

            //Set fill color

            int[] Color = new int[WeekDate.Length];

            for (int j = 0; j < Color.Length; j++)
            {
                Color[j] = j < some_colors.Length ? some_colors[j] : 15; //default color is 15 in case of not enough color in some_colors array
            }

            vchart0.Application.PlotBy = Graph.XlRowCol.xlRows;
            vchart0.ChartType = Graph.XlChartType.xlColumnStacked;//xlColumnStacked; 'xlLine
            vchart0.Width = 360;
            vchart0.Height = 140;
            vchart0.HasLegend = false;
            vchart0.HasAxis[Graph.XlAxisType.xlCategory, Graph.XlAxisGroup.xlPrimary] = true;
            vchart0.HasAxis[Graph.XlAxisType.xlValue, Graph.XlAxisGroup.xlPrimary] = false;
            vchart0.Axes(Graph.XlAxisType.xlCategory).HasMajorGridlines = false;
            vchart0.Axes(Graph.XlAxisType.xlCategory).HasMinorGridlines = false;
            vchart0.Axes(Graph.XlAxisType.xlValue).HasMajorGridlines = false;
            vchart0.Axes(Graph.XlAxisType.xlValue).HasMinorGridlines = false;
            vchart0.Axes(Graph.XlAxisType.xlCategory, Graph.XlAxisGroup.xlPrimary).CategoryType = Graph.XlCategoryType.xlCategoryScale;//xlCategoryScale;
            vchart0.PlotArea.ClearFormats();
            vchart0.SeriesCollection(1).HasDataLabels = true;

            for (int j = 1; j < Color.Length + 1; j++)
            {
                vchart0.SeriesCollection(1).Points(j).DataLabel.Top = vchart0.SeriesCollection(1).Points(j).DataLabel.Top - 25;
                vchart0.SeriesCollection(1).Points(j).Fill.OneColorGradient(Style: 2, Variant: 4, Degree: 0.231372549019608);
                vchart0.SeriesCollection(1).Points(j).Fill.Visible = true;
                vchart0.SeriesCollection(1).Points(j).Fill.ForeColor.SchemeColor = Color[j - 1];
                vchart0.SeriesCollection(1).Points(j).Border.LineStyle = Word.WdLineStyle.wdLineStyleNone;//-4142; //xlNone

            }

            freeGraphObjects(vchart0, vsheet0, vapp0);

            int i = 6;
            worddoc.Tables[i].Columns[1].Cells[3].Select();
            wordapp.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 1);
            wordapp.Selection.Delete(Unit: Word.WdUnits.wdCharacter, Count: 1);

        }

        private static void initGraphParams(Word.Application wordapp, Word.Document worddoc, out Graph.Chart vchart0, out Graph.DataSheet vsheet0, out Graph.Application vapp0, int gn = 0)
        {



            Word.Range rng0;
            Word.OLEFormat oleF0;

            rng0 = wordapp.Selection.Range;

            if (gn == 0)
            {
                rng0.Collapse(Direction: Word.WdCollapseDirection.wdCollapseEnd);

                rng0.InsertAfter("\r");
                rng0.Collapse(Direction: Word.WdCollapseDirection.wdCollapseEnd);//rng0.Collapse( Direction:0);
            }
            oleF0 = worddoc.InlineShapes.AddOLEObject(ClassType: "MSGraph.Chart.8", Range: rng0).OLEFormat;

            oleF0.DoVerb();

            vchart0 = oleF0.Object;
            vapp0 = vchart0.Application;
            vsheet0 = vapp0.DataSheet; vsheet0.Cells.Clear();

            vsheet0.Cells.Clear();
        }

        public void drawGraph2(Word.Application wordapp, Word.Document worddoc, int[] breakdown_tots, string[] breakdown_labels, int[] agg_tots, int[] margins)
        {

            Graph.Chart vchart0;
            Graph.DataSheet vsheet0;
            Graph.Application vapp0;
            initGraphParams(wordapp, worddoc, out vchart0, out vsheet0, out vapp0);

            for (int j = 0; j < breakdown_tots.Length; j++)
            {
                vsheet0.Cells[2, j + 2].Value = Math.Round((double)breakdown_tots[j] / (double)agg_tots[0] * 100, 2) + "%";
                vsheet0.Cells[1, j + 2].Value = breakdown_labels[j];

            }

            vchart0.Application.PlotBy = Graph.XlRowCol.xlRows;
            vchart0.ChartType = Graph.XlChartType.xlColumnStacked;//xlColumnStacked; 'xlLine
            vchart0.Width = 350;
            vchart0.Height = 150;
            vchart0.HasLegend = false;
            vchart0.HasAxis[Graph.XlAxisType.xlCategory, Graph.XlAxisGroup.xlPrimary] = true;
            vchart0.HasAxis[Graph.XlAxisType.xlValue, Graph.XlAxisGroup.xlPrimary] = false;
            vchart0.Axes(Graph.XlAxisType.xlCategory).HasMajorGridlines = false;
            vchart0.Axes(Graph.XlAxisType.xlCategory).HasMinorGridlines = false;
            vchart0.Axes(Graph.XlAxisType.xlValue).HasMajorGridlines = false;
            vchart0.Axes(Graph.XlAxisType.xlValue).HasMinorGridlines = false;
            vchart0.Axes(Graph.XlAxisType.xlCategory, Graph.XlAxisGroup.xlPrimary).CategoryType = Graph.XlCategoryType.xlCategoryScale;//xlCategoryScale;
            vchart0.PlotArea.ClearFormats();
            vchart0.SeriesCollection(1).HasDataLabels = true;

            for (int j = 1; j < breakdown_tots.Length + 1; j++)
            {

                vchart0.SeriesCollection(1).Points(j).DataLabel.Top = vchart0.SeriesCollection(1).Points(j).DataLabel.Top - margins[j - 1];
                vchart0.SeriesCollection(1).Points(j).Fill.OneColorGradient(Style: 2, Variant: 4, Degree: 0.231372549019608);
                vchart0.SeriesCollection(1).Points(j).Fill.Visible = true;
                vchart0.SeriesCollection(1).Points(j).Fill.ForeColor.SchemeColor = j < some_colors.Length ? some_colors[j - 1] : 15; //default color is 15 in case of not enough color in some_colors array
                vchart0.SeriesCollection(1).Points(j).Border.LineStyle = Word.WdLineStyle.wdLineStyleNone;

            }

            freeGraphObjects(vchart0, vsheet0, vapp0);


            int i = 8;
            worddoc.Tables[i].Columns[1].Cells[3].Select();
            wordapp.Selection.MoveDown(Unit: Word.WdUnits.wdLine, Count: 2); //'chg 2 to 1
            wordapp.Selection.MoveRight(Unit: Word.WdUnits.wdCharacter, Count: 1);


        }

        private static void freeGraphObjects(Graph.Chart vchart0, Graph.DataSheet vsheet0, Graph.Application vapp0)
        {
            vapp0.Quit();
            GC.SuppressFinalize(vsheet0);
            GC.SuppressFinalize(vapp0);
            GC.SuppressFinalize(vchart0);
        }
        

	}
}
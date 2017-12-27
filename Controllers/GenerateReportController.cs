using ClosedXML.Excel;
using Nigeria_Reg.Helpers;
using Nigeria_Reg.Models;
using Nigeria_Reg.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Word = Microsoft.Office.Interop.Word;



namespace Nigeria_Reg.Controllers
{
    public class GenerateReportController : Controller
    {
        cccmDBEntities db = new cccmDBEntities();

        //
        // GET: /GenerateReport/
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Active = "rep";
            return View();
        }

        [AuthorizeRoles("Admin", "Manager", "Can Run Reports")]
        public ActionResult ViewReport()
        {
            ViewBag.Active = "rep";

            ViewBag.Exception = null;

            //setViewBag
            setViewBag();

            return View();
        }

        public ActionResult ExportWord()
        {

            vw_surveys s = db.vw_surveys.Where(x => x.WeekNo == 7 && x.weekNoYear == "7-2017" && x.SSID == "AD_S001").FirstOrDefault();
            // string sql = "SELECT * FROM vw_surveys s WHERE s.WeekNo = @strWeekNo@ AND s.WeekNoYear = '@strWeekNoYear@'@strTem@;";
            vw_Basicinfo b = db.vw_Basicinfo.Where(x => x.SSID == "AD_S001").FirstOrDefault(); ;

            spGetDemoTrend_Result[] res = db.spGetDemoTrend("AD_S001", 7).ToArray();

             //count age_group
            int t1, t1_5, t6_12, t13_17, t18_59, t60p, tpct, totF, totM;

            //computations
            t1 = s.f_lt1??0 + s.m_lt1?? 0;
            t1_5 = s.f_1_5??0 + s.m_1_5??0;
            t6_12 =s.f_6_12??0 + s.m_6_12??0;
            t13_17 =s.f_13_17??0 + s.m_13_17??0;
            t18_59 =s.f_18_59??0 + s.m_18_59??0;
            t60p =s.f_60p??0 + s.m_60p??0;

            tpct = t1 + t1_5 + t6_12 + t13_17 + t18_59 + t60p;

            totF =s.f_lt1??0 + s.f_1_5??0 + s.f_6_12??0+
                  s.f_13_17??0+ s.f_18_59??0+ s.f_60p??0;
            totM = s.m_lt1 ?? 0 + s.m_1_5 ?? 0 + s.m_6_12 ?? 0 +
                s.m_13_17 ?? 0 + s.m_18_59 ?? 0 + s.m_60p ?? 0;


            object missing = System.Reflection.Missing.Value;
            object readOnly = false;
            object isVisible = true;
            object fileName = HttpContext.Server.MapPath("~/ReportXlsx/template.doc");

            var applicationWord = new Word.Application();
            //applicationWord.Visible = true;

            Word.Document worddoc;
            try
            {
                worddoc = applicationWord.Documents.Open(ref fileName, ref missing, ref readOnly, ref missing, ref  missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing, ref missing);
                worddoc.Activate();

                int i = 1;
                //worddoc.Tables[i].Columns(3).Cells(2).Range.Text = "Site Name: " & rs("SiteName").Value & "  ===  Week: " & rs("WeekNo").Value
                worddoc.Tables[i].Columns[3].Cells[2].Range.Text = s.SiteName + "  ===  Week " + s.WeekNo;
                applicationWord.Visible = true;

                i = 2; // table HH/Ind / female / male
                worddoc.Tables[i].Columns[1].Cells[1].Range.Text = s.hhs.ToString();
                worddoc.Tables[i].Columns[2].Cells[1].Range.Text = s.inds.ToString();
    
                i = 3;
                worddoc.Tables[i].Columns[1].Cells[1].Range.Text = totF.ToString();
                i = 4;
                worddoc.Tables[i].Columns[1].Cells[1].Range.Text = totM.ToString();
    
                i = 5;
                worddoc.Tables[i].Columns[2].Cells[2].Range.Text = s.SSID;
                worddoc.Tables[i].Columns[2].Cells[3].Range.Text = s.SiteName;
                worddoc.Tables[i].Columns[2].Cells[4].Range.Text = s.Lat.ToString();
                worddoc.Tables[i].Columns[2].Cells[5].Range.Text = s.Lon.ToString();
                worddoc.Tables[i].Columns[2].Cells[6].Range.Text = string.IsNullOrEmpty(b.Type)?"N/A":b.Type;
                worddoc.Tables[i].Columns[2].Cells[7].Range.Text = string.IsNullOrEmpty(b.classification) ? "N/A" : b.classification;
                worddoc.Tables[i].Columns[2].Cells[8].Range.Text = s.State;
                worddoc.Tables[i].Columns[2].Cells[9].Range.Text = s.LGA;
                worddoc.Tables[i].Columns[2].Cells[10].Range.Text = s.Ward;

                i=6; string[] WeekDate = new string[6]; string[] N = new string[6];
                
                for(int j=0; j< res.Length; j++)
                {
                    DateTime dt = DateTime.ParseExact(res[j].SurveyDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    WeekDate[j] = dt.ToString("dd-MMM-yy");
                    worddoc.Tables[i].Columns[j+1].Cells[2].Range.Text = WeekDate[j];
                    N[j] = res[j].inds<=0?"-":res[j].inds.ToString();
                    worddoc.Tables[i].Columns[j+1].Cells[3].Range.Text = N[j];
                }


                worddoc.Tables[i].Columns[1].Cells[3].Select();
                applicationWord.Selection.MoveDown(Unit:5, Count:1);
                applicationWord.Selection.Font.Size = 8;


            }
            catch (COMException ex){
                applicationWord.Quit(ref missing, ref missing, ref missing);
            }
            //Guid parameter, 
            
            //var workbook = new XLWorkbook(path);

           

            string filePath = "~/ReportXlsx/template.doc";

            
            
            return null;
        }
            
        
        [HttpPost]
        [AuthorizeRoles("Admin", "Manager", "Can Run Reports")]
        public ActionResult ExportExcel(string stateId, string date1, string tokenId)
        {

            ExportWord();

            ViewBag.Active = "rep";
            ViewBag.Exception = null;
            try
            {
                int? s = -1, esnfi = -1, w_gaps = -1, w_serv = -1, edu = -1, prot = -1, nut = -1,
                    weekno=0;
                
                DateTime dt = DateTime.Parse(date1);

                if (dt != null) weekno = DateCalc.GetWeekNo(dt);

                string weekno_year = weekno + "-" + dt.Year.ToString();

                var tmp = db.vw_surveys.FirstOrDefault();

                s = db.vw_surveys.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(z => z.WeekNo == weekno).Count();

                esnfi = db.vw_ESNFI_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                        Where(z => z.weekNoYear == weekno_year).
                        Where(z => z.WeekNo == weekno).Count();

                w_gaps = db.vw_WASH_Gaps.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                         Where(z => z.weekNoYear == weekno_year).
                         Where(z => z.WeekNo == weekno).Count();

                w_serv = db.vw_WASH_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                         Where(z => z.weekNoYear == weekno_year).
                         Where(z => z.WeekNo == weekno).Count();

                edu = db.vw_EDU_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                      Where(z => z.weekNoYear == weekno_year).
                      Where(z => z.WeekNo == weekno).Count();

                prot = db.vw_PROT_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                       Where(z => z.weekNoYear == weekno_year).
                        Where(z => z.WeekNo == weekno).Count();

                nut = db.vw_HEALTH_NUT_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                      Where(z => z.weekNoYear == weekno_year).
                      Where(z => z.WeekNo == weekno).Count();

                if (s <= 0)
                {
                    ViewBag.errorData = "1";
                    //ModelState.AddModelError("stateId", "*");
                    throw new MyException("No data for current filter! Please try again...");
                }


                //Guid parameter, 
                string path = HttpContext.Server.MapPath("~/ReportXlsx/template.xlsx");
                var workbook = new XLWorkbook(path);

                var ws1 = workbook.Worksheet(1); //sitrep
                var ws2 = workbook.Worksheet(2); //surveys
                var ws3 = workbook.Worksheet(3); //esnfi
                var ws4 = workbook.Worksheet(4); //w_gaps
                var ws5 = workbook.Worksheet(5); //w_serv
                var ws6 = workbook.Worksheet(6); //edu

                var ws7 = workbook.Worksheet(7); //prot
                var ws8 = workbook.Worksheet(8); //nut

                var ws9 = workbook.Worksheet(9); //comments

                var ws10 = workbook.Worksheet(10); //data
                var ws11 = workbook.Worksheet(11); //trend
                var ws12 = workbook.Worksheet(12); //trend

                // Change the background color of the headers
                //var rngHeaders = ws.Range("A1:AB1");
                // rngHeaders.Style.Fill.BackgroundColor = XLColor.LightSalmon;

                string date_range="", state_;

                state_ = db.tblSites.Where(y => y.state_code == stateId).Select(x => x.tlkp_State.state_name).FirstOrDefault().InitialCapital();
                    //.Where(z => dt1 <= z.DateS && z.DateS <= dt2).Select(x => x.State).FirstOrDefault().InitialCapital();

                //set report dates range (E5:I6)

                date_range = DateCalc.FirstDayOfWeek(dt) < DateCalc.LastDayOfWeek(dt) ?
                             DateCalc.FirstDayOfWeek(dt).ToString() + " to " + DateCalc.LastDayOfWeek(dt) + " " + DateCalc.GetMonthName(dt) + " " + dt.Year + " (Week " + weekno + ")" :
                             DateCalc.FirstDayOfWeek(dt).ToString() + " " + DateCalc.GetMonthName(dt.Month - 1) + " to " + DateCalc.LastDayOfWeek(dt) + " " +
                             DateCalc.GetMonthName(dt) + " " + dt.Year + " (Week " + weekno + ")";              

                if (s > 0)
                {
                    ws1.Range("E5:I6").Value = date_range;
                    //ws1.Range("K5:L5").Value = DateTime.Now.ToLongDateString();

                    ws2.Cell(4, 1).InsertData(
                    db.vw_surveys.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(y => y.WeekNo == weekno).
                    Select(x => new
                    {
                        x.SurveyID,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.Lat,
                        x.Lon,
                        x.State,
                        x.LGA,
                        x.Ward,
                        x.hhs,
                        x.inds,
                        x.m_lt1,
                        x.f_lt1,
                        x.m_1_5,
                        x.f_1_5,
                        x.m_6_12,
                        x.f_6_12,
                        x.m_13_17,
                        x.f_13_17,
                        x.m_18_59,
                        x.f_18_59,
                        x.m_60p,
                        x.f_60p,
                        x.PopChg,
                        x.ReasonChg,
                        x.Arr_Gvt_transYN,
                        x.Arr_Mil_transYN,
                        x.Arr_Ind_arr_campYN,
                        x.Arr_Ind_arr_HCYN,
                        x.Arr_Ind_arr_originYN,
                        x.Arr_othYN,
                        x.Dep_Gvt_transYN,
                        x.Dep_Mil_transYN,
                        x.Dep_Ind_ret_originYN,
                        x.Dep_Ind_move_oth_campYN,
                        x.Dep_Ind_move_HCYN,
                        x.Dep_othYN,
                        x.shelter_kits_distYN,
                        x.tent_distYN,
                        x.nfi_distYN,
                        x.hyg_kits_distYN,
                        x.shelter_repairsYN,
                        x.Other_activity_cYN,
                        x.MostNeededNFI,
                        x.SecMostNeededNFI,
                        x.perc_HHs_living_outside,
                        x.c_refer_srv,
                        x.Water_distYN,
                        x.WaterStorage_fac_distYN,
                        x.Install_repair_latYN,
                        x.Install_repair_washing_facYN,
                        x.Install_repair_garbage_dispYN,
                        x.Install_repair_drainage_systYN,
                        x.Hygiene_promo_campaignYN,
                        x.Other_activity_dYN,
                        x.d_refer_gap,
                        x.EvacuationFreq,
                        x.SolidWasteDisp,
                        x.DispMean,
                        x.d_refer_srv,
                        x.ProtectionMonitoringYN,
                        x.FocusGroup_discussYN,
                        x.RecreationalActivities_wmYN,
                        x.RecreationalActivities_chYN,
                        x.Trainings_wmYN,
                        x.Trainings_chYN,
                        x.Other_activity_eYN,
                        x.SecurityOnSite,
                        x.IdpRelToSec,
                        x.SecurityInc,
                        x.nb_childAbuse,
                        x.nb_childLabor,
                        x.nb_sexualExploitation,
                        x.nb_psychosocial,
                        x.nb_forcedLabor,
                        x.nb_GBV,
                        x.nb_generalViolence,
                        x.UAC_m,
                        x.UAC_f,
                        x.SP_ch_m,
                        x.SP_ch_f,
                        x.UAC_foster_m,
                        x.UAC_foster_f,
                        x.ChildAbuseYN,
                        x.ChildLaborYN,
                        x.SexualExploitationYN,
                        x.PsychosocialYN,
                        x.ForcedLaborYN,
                        x.GBVYN,
                        x.GeneralViolenceYN,
                        x.UACYN,
                        x.SP_chYN,
                        x.SexualAbusePrev,
                        x.AgeRange,
                        x.GBV_Cases,
                        x.e_refer_srv,
                        x.SchoolEstablishmentYN,
                        x.MaterialDistributionYN,
                        x.TrainingForTeachersYN,
                        x.Other_activity_fYN,
                        x.Perc_child_att_sch,
                        x.Perc_avail_instruct_mat,
                        x.Perc_child_access_edu_fac,
                        x.f_refer_srv,
                        x.SupplementaryFeeding_chYN,
                        x.SupplementaryFeeding_moYN,
                        x.VaccinationYN,
                        x.HealthStucture_EstablishmentYN,
                        x.MedicalReferralsYN,
                        x.FoodDistributionYN,
                        x.Other_activity_gYN,
                        x.HealthcareDelivery,
                        x.DiseaseOutbreak,
                        x.Disease,
                        x.HeathFacLoc,
                        x.FoodAccess,
                        x.MarketAccess,
                        x.MealsPerDay,
                        x.g_refer_srv

                    }));

                    //Filling data tab
                    ws10.Cell(2, 1).InsertData(
                    db.vw_surveys.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(y => y.WeekNo == weekno).
                    Select(x => new
                    {
                        x.SurveyID,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.Lat,
                        x.Lon,
                        x.State,
                        x.LGA,
                        x.Ward,
                        x.hhs,
                        x.inds,
                        x.m_lt1,
                        x.f_lt1,
                        x.m_1_5,
                        x.f_1_5,
                        x.m_6_12,
                        x.f_6_12,
                        x.m_13_17,
                        x.f_13_17,
                        x.m_18_59,
                        x.f_18_59,
                        x.m_60p,
                        x.f_60p,
                        x.PopChg,
                        x.ReasonChg,
                        x.Arr_Gvt_transYN,
                        x.Arr_Mil_transYN,
                        x.Arr_Ind_arr_campYN,
                        x.Arr_Ind_arr_HCYN,
                        x.Arr_Ind_arr_originYN,
                        x.Arr_othYN,
                        x.Dep_Gvt_transYN,
                        x.Dep_Mil_transYN,
                        x.Dep_Ind_ret_originYN,
                        x.Dep_Ind_move_oth_campYN,
                        x.Dep_Ind_move_HCYN,
                        x.Dep_othYN,
                        x.shelter_kits_distYN,
                        x.tent_distYN,
                        x.nfi_distYN,
                        x.hyg_kits_distYN,
                        x.shelter_repairsYN,
                        x.Other_activity_cYN,
                        x.MostNeededNFI,
                        x.SecMostNeededNFI,
                        x.perc_HHs_living_outside,
                        x.Water_distYN,
                        x.WaterStorage_fac_distYN,
                        x.Install_repair_latYN,
                        x.Install_repair_washing_facYN,
                        x.Install_repair_garbage_dispYN,
                        x.Install_repair_drainage_systYN,
                        x.Hygiene_promo_campaignYN,
                        x.Other_activity_dYN,
                        x.EvacuationFreq,
                        x.SolidWasteDisp,
                        x.DispMean,
                        x.ProtectionMonitoringYN,
                        x.FocusGroup_discussYN,
                        x.RecreationalActivities_wmYN,
                        x.RecreationalActivities_chYN,
                        x.Trainings_wmYN,
                        x.Trainings_chYN,
                        x.Other_activity_eYN,
                        x.SecurityOnSite,
                        x.IdpRelToSec,
                        x.SecurityInc,
                        x.nb_childAbuse,
                        x.nb_childLabor,
                        x.nb_sexualExploitation,
                        x.nb_psychosocial,
                        x.nb_forcedLabor,
                        x.nb_GBV,
                        x.nb_generalViolence,
                        x.UAC_m,
                        x.UAC_f,
                        x.SP_ch_m,
                        x.SP_ch_f,
                        x.UAC_foster_m,
                        x.UAC_foster_f,
                        x.ChildAbuseYN,
                        x.ChildLaborYN,
                        x.SexualExploitationYN,
                        x.PsychosocialYN,
                        x.ForcedLaborYN,
                        x.GBVYN,
                        x.GeneralViolenceYN,
                        x.UACYN,
                        x.SP_chYN,
                        x.SexualAbusePrev,
                        x.AgeRange,
                        x.GBV_Cases,
                        x.SchoolEstablishmentYN,
                        x.MaterialDistributionYN,
                        x.TrainingForTeachersYN,
                        x.Other_activity_fYN,
                        x.Perc_child_att_sch,
                        x.Perc_avail_instruct_mat,
                        x.Perc_child_access_edu_fac,
                        x.SupplementaryFeeding_chYN,
                        x.SupplementaryFeeding_moYN,
                        x.VaccinationYN,
                        x.HealthStucture_EstablishmentYN,
                        x.MedicalReferralsYN,
                        x.FoodDistributionYN,
                        x.Other_activity_gYN,
                        x.HealthcareDelivery,
                        x.DiseaseOutbreak,
                        x.Disease,
                        x.HeathFacLoc,
                        x.FoodAccess,
                        x.MarketAccess,
                        x.MealsPerDay

                    }));

                }
                //Trend tab
                if (string.IsNullOrEmpty(stateId))
                {
                    ws11.Cell(2, 1).InsertData(
                    db.spGetDemoTrend4AllSitrep(weekno, dt.Year).
                    Select(x => new
                    {
                        x.WeekNo,
                        x.weekNoYear,
                        x.Week,
                        x.inds

                    }));

                }
                else
                {
                    ws11.Cell(2, 1).InsertData(
                    db.spGetDemoTrendPerStateSitrep(stateId, weekno, dt.Year).
                    Select(x => new
                    {
                        x.WeekNo,
                        x.weekNoYear,
                        x.Week,
                        x.idps

                    }));
                }
                //ESNFI services
                if (esnfi > 0)
                {
                    ws3.Cell(2, 1).InsertData(
                    db.vw_ESNFI_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(z => z.WeekNo == weekno).
                    Select(x => new
                    {
                        x.SurveyID,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.SubCat,
                        x.Description,
                        x.QTTY,
                        x.Lat,
                        x.Lon,
                        x.Provider,
                        x.Remark
                    }));

                }

                //Wash gaps
                if (w_gaps > 0)
                {
                    ws4.Cell(2, 1).InsertData(
                    db.vw_WASH_Gaps.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(z => z.WeekNo == weekno).
                    Select(x => new
                    {
                        x.s_id,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.Provider,
                        x.nb_f_lat,
                        x.nb_nf_lat,
                        x.nb_f_bat,
                        x.nb_nf_bat,
                        x.nb_f_wp,
                        x.nb_nf_wp

                    }));

                }

                //Wash services
                if (w_serv > 0)
                {
                    ws5.Cell(2, 1).InsertData(
                    db.vw_WASH_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(z => z.WeekNo == weekno).
                    Select(x => new
                    {
                        x.SurveyID,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.SubCat,
                        x.Description,
                        x.QTTY,
                        x.Lat,
                        x.Lon,
                        x.Provider,
                        x.Remark

                    }));

                }

                //Education services
                if (edu > 0)
                {
                    ws6.Cell(2, 1).InsertData(
                    db.vw_EDU_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(z => z.WeekNo == weekno).
                    Select(x => new
                    {
                        x.SurveyID,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.Description,
                        x.QTTY,
                        x.Lat,
                        x.Lon,
                        x.Provider,
                        x.Remark

                    }));

                }

                //Protection services
                if (prot > 0)
                {
                    ws7.Cell(2, 1).InsertData(
                    db.vw_PROT_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(z => z.WeekNo == weekno).
                    Select(x => new
                    {
                        x.SurveyID,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.SubCat,
                        x.QTTY,
                        x.Lat,
                        x.Lon,
                        x.Provider,
                        x.Remark

                    }));

                }

                //Health & nutrition services
                if (nut > 0)
                {
                    ws8.Cell(2, 1).InsertData(
                    db.vw_HEALTH_NUT_Services.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(z => z.WeekNo == weekno).
                    Select(x => new
                    {
                        x.SurveyID,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.SubCat,
                        x.Description,
                        x.QTTY,
                        x.Lat,
                        x.Lon,
                        x.Provider,
                        x.Remark

                    }));

                }

                if (s > 0)
                {
                    ws9.Cell(2, 1).InsertData(
                    db.vw_surveys.Where(x => x.state_code == stateId || (string.IsNullOrEmpty(stateId))).
                    Where(z => z.weekNoYear == weekno_year).
                    Where(z => z.WeekNo == weekno).
                    Select(x => new
                    {
                        x.SurveyID,
                        x.SurveyDate,
                        x.WeekNo,
                        x.SSID,
                        x.SiteName,
                        x.notes_c,
                        x.notes_d,
                        x.notes_e,
                        x.notes_f,
                        x.notes_g

                    }));
                }

                IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
                String date = DateTime.Now.ToShortDateString();

                date = date.Replace("/", ".");

                if (date.Length == 9)
                    date = "0" + date;
                else if (date.Length == 8)
                    date = "0" + date.Left(1) + "0" + date.Right(6);

                String ReportName = "cccm_tracker_week";
                string myName = Server.UrlEncode(ReportName + weekno + "-" + dt.Year + "_report_" + 
                                (String.IsNullOrEmpty(state_)? "" : state_ + "_") + date + ".xlsx");

                // string myName = Server.UrlEncode(ReportName + ".xlsx");
                MemoryStream stream = new MemoryStream();
                GetStream(workbook).CopyTo(stream);

                byte[] byteArray = stream.ToArray();

                workbook.Dispose();

                Response.Clear();

                var cookie = new HttpCookie("fileDownloadToken", tokenId);
                Response.AppendCookie(cookie);

                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + myName);
                Response.ContentType = "application/vnd.ms-excel";
                Response.BinaryWrite(stream.ToArray());
                Response.End();

                Response.Flush();

                date_range = DateCalc.FirstDayOfWeek(dt) < DateCalc.LastDayOfWeek(dt) ?
                DateCalc.FirstDayOfWeek(dt).ToString() + " to " + DateCalc.LastDayOfWeek(dt) + " " + DateCalc.GetMonthName(dt) + " " + dt.Year:
                DateCalc.FirstDayOfWeek(dt).ToString() + " " + DateCalc.GetMonthName(dt.Month - 1) + " to " + DateCalc.LastDayOfWeek(dt) + " " +
                DateCalc.GetMonthName(dt) + " " + dt.Year;

                //Call stored procedure to add user activity here
                db.spAddUserActivity(User.Identity.Name, "Generate report", "[User]='" + User.Identity.Name +  " generated report for [Week]='"
                    + date_range + (String.IsNullOrEmpty(state_) ? "'" : "'and [State]=" + state_ + "'") + ".", DateTime.Now);

                return File(byteArray, "application/vnd.ms-excel", myName);


            }
            catch (/*MyException*/ Exception e)
            {
                ViewBag.Exception = e.Message;
                setViewBag();
                return View("ViewReport");
            }
            finally { }

        }


        private void setViewBag()
        {
            List<SelectListItem> states = new List<SelectListItem>();

            states.Add(new SelectListItem { Value = "", Text = "[Select State]", Selected = true });

            //Errors errors_ =

            foreach (vw_tlkp_State st_ in db.vw_tlkp_State)
            {
                //if (new Errors(dtm_.phase).canGenerateReport())
                //{
                states.Add(new SelectListItem { Value = st_.state_code.ToString(), Text = st_.state_name });
                //}
            }


            ViewBag.StatesList = states;
        }

        private Stream GetStream(XLWorkbook excelWorkbook)
        {
            Stream fs = new MemoryStream();
            excelWorkbook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }


        //free the memory -- garbage collector
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
	}
}
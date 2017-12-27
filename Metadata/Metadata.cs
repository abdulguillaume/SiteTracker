using Nigeria_Reg.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nigeria_Reg.Metadata
{
    public class tblSurvey_MD
    {
        [Key]
        public int? SurveyID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Survey Date")]
        public string SurveyDate { get; set; }

        [Display(Name = "Week No")]
        public int WeekNo { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Enumerator Name")]
        public string EnumeratorName { get; set; }

        [Display(Name = "Enumerator phone")]
        public string EnumeratorPhone { get; set; }

    }

    public class tblSite_MD
    {

        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Site ID")]
        public string SSID { get; set; }
        [Required(ErrorMessage = "*", AllowEmptyStrings = false)]
        [Display(Name = "Site Name")]
        public string SiteName { get; set; }
        [Display(Name = "GPS Latitude")]
        public Nullable<double> Lat { get; set; }
        [Display(Name = "GPS Longitude")]
        public Nullable<double> Lon { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        public string state_code { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "LGA")]
        public string lga_code { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Ward")]
        public string ward_code { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Type of Site")]
        public int Type { get; set; }

        [Display(Name = "Other type")]
        [RequiredIf("Type", 5, "*")]
        public string Other_type { get; set; }

    
    }

    public class tblSectionG_MD
    {
        [Key]
        public int SurveyID { get; set; }

        [RequiredIf("b_Other", true, "*")]
        [StringLength(50, ErrorMessage = "Less than 50 characters.")]
        public string Other_activity_spec { get; set; }


        [UIHint("YesNo")]
        public Nullable<int> DiseaseOutbreak { get; set; }

        [RequiredIf("b_disease", true, "*")]
        public Nullable<int> WichDisease { get; set; }


        [RequiredIf("b_oth_disease", true, "*")]
        //[RequiredIf("WichDisease", 11, "*")]
        [StringLength(50, ErrorMessage = "Less than 50 characters.")]
        public string OtherDisease_spec { get; set; }

        [UIHint("YesNo")]
        public Nullable<int> MarketAccess { get; set; }

        [StringLength(255, ErrorMessage = "Less than 255 characters.")]
        [DataType(DataType.MultilineText)]
        public string ActivePartnersOnSite { get; set; }

        [DataType(DataType.MultilineText)]

        [StringLength(500, ErrorMessage = "Less than 500 characters.")]
        public string Comments { get; set; }

    }

    public class tblSectionF_MD
    {
        [Key]
        public int SurveyID { get; set; }

        [RequiredIf("b_Other", true, "*")]
        public string Other_activity_spec { get; set; }

        [UIHint("Perc")]
        public Nullable<int> perc_ch_attend_sch { get; set; }

        [UIHint("Perc")]
        public Nullable<int> perc_avail_instruction_mat { get; set; }

        [UIHint("Perc")]
        public Nullable<int> perc_ch_access_edu_fac { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

    }

    public class tblSectionE_MD
    {
        [Key]
        public int SurveyID { get; set; }

        [RequiredIf("b_other_activity", true, "*")]
        [StringLength(50, ErrorMessage = "Less than 50 characters.")]
        public string other_activity_spec { get; set; }

        //[Required(ErrorMessage = "*")]
        [UIHint("YesNo")]
        public Nullable<int> SecurityOnSite { get; set; }

        //[Required(ErrorMessage = "*")]
        [UIHint("YesNo")]
        public Nullable<int> SecIncidents { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_childAbuse { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_childLabor { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_sexualExploitation { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_psychosocial { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_forcedLabor { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_GBV { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_generalViolence { get; set; }


        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> UAC_m { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> UAC_f { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> SP_ch_m { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> SP_ch_f { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> UAC_foster_m { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> UAC_foster_f { get; set; }

        [RequiredIf("b_range", true, "*")]
        [StringLength(7, ErrorMessage = "Less than 7 characters.")]
        //[DisplayFormat(DataFormatString="{0:## - ##}", ApplyFormatInEditMode=true, NullDisplayText="")]
        public string Other_AgeRange { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }
        
    }

    public class tblSectionD_MD
    {
        [Key]
        public int SurveyID { get; set; }

        [RequiredIf("b_other_activity", true, "*")]
        public string other_activity_spec { get; set; }

        [RequiredIf("b_freq", true, "*")]
        //[RequiredIf("ToiletsEvacuationFreq", 5, "*")]
        [Display(Name = "When last?")]
        public string IfNone_LastEvacuation { get; set; }


        [RequiredIf("b_solidW", true, "*")]
        //[RequiredIf("SolidWasteDisposal", 4, "*")]
        public string OtherSolidWasteDisp { get; set; }


        [RequiredIf("b_WasteDisp", true, "*")]
        //[RequiredIf("WasteDisposal", 4, "*")]
        public string OtherWasteDisposal { get; set; }

        [StringLength(255, ErrorMessage = "Less than 255 characters.")]
        [DataType(DataType.MultilineText)]
        public string OtherAgenciesRespDisp { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_func_latrines { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_non_func_latrines { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_func_bathrooms { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_non_func_bathrooms { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_func_WP { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_non_func_WP { get; set; }

        [UIHint("CholeraPeriod")]
        public Nullable<int> LastCholera_case { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]

        [Range(0, 48, ErrorMessage = "Number of months between 0 and 48")]
        public Nullable<int> NbMonths { get; set; }

        [UIHint("SolidWaste_st")]
        public Nullable<int> SolidWaste_st { get; set; }

        [UIHint("Drainage_st")]
        public Nullable<int> Drainage_st { get; set; }

        [UIHint("AvgWaitingTime")]
        public Nullable<int> AvgWaitingTimeWP { get; set; }

        [Range(0, 600, ErrorMessage = "Number of minutes between 0 and 600")]
        public Nullable<int> NbMinutes { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> HandPump_NbUnit { get; set; }

        [Range(0, 600, ErrorMessage = "Number of working hours between 0 and 24")]
        public Nullable<int> HandPump_NbWorkingHrs { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> WaterTank2KL_NbUnit { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> WaterTank2KL_NbTimesFilled { get; set; }


        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> WaterTank3KL_NbUnit { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> WaterTank3KL_NbTimesFilled { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> WaterTank5KL_NbUnit { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> WaterTank5KL_NbTimesFilled { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> WaterTank10KL_NbUnit { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> WaterTank10KL_NbTimesFilled { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> Tap_NbUnit { get; set; }

        [Range(0, 600, ErrorMessage = "Number of working hours between 0 and 24")]
        public Nullable<int> Tap_NbWorkingHrs { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }

    }

    public class tblSectionC_MD
    {
        [Key]
        public int SurveyID { get; set; }


        [RequiredIf("b_other_activity", true, "*")]
        public string other_activity_spec { get; set; }

        //[Required(ErrorMessage = "*")]
        [UIHint("NeededNFI")]
        public Nullable<int> most_needed_nfi { get; set; }
        
        [RequiredIf("b_nfi1", true, "*")]
        //[RequiredIf("most_needed_nfi", 9, "*")]
        public string Other_needed_nfi1 { get; set; }

        //[Required(ErrorMessage = "*")]
        [UIHint("NeededNFI")]
        public Nullable<int> sec_most_needed_nfi { get; set; }


        [RequiredIf("b_nfi2", true, "*")]
        //[RequiredIf("sec_most_needed_nfi", 9, "*")]
        public string Other_needed_nfi2 { get; set; }

        [UIHint("Perc")]
        public Nullable<int> perc_hh_living_out { get; set; }

        [DataType(DataType.MultilineText)]
        public string comments { get; set; }

    }

    public class tblSectionB_MD
    {
        [Key]
        public int SurveyID { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[1-9]+[0-9]*", ErrorMessage = "number>0")]
        public int? hhs { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> m_lt1 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> f_lt1 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> m_1_5 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> f_1_5 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> m_6_12 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> f_6_12 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> m_13_17 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> f_13_17 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> m_18_59 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> f_18_59 { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> m_60p { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> f_60p { get; set; }


        [Required(ErrorMessage = "*")]
        //[UIHint("x")]
        public Nullable<int> pop_chg { get; set; }


        [RequiredIf("b_pop_chg", true, "Please specify the reason if increase/decrease.")]
        public Nullable<int> inc_dec_reason { get; set; }


        [RequiredIf("ck_Arr_oth", true, "*")]
        public string Arr_oth_spec { get; set; }


        [RequiredIf("ck_Dep_oth", true, "*")]
        public string Dep_oth_spec { get; set; }

    }

    public class tbl_EDU_Services_MD
    {

        //Check wheter row is empty or not
        public bool notEmpty { get; set; }

        [Display(Name = "Sub-Category")]
        public Nullable<int> SubCat { get; set; }

        [RegularExpression("^[1-9]+[0-9]*", ErrorMessage = "number>0")]
        public Nullable<int> QTTY { get; set; }

        [Display(Name = "Latitude")]
        public Nullable<double> Lat { get; set; }

        [Display(Name = "Longitude")]
        public Nullable<double> Lon { get; set; }

        [Display(Name = "Provided by?")]
        public Nullable<int> Provider { get; set; }
       
    }

    public class tbl_ESNFI_Services_MD
    {

        [Display(Name = "Sub-Category")]
        public Nullable<int> SubCat { get; set; }

        [RegularExpression("^[1-9]+[0-9]*", ErrorMessage = "number>0")]
        public Nullable<int> QTTY { get; set; }

        [Display(Name = "Latitude")]
        public Nullable<double> Lat { get; set; }

        [Display(Name = "Longitude")]
        public Nullable<double> Lon { get; set; }

        [Display(Name = "Provided by?")]
        public Nullable<int> Provider { get; set; }
       
    }

    public class tbl_HEALTH_NUT_Services_MD
    {

        [Display(Name = "Sub-Category")]
        public Nullable<int> SubCat { get; set; }

        [RegularExpression("^[1-9]+[0-9]*", ErrorMessage = "number>0")]
        public Nullable<int> QTTY { get; set; }

        [Display(Name = "Latitude")]
        public Nullable<double> Lat { get; set; }

        [Display(Name = "Longitude")]
        public Nullable<double> Lon { get; set; }

        [Display(Name = "Provided by?")]
        public Nullable<int> Provider { get; set; }

    }

    public class tbl_PROT_Services_MD
    {
        [Display(Name = "Sub-Category")]
        public Nullable<int> SubCat { get; set; }

        [RegularExpression("^[1-9]+[0-9]*", ErrorMessage = "number>0")]
        public Nullable<int> QTTY { get; set; }

        [Display(Name = "Latitude")]
        public Nullable<double> Lat { get; set; }

        [Display(Name = "Longitude")]
        public Nullable<double> Lon { get; set; }

        [Display(Name = "Provided by?")]
        public Nullable<int> Provider { get; set; }
        
    }

    public class tbl_WASH_Gaps_MD
    {
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Providing Agency")]
        public Nullable<int> ProvidingAgency { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_func_latrines { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_non_func_latrines { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_func_bathrooms { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_non_func_bathrooms { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_func_WP { get; set; }

        [RegularExpression("^[0-9]*", ErrorMessage = "number")]
        public Nullable<int> nb_non_func_WP { get; set; }


    }

    public class tbl_WASH_Services_MD
    {
        //[Required(ErrorMessage = "*")]
        [Display(Name = "Sub-Category")]
        public Nullable<int> SubCat { get; set; }

        [RegularExpression("^[1-9]+[0-9]*", ErrorMessage = "number>0")]
        public Nullable<int> QTTY { get; set; }

        [Display(Name = "Latitude")]
        public Nullable<double> Lat { get; set; }

        [Display(Name = "Longitude")]
        public Nullable<double> Lon { get; set; }

        [Display(Name = "Provided by?")]
        public Nullable<int> Provider { get; set; }

    }

    public class SYSUser_MD
    {
        [Required(ErrorMessage = "*")]
        [StringLength(10, ErrorMessage = "The {0} must between {2} and 10 characters long.", MinimumLength = 4)]
        [RegularExpression("^\\S+$", ErrorMessage = "No space allowed in username.")]
        [Display(Name = "User name")]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string HashedPassword { get; set; }

        public Guid PasswordSalt { get; set; }

    }


    public class SYSUserProfile_MD
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "Less than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Less than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "*")]
        public int Organization { get; set; }

        [StringLength(50, ErrorMessage = "Less than 50 characters.")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
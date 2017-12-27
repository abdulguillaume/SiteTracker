using Nigeria_Reg.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nigeria_Reg.Models
{
    [MetadataType(typeof(tblSurvey_MD))]
    public partial class tblSurvey
    { }

    [MetadataType(typeof(tblSite_MD))]
    public partial class tblSite
    { }

    [MetadataType(typeof(tblSectionG_MD))]
    public partial class tblSectionG
    {
        public bool b_SupplementaryFeeding_ch
        {
            get { return SupplementaryFeeding_ch == 1; }
            set { if (value) SupplementaryFeeding_ch = 1; else SupplementaryFeeding_ch = 0; }
        }

        public bool b_SupplementaryFeeding_mo
        {
            get { return SupplementaryFeeding_mo == 1; }
            set { if (value) SupplementaryFeeding_mo = 1; else SupplementaryFeeding_mo = 0; }
        }
        public bool b_Vaccination { get { return Vaccination == 1; } set { if (value) Vaccination = 1; else Vaccination = 0; } }

        public bool b_HealthStucture_Establishment
        {
            get { return HealthStucture_Establishment == 1; }
            set { if (value) HealthStucture_Establishment = 1; else HealthStucture_Establishment = 0; }
        }
        public bool b_MedicalReferrals
        {
            get { return MedicalReferrals == 1; }
            set { if (value) MedicalReferrals = 1; else MedicalReferrals = 0; }
        }

        public bool b_FoodDistribution
        {
            get { return FoodDistribution == 1; }
            set { if (value) FoodDistribution = 1; else FoodDistribution = 0; }
        }
        public bool b_Other { get { return Other == 1; } set { if (value) Other = 1; else Other = 0; } }
        public bool b_disease
        {
            get { return (DiseaseOutbreak == 1) ? true : false; }
            set { if (value) { ;} }
        }
        public bool b_oth_disease
        {
            get { return (WichDisease == 11) ? true : false; }
            set { if (value) { ;} }
        }
    }

    [MetadataType(typeof(tblSectionF_MD))]
    public partial class tblSectionF
    {
        public bool b_SchoolEstablishment
        {
            get { return SchoolEstablishment == 1; }
            set { if (value) SchoolEstablishment = 1; else SchoolEstablishment = 0; }
        }
        public bool b_MaterialDistribution
        {
            get { return MaterialDistribution == 1; }
            set { if (value) MaterialDistribution = 1; else MaterialDistribution = 0; }
        }
        public bool b_TrainingForTeachers
        {
            get { return TrainingForTeachers == 1; }
            set { if (value) TrainingForTeachers = 1; else TrainingForTeachers = 0; }
        }
        public bool b_Other
        {
            get { return Other == 1; }
            set { if (value) Other = 1; else Other = 0; }
        }
    }

    [MetadataType(typeof(tblSectionE_MD))]
    public partial class tblSectionE
    {
        public int? tot_UAC { get; set; }
        public int? tot_SP_ch { get; set; }
        public int? tot_UAC_f { get; set; }
        public int? tot_UAC_SP { get; set; }
        public bool b_ProtectionMonitoring
        {
            get { return ProtectionMonitoring == 1; }
            set { if (value) ProtectionMonitoring = 1; else ProtectionMonitoring = 0; }
        }
        public bool b_FocusGroup_discuss
        {
            get { return FocusGroup_discuss == 1; }
            set { if (value) FocusGroup_discuss = 1; else FocusGroup_discuss = 0; }
        }
        public bool b_RecreationalActivities_wm
        {
            get { return RecreationalActivities_wm == 1; }
            set { if (value) RecreationalActivities_wm = 1; else RecreationalActivities_wm = 0; }
        }
        public bool b_RecreationalActivities_ch
        {
            get { return RecreationalActivities_ch == 1; }
            set { if (value) RecreationalActivities_ch = 1; else RecreationalActivities_ch = 0; }
        }
        public bool b_Trainings_wm
        {
            get { return Trainings_wm == 1; }
            set { if (value) Trainings_wm = 1; else Trainings_wm = 0; }
        }
        public bool b_Trainings_ch
        {
            get { return Trainings_ch == 1; }
            set { if (value) Trainings_ch = 1; else Trainings_ch = 0; }
        }
        public bool b_other_activity
        {
            get { return other_activity == 1; }
            set { if (value) other_activity = 1; else other_activity = 0; }
        }
        public bool b_ChildAbuse
        {
            get { return ChildAbuse == 1; }
            set { if (value) ChildAbuse = 1; else ChildAbuse = 0; }
        }
        public bool b_ChildLabor
        {
            get { return ChildLabor == 1; }
            set { if (value) ChildLabor = 1; else ChildLabor = 0; }
        }
        public bool b_SexualExploitation
        {
            get { return SexualExploitation == 1; }
            set { if (value) SexualExploitation = 1; else SexualExploitation = 0; }
        }
        public bool b_Psychosocial
        {
            get { return Psychosocial == 1; }
            set { if (value) Psychosocial = 1; else Psychosocial = 0; }
        }
        public bool b_ForcedLabor
        {
            get { return ForcedLabor == 1; }
            set { if (value) ForcedLabor = 1; else ForcedLabor = 0; }
        }
        public bool b_GBV
        {
            get { return GBV == 1; }
            set { if (value) GBV = 1; else GBV = 0; }
        }
        public bool b_GeneralViolence
        {
            get { return GeneralViolence == 1; }
            set { if (value) GeneralViolence = 1; else GeneralViolence = 0; }
        }
        public bool b_UAC
        {
            get { return UAC == 1; }
            set { if (value) UAC = 1; else UAC = 0; }
        }
        public bool b_SP_ch
        {
            get { return SP_ch == 1; }
            set { if (value) SP_ch = 1; else SP_ch = 0; }
        }

        public bool b_range
        {
            get { return (Abused_ppl_AgeRange == 3) ? true : false; }
            set { if (value) { ;} }
        }
    }

    [MetadataType(typeof(tblSectionD_MD))]
    public partial class tblSectionD
    {
        public bool b_Water_dist { get { return Water_dist == 1; } set { if (value) Water_dist = 1; else Water_dist = 0; } }

        public bool b_WaterStorage_fac_dist
        {
            get { return WaterStorage_fac_dist == 1; }
            set { if (value) WaterStorage_fac_dist = 1; else WaterStorage_fac_dist = 0; }
        }

        public bool b_Install_repair_lat
        {
            get { return Install_repair_lat == 1; }
            set { if (value) Install_repair_lat = 1; else Install_repair_lat = 0; }
        }

        public bool b_Install_repair_washing_fac
        {
            get { return Install_repair_washing_fac == 1; }
            set { if (value) Install_repair_washing_fac = 1; else Install_repair_washing_fac = 0; }
        }
        public bool b_Install_repair_garbage_disp
        {
            get { return Install_repair_garbage_disp == 1; }
            set { if (value) Install_repair_garbage_disp = 1; else Install_repair_garbage_disp = 0; }
        }

        public bool b_Install_repair_drainage_syst
        {
            get { return Install_repair_drainage_syst == 1; }
            set { if (value) Install_repair_drainage_syst = 1; else Install_repair_drainage_syst = 0; }
        }

        public bool b_Hygiene_promo_campaign
        {
            get { return Hygiene_promo_campaign == 1; }
            set { if (value) Hygiene_promo_campaign = 1; else Hygiene_promo_campaign = 0; }
        }

        public bool b_other_activity
        {
            get { return other_activity == 1; }
            set { if (value) other_activity = 1; else other_activity = 0; }
        }

        public bool b_freq
        {
            get { return (ToiletsEvacuationFreq == 5) ? true : false; }
            set { if (value) { ;} }
        }
        public bool b_solidW
        {
            get { return (SolidWasteDisp == 4) ? true : false; }
            set { if (value) { ;} }
        }

        public bool b_WasteDisp
        {
            get { return (WasteDisposal == 4) ? true : false; }
            set { if (value) { ;} }
        }

        public int? tot_lat { get; set; }
        public int? tot_bath { get; set; }
        public int? tot_wp { get; set; }
    }

    [MetadataType(typeof(tblSectionC_MD))]
    public partial class tblSectionC
    {

        public bool b_shelter_kits_dist { get { return shelter_kits_dist == 1; } set { if (value) shelter_kits_dist = 1; else shelter_kits_dist = 0; } }

        public bool b_tent_dist { get { return tent_dist == 1; } set { if (value) tent_dist = 1; else tent_dist = 0; } }

        public bool b_nfi_dist { get { return nfi_dist == 1; } set { if (value) nfi_dist = 1; else nfi_dist = 0; } }

        public bool b_hyg_kits_dist { get { return hyg_kits_dist == 1; } set { if (value) hyg_kits_dist = 1; else hyg_kits_dist = 0; } }

        public bool b_shelter_repairs { get { return shelter_repairs == 1; } set { if (value) shelter_repairs = 1; else shelter_repairs = 0; } }

        public bool b_other_activity { get { return other_activity == 1; } set { if (value) other_activity = 1; else other_activity = 0; } }

        public bool b_nfi1
        {
            get { return (most_needed_nfi == 9) ? true : false; }
            set { if (value) { ;} }
        }

        public bool b_nfi2
        {
            get { return (sec_most_needed_nfi == 9) ? true : false; }
            set { if (value) { ;} }
        }
    }

    [MetadataType(typeof(tblSectionB_MD))]
    public partial class tblSectionB
    {

        public int? tot_m { get; set; }
        public int? tot_f { get; set; }

        public int? tot_lt1 { get; set; }
        public int? tot_1_5 { get; set; }
        public int? tot_6_12 { get; set; }
        public int? tot_13_17 { get; set; }
        public int? tot_18_59 { get; set; }
        public int? tot_60p { get; set; }
        public bool b_pop_chg
        {
            get { return (pop_chg == 1 || pop_chg == 2) ? true : false; }
            set { if (value) { ;} }
        }

        public bool ck_Arr_Gvt_trans { get { return Arr_Gvt_trans == 1; } set { if (value) Arr_Gvt_trans = 1; else Arr_Gvt_trans = 0; } }

        public bool ck_Arr_Mil_trans { get { return Arr_Mil_trans == 1; } set { if (value) Arr_Mil_trans = 1; else Arr_Mil_trans = 0; } }

        public bool ck_Arr_Ind_arr_camp { get { return Arr_Ind_arr_camp == 1; } set { if (value) Arr_Ind_arr_camp = 1; else Arr_Ind_arr_camp = 0; } }

        public bool ck_Arr_Ind_arr_HC { get { return Arr_Ind_arr_HC == 1; } set { if (value) Arr_Ind_arr_HC = 1; else Arr_Ind_arr_HC = 0; } }

        public bool ck_Arr_Ind_arr_origin { get { return Arr_Ind_arr_origin == 1; } set { if (value) Arr_Ind_arr_origin = 1; else Arr_Ind_arr_origin = 0; } }

        public bool ck_Arr_oth { get { return Arr_oth == 1; } set { if (value) Arr_oth = 1; else Arr_oth = 0; } }

        public bool ck_Dep_Gvt_trans { get { return Dep_Gvt_trans == 1; } set { if (value) Dep_Gvt_trans = 1; else Dep_Gvt_trans = 0; } }

        public bool ck_Dep_Mil_trans { get { return Dep_Mil_trans == 1; } set { if (value) Dep_Mil_trans = 1; else Dep_Mil_trans = 0; } }

        public bool ck_Dep_Ind_ret_origin { get { return Dep_Ind_ret_origin == 1; } set { if (value) Dep_Ind_ret_origin = 1; else Dep_Ind_ret_origin = 0; } }

        public bool ck_Dep_Ind_move_oth_camp { get { return Dep_Ind_move_oth_camp == 1; } set { if (value) Dep_Ind_move_oth_camp = 1; else Dep_Ind_move_oth_camp = 0; } }

        public bool ck_Dep_Ind_move_HC { get { return Dep_Ind_move_HC == 1; } set { if (value) Dep_Ind_move_HC = 1; else Dep_Ind_move_HC = 0; } }

        public bool ck_Dep_oth { get { return Dep_oth == 1; } set { if (value) Dep_oth = 1; else Dep_oth = 0; } }
    }


    [MetadataType(typeof(tbl_EDU_Services_MD))]
    public partial class tbl_EDU_Services
    {
        public bool notEmpty { get; set; }
    }


    [MetadataType(typeof(tbl_ESNFI_Services_MD))]
    public partial class tbl_ESNFI_Services
    {
        public bool notEmpty { get; set; }
    }

    [MetadataType(typeof(tbl_HEALTH_NUT_Services_MD))]
    public partial class tbl_HEALTH_NUT_Services
    {
        public bool notEmpty { get; set; }
    }

    [MetadataType(typeof(tbl_PROT_Services_MD))]
    public partial class tbl_PROT_Services
    {
        public bool notEmpty { get; set; }
    }

    [MetadataType(typeof(tbl_WASH_Gaps_MD))]
    public partial class tbl_WASH_Gaps
    {
        public bool notEmpty { get; set; }

        public int? tot_lat { get; set; }
        public int? tot_bath { get; set; }
        public int? tot_wp { get; set; }
    }

    [MetadataType(typeof(tbl_WASH_Services_MD))]
    public partial class tbl_WASH_Services
    {
        public bool notEmpty { get; set; }
    }

    [MetadataType(typeof(SYSUser_MD))]
    public partial class SYSUser
    {
    }

    [MetadataType(typeof(SYSUserProfile_MD))]
    public partial class SYSUserProfile
    {
        [Display(Name = "Change password?")]
        public bool ChangePassword { get; set; }
    }


    public partial class vw_surveys_latest_week
    {
        public string EvacuationFreq { get; set; }
        public string SolidWasteDisp { get; set; }
        public string DispMean { get; set; }

        public string notes_c { get; set; }
        public string notes_d { get; set; }
        public string notes_e { get; set; }
        public string notes_f { get; set; }
        public string notes_g { get; set; }

    }

    public partial class vw_surveys
    {
        //public string EvacuationFreq { get; set; }
        //public string SolidWasteDisp { get; set; }
        //public string DispMean { get; set; }

        //[System.ComponentModel.DefaultValue("")]
        //public string d_refer_gap { get; set; }
        //public string notes_c { get; set; }
        //public string notes_d { get; set; }
        //public string notes_e { get; set; }
        //public string notes_f { get; set; }
        //public string notes_g { get; set; }

    }
}
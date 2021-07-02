using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Stationary_Management.Common
{
    public enum EnumPfiCatagory
    {
        Indent,
        PFI
    }
    public enum EnumIncoterms
    {
        CFR,
        CPT,
        CPT_CFR,
        FOB,
        DP,
        TT,
        FCA,
        CIF,
        CIP,
        EXW
    }
    public enum EnumActiveDactiveStatus : byte
    {
        Inactive = 0,      // Inactive or deleted,
        Active = 1,      // Inactive or deleted,

    }
    public enum EnumGeneralConfigKey : byte
    {

        CountryMaster = 1,
        Department = 2,
        Designation = 3,
        Agent = 4,
    }
    public enum EnumMonth : byte
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
    //If you change EnumShipmentMode Enum , please Check ProformaInvoice.js file CalculateAirFreight() function 
    public enum EnumShipmentMode : byte
    {
        Sea = 1,
        Air = 2,
        Road = 3
    }
    public enum EnumApprovalStatus : byte
    {
        Pending = 1,
        Approve = 2,
        Auto_Approve = 3,
        Reject = 4
    }
    public enum EnumVisitingEventStatus : byte
    {
        Pending = 1,
        Attended = 2,
    }
    public enum EnumMapdataStatus : byte
    {
        OfficeAttendence = 1,
        EventAttendence = 2,
    }
}
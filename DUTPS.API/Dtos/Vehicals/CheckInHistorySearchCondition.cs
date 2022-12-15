using DUTPS.API.Extensions;
using DUTPS.Commons;
using static DUTPS.API.Extensions.ServicesExtensions;

namespace DUTPS.API.Dtos.Vehicals
{
    public class CheckInHistorySearchCondition : ParamsSearch
    {
        public DateTime? CheckInBeginDate { get; set; }
        
        public DateTime? CheckInEndDate { get; set; }

        public string CustomerUsername { get; set; }

        public string Query { set; get; }

        [SwaggerExclude]
        public override string OrderBy
        {
            get
            {
                switch (SortBy)
                {
                    case "dateOfCheckout":
                    case "staffCheckOutName":
                        return SortBy.FirstCharToUpper();
                    case "dateOfCheckIn":
                        return "CheckIn.DateOfCheckIn";
                    case "vehicalLicensePlate":
                        return "CheckIn.Vehical.LicensePlate";
                    case "vehicalDescription":
                        return "CheckIn.Vehical.Description";
                    case "customerName":
                        return "CheckIn.Customer.Information.Name";
                    case "staffCheckInName":
                        return "CheckIn.Staff.Information.Name";
                    default:
                        return "Id";
                }
            }
        }

        [SwaggerExclude]
        public override string Order
        { 
            get
            {
                return String.IsNullOrEmpty(Sort) ? "ASC" : Sort;
            }
        }
    }
}
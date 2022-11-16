using DUTPS.Commons;
using static DUTPS.API.Extensions.ServicesExtensions;

namespace DUTPS.API.Dtos.Vehicals
{
    public class AvailableCheckInSearchCondition : ParamsSearch
    {
        public string Query { set; get; }

        [SwaggerExclude]
        public override string OrderBy
        {
            get
            {
                switch (SortBy)
                {
                    case "dateOfCheckIn":
                        return "DateOfCheckIn";
                    case "vehicalLicensePlate":
                        return "Vehical.LicensePlate";
                    case "vehicalDescription":
                        return "Vehical.Description";
                    case "customerName":
                        return "Customer.Information.Name";
                    case "staffCheckInName":
                        return "Staff.Information.Name";
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
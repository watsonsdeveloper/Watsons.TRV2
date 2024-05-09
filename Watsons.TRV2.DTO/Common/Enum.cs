using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsons.TRV2.DTO.Common
{
    public enum ResponseCode : byte
    {
        Error = 0,
        Success = 1,
        NotFound = 2,
        Unauthorized = 3,
        Forbidden = 4,
        BadRequest = 5,
        Conflict = 6,
        UnprocessableEntity = 7,
        InternalServerError = 8,
        NotImplemented = 9,
        ServiceUnavailable = 10,
        GatewayTimeout = 11,
        Unknown = 12,
    }

    public enum TrStatus : byte
    {
        All = 0,
        Pending = 1,
        Approved = 2,
        Rejected = 3,
    }
    public enum TrOrderBatchStatus : byte
    {
        All = 0,
        Pending = 1,
        Processed = 2,
        Overdue = 3
    }
    public enum TrOrderStatus : byte
    {
        All = 0,
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Processed = 4,
        Fulfilled = 5,
        Unfulfilled = 6,
        Cancelled = 7
    }

    public enum TrReason : byte
    {
        NewListing = 1,
        Damaged = 2,
        Depleted = 3,
        Missing = 4,
        Expired = 5
    }
    public enum Brand : byte
    {
        Own = 1,
        Supplier = 2,
    }

    //public static class SalesBand 
    //{
    //    public static readonly int PLU_UNIT_LIMIT_SUPPLIER = 1;
    //    public static readonly int PLU_UNIT_LIMIT_OWN = 2;
    //    public static readonly int COST_LIMIT_OWN = 3;
    //}

    //public enum SalesBand : byte
    //{
    //    PLU_UNIT_LIMIT_SUPPLIER = 1,
    //    PLU_UNIT_LIMIT_OWN = 2,
    //    COST_LIMIT_OWN = 3
    //}
}

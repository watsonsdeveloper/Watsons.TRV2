using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.TR.Entities;

public partial class TrafficLog
{
    public Guid RequestId { get; set; }

    public string? AccessToken { get; set; }

    public string? AbsoluteUrlWithQuery { get; set; }

    public string? Action { get; set; }

    public string? Headers { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public int? HttpStatus { get; set; }

    public DateTime? RequestDt { get; set; }

    public DateTime? ResponseDt { get; set; }

    public float? TimeTaken { get; set; }
}

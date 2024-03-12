using System;
using System.Collections.Generic;

namespace Watsons.TRV2.DA.SysCred.Entities;

public partial class Otptracking
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ApplicationId { get; set; }

    public string? Otp { get; set; }

    public DateTime? OtpTimeout { get; set; }

    public string? SessionIdentifier { get; set; }

    public int? Status { get; set; }
}

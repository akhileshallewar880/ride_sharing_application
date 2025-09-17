using System;
using System.ComponentModel.DataAnnotations;

namespace RideSharing.API.Models.DTO
{
    public record SendOtpRequest(
        [Required, Phone] string Phone
    );

    public record SendOtpResponse(
        bool OtpSent
    );

    public record VerifyOtpRequest(
        [Required, Phone] string Phone,
        [Required, MinLength(4), MaxLength(8)] string Otp
    );

    public record VerifyOtpResponse(
        [Required] string AccessToken,
        [Required] Guid UserId,
        [Required] string Role
    );
}

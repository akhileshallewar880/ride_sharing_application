using System;
using System.ComponentModel.DataAnnotations;

namespace RideSharing.API.Models.DTO
{
    public record UserProfileDto(
        [Required] Guid Id,
        [Required, Phone] string Phone,
        [MinLength(2)] string? Name,
        [EmailAddress] string? Email,
        [Required] string Role,
        bool IsActive
    );

    public record UpdateProfileRequest(
        [MinLength(2)] string? Name,
        [EmailAddress] string? Email
    );
}

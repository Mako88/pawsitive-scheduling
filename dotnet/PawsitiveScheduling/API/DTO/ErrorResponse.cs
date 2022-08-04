using System.Collections.Generic;

namespace PawsitiveScheduling.API.DTO
{
    public class ErrorResponse
    {
        public string? Error { get; set; }

        public string? Stack { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}

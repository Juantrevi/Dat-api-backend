using Dat_api.Entities;

namespace Dat_api.DTOs
{
    public class PhotoDto
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public bool IsMain { get; set; } 

    }
}
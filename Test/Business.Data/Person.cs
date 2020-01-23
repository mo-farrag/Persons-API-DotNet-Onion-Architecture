using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Data
{
    [Table("Persons")]
    public class Person
    {
        [JsonProperty]
        public int Id { get; set; }

        [Required]
        [JsonProperty]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty]
        public string LastName { get; set; }

        [Required]
        [JsonProperty]
        public string CountryCode { get; set; }

        [Required]
        [JsonProperty]
        public string Phone { get; set; }

        [Required]
        [JsonProperty]
        public string Gender { get; set; }

        [Required]
        [JsonProperty]
        public DateTime BirthDate { get; set; }

        [Required]
        [JsonIgnore]
        public byte[] Avatar { get; set; }

        [JsonIgnore]
        public string Email { get; set; }

        //[JsonIgnore]
        //public Guid Guid { get; set; }
    }
}

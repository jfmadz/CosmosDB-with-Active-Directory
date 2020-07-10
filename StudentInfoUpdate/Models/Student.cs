using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace StudentInfoUpdate.Models

{
    public class Student
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [DisplayName("Student Number")]
        [Required(ErrorMessage ="8 digits required")]
        [MaxLength(8)]
        [JsonProperty(PropertyName = "Student Number")]
        public string Student_No { get; set; }

        [DisplayName("Name")]
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [DisplayName("Surname")]
        [JsonProperty(PropertyName = "Surname")]
        public string Surname { get; set; }

        [DisplayName("Email Address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        [JsonProperty(PropertyName = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Home Address")]
        [JsonProperty(PropertyName = "Address")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Mobile Number")]
        [JsonProperty(PropertyName = "Mobile Number")]
        public string Mobile_No { get; set; }

        [DisplayName("Not Active")]
        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }


        public string ImageUri { get; set; }
        public string ThumbnailUri { get; set; }
        public string Caption { get; set; }


        [JsonProperty(PropertyName = "Blob Uri")]
        public string URI { get; set; }


    }
}
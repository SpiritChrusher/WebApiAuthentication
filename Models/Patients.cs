using System.ComponentModel.DataAnnotations;

namespace WebApiAuthentication.Models;
public record PatientRequest(
    [Required]
    string Name,
    string Address,
    [MaxLength(12)]
    string SSN,
    [Required]
    string Description);

public class PatientDto
{
    [Key]
    public long Id { get; set; }
    [Required(ErrorMessage = $"{nameof(Name)} is required!")]
    public string Name { get; set; }
    public string Address { get; set; }
    [MaxLength(12)]
    public string SSN { get; set; }
    [Required(ErrorMessage = $"{nameof(Description)} is required!")]
    public string Description { get; set; }

    public PatientDto() { }

    public PatientDto(long id, string name, string address, string ssn, string description)
    {
        Id = id;
        Name = name;
        Address = address;
        SSN = ssn;
        Description = description;
    }
}

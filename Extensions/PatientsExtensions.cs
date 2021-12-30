using WebApiAuthentication.Models;

namespace WebApiAuthentication.Extensions;
public static class PatientExtensions
{
    public static PatientDto ToPatientDto(this PatientRequest patientRequest, long id)
    {
        return new PatientDto(
            id: id,
            name: patientRequest.Name,
            address: patientRequest.Address,
            ssn: patientRequest.SSN,
            description: patientRequest.Description
            );
    }
}
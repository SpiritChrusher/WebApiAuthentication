using WebApiAuthentication.Models;

namespace WebApiAuthentication.DataAccess;
public interface IPatientsRepository
{
    Task StorePatientAsync(PatientRequest patient);
    List<PatientDto> ReadAllPatientsAsync();
    Task<PatientDto> ReadPatientAsync(long id);
    Task UpdatePatientAsync(PatientDto updatedPatient);
    Task DeletePatientAsync(long id);
    Task DeleteAllPatientsAsync();
}
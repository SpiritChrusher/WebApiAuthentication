using Microsoft.EntityFrameworkCore;
using WebApiAuthentication.Exceptions;
using WebApiAuthentication.Extensions;
using WebApiAuthentication.Models;

namespace WebApiAuthentication.DataAccess;

public class PatientsRepository : IPatientsRepository
{
    private PatientsContext _context { get; }

    public PatientsRepository(PatientsContext context)
    {
        _context = context;
    }


    public List<PatientDto> ReadAllPatientsAsync()
    {
        try
        {
            var patients = _context.Patients.ToList();
            return patients;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<PatientDto> ReadPatientAsync(long id)
    {
        try
        {
            var patient = await _context.Patients.Where(x => x.Id == id).FirstOrDefaultAsync();
            return patient;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task StorePatientAsync(PatientRequest patientRequest)
    {
        try
        {
            var maxId = _context.Patients.Max(x => x.Id);

            await _context.AddAsync(patientRequest.ToPatientDto(maxId+1));
            await _context.SaveChangesAsync();
        }
        catch (InvalidOperationException)
        {
            await _context.AddAsync(patientRequest.ToPatientDto(1));
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdatePatientAsync(PatientDto updatedPatient)
    {
        try
        {
            _context.Update(updatedPatient);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public async Task DeleteAllPatientsAsync()
    {
        try
        {
            var patients = _context.Patients.ToList();
            _context.RemoveRange(patients);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task DeletePatientAsync(long id)
    {
        try
        {
            var patient = _context.Patients.Where(x => x.Id == id).First();
            _context.Remove(patient);
            await _context.SaveChangesAsync();
        }
        catch (InvalidOperationException)
        {
            new BadRequestException();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
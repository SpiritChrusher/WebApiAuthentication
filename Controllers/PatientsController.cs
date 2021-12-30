using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiAuthentication.Authentication;
using WebApiAuthentication.DataAccess;
using WebApiAuthentication.Extensions;
using WebApiAuthentication.Models;

namespace WebApiAuthentication.Controllers;

[Route("/patients")]
public class PatientsController : Controller
{
    private IPatientsRepository _patientsRepository { get; }

    public PatientsController(IPatientsRepository patientsRepository)
    {
        _patientsRepository = patientsRepository;
    }

    [HttpGet]
    [Route("")]
    [Authorize(Roles = UserRoles.Admin)]
    public ActionResult<List<PatientDto>> GetPatients()
    {
        if (!User.Identity.IsAuthenticated)
            return BadRequest();

        var identity = User.Identity as ClaimsIdentity;

        if (identity is null)
            return BadRequest();

        try
        {
            var patients = _patientsRepository.ReadAllPatientsAsync();
            return Ok(patients);
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<PatientRequest>> GetPatient([FromRoute] long id)
    {
        if (Validator.ValidateLong(id))
            return BadRequest("bad patient data!!!");

        try
        {
            var result = await _patientsRepository.ReadPatientAsync(id);
            return Ok(result);
        }
        catch (Exception)
        {

            return BadRequest();
        }
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult> CreatePatient([FromBody] PatientRequest patientRequest)
    {
        if (Validator.IsPatientRequestBad(patientRequest))
            return BadRequest("bad patient data!!!");

        try
        {
            await _patientsRepository.StorePatientAsync(patientRequest);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut]
    [Route("")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult> UpdatePatient([FromBody] PatientDto updatedPatient)
    {
        if (Validator.IsPatientDtoBad(updatedPatient))
            return BadRequest("bad patient data!!!");

        try
        {
            await _patientsRepository.UpdatePatientAsync(updatedPatient);
            return Ok();
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpDelete]
    [Route("")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult> DeletePatient()
    {
        try
        {
            await _patientsRepository.DeleteAllPatientsAsync();
            return NoContent();
        }
        catch (Exception)
        {

            return Conflict();
        }

    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult> DeletePatient([FromRoute] long id)
    {
        if (Validator.ValidateLong(id))
            return BadRequest();

        try
        {
            await _patientsRepository.DeletePatientAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception)
        {

            return Conflict();
        }

    }
}

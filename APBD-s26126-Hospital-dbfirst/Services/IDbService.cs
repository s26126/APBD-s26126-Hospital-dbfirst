using APBD_s26126_Hospital_dbfirst.DTOs;

namespace APBD_s26126_Hospital_dbfirst.Services;

public interface IDbService
{
    Task<IEnumerable<PatientDto>> GetPatientsAsync(string? search);
    Task AddBedAssignmentAsync(string pesel, CreateBedAssignmentDto dto);
}

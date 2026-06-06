using APBD_s26126_Hospital_dbfirst.Data;
using APBD_s26126_Hospital_dbfirst.DTOs;
using Microsoft.EntityFrameworkCore;

namespace APBD_s26126_Hospital_dbfirst.Services;

public class DbService : IDbService
{
    private readonly S26126Context _context;

    public DbService(S26126Context context)
    {
        _context = context;
    }


    public async Task<IEnumerable<PatientDto>> GetPatientsAsync()
    {
        var res = await _context.Patients
            .Select(p => new PatientDto()
            {
                Pesel = p.Pesel,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                Sex = (p.Sex ? "Male" : "Female"),
                Admissions = p.Admissions.Select(a => new AdmissionDto
                {
                    Id = a.Id,
                    AdmissionDate = a.AdmissionDate,
                    DischargeDate = a.DischargeDate,
                    Ward = new WardDto
                    {
                        Id = a.Ward.Id,
                        Name = a.Ward.Name,
                        Description = a.Ward.Description
                    }
                }).ToList(),
                BedAssignments = p.BedAssignments.Select(b => new BedAssignmentDto
                {
                    Id = b.Id,
                    From = b.From,
                    To = b.To,
                    Bed = new BedDto()
                    {
                        Id = b.Bed.Id,
                        BedType = new BedTypeDto()
                        {
                            Id = b.Bed.BedType.Id,
                            Name = b.Bed.BedType.Name,
                            Description = b.Bed.BedType.Description
                        },
                        Room = new RoomDto()
                        {
                            Id = b.Bed.Room.Id,
                            HasTv = b.Bed.Room.HasTv,
                            Ward = new WardDto()
                            {
                                Id = b.Bed.Room.Ward.Id,
                                Name = b.Bed.Room.Ward.Name,
                                Description = b.Bed.Room.Ward.Description
                            }
                        }
                    }
                }).ToList()
            })
            .ToListAsync();

        return res;
    }
}
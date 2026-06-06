using APBD_s26126_Hospital_dbfirst.Data;
using APBD_s26126_Hospital_dbfirst.DTOs;
using APBD_s26126_Hospital_dbfirst.Exceptions;
using APBD_s26126_Hospital_dbfirst.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_s26126_Hospital_dbfirst.Services;

public class DbService : IDbService
{
    private readonly S26126Context _context;

    public DbService(S26126Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientDto>> GetPatientsAsync(string? search)
    {
        var query = _context.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName, $"%{search}%") ||
                EF.Functions.Like(p.LastName, $"%{search}%"));
        }

        var res = await query
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

    public async Task AddBedAssignmentAsync(string pesel, CreateBedAssignmentDto dto)
    {
        var patientExists = await _context.Patients.AnyAsync(p => p.Pesel == pesel);
        if (!patientExists)
            throw new NotFoundException($"Nie znaleziono pacjenta o numerze PESEL {pesel}");

        var ward = await _context.Wards.FirstOrDefaultAsync(w => w.Name == dto.Ward);
        if (ward == null)
            throw new NotFoundException($"Nie znaleziono oddziału '{dto.Ward}'");

        var bedType = await _context.BedTypes.FirstOrDefaultAsync(bt => bt.Name == dto.BedType);
        if (bedType == null)
            throw new NotFoundException($"Nie znaleziono typu łóżka '{dto.BedType}'");

        var bed = await _context.Beds.FirstOrDefaultAsync(b =>
            b.BedTypeId == bedType.Id &&
            b.Room.WardId == ward.Id &&
            !b.BedAssignments.Any(ba =>
                (ba.To == null || ba.To > dto.From) &&
                (dto.To == null || ba.From < dto.To)));

        if (bed == null)
            throw new NotFoundException(
                $"Brak wolnego łóżka typu '{dto.BedType}' na oddziale '{dto.Ward}' w podanym okresie");

        var bedAssignment = new BedAssignment
        {
            PatientPesel = pesel,
            BedId = bed.Id,
            From = dto.From,
            To = dto.To
        };

        await _context.BedAssignments.AddAsync(bedAssignment);
        await _context.SaveChangesAsync();
    }
}

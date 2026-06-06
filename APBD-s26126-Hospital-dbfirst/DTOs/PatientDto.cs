namespace APBD_s26126_Hospital_dbfirst.DTOs;

public class PatientDto
{
    public string Pesel { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Sex { get; set; }
    public List<AdmissionDto> Admissions { get; set; }
    public List<BedAssignmentDto> BedAssignments { get; set; }
}
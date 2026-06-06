namespace APBD_s26126_Hospital_dbfirst.DTOs;

public class CreateBedAssignmentDto
{
    public DateTime From { get; set; }
    public DateTime? To { get; set; }
    public string BedType { get; set; }
    public string Ward { get; set; }
}

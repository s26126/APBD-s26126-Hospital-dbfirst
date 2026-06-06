namespace APBD_s26126_Hospital_dbfirst.DTOs;

public class BedDto
{
    public int Id { get; set; }
    public BedTypeDto BedType { get; set; }
    public RoomDto Room { get; set; }
    
}
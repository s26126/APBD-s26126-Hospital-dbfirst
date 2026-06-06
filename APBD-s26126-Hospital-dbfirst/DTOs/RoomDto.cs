namespace APBD_s26126_Hospital_dbfirst.DTOs;

public class RoomDto
{
    public string Id { get; set; }
    public bool HasTv { get; set; }
    public WardDto Ward { get; set; }
}
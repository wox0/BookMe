namespace BookMe.Models;

public class Hotel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<RoomTypes> RoomTypes { get; set; }
    public List<Room> Rooms { get; set; }
}
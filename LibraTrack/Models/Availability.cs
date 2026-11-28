namespace LibraTrack.Models;

public enum AvailabilityLevel
{
    Green,   // available
    Orange,  // reserved
    Red      // borrowed, or no copies
}

public record BookAvailability(AvailabilityLevel Level, int AvailableCount, int TotalCopies)
{
    // Lets the view tell the two red cases apart ("no copies" vs "all borrowed")
    public bool HasNoCopies => TotalCopies == 0;
}

public static class AvailabilityExtensions
{
    public static AvailabilityLevel GetAvailability(this BookCopy copy) =>
        copy.Status switch
        {
            CopyStatus.Available => AvailabilityLevel.Green,
            CopyStatus.Reserved => AvailabilityLevel.Orange,
            CopyStatus.Borrowed => AvailabilityLevel.Red,
            _ => AvailabilityLevel.Red   // unknown status: never promise availability
        };

    public static BookAvailability GetAvailability(this Book book)
    {
        var copies = book.Copies.Where(c => !c.IsDeleted).ToList();

        int available = copies.Count(c => c.Status == CopyStatus.Available);
        int reserved = copies.Count(c => c.Status == CopyStatus.Reserved);

        var level =
            available > 0 ? AvailabilityLevel.Green :
            reserved > 0 ? AvailabilityLevel.Orange :
                            AvailabilityLevel.Red;

        return new BookAvailability(level, available, copies.Count);
    }

    public static string BadgeClass(this AvailabilityLevel level) => level switch
    {
        AvailabilityLevel.Green => "text-bg-success",
        AvailabilityLevel.Orange => "text-bg-warning",
        _ => "text-bg-danger"
    };

    public static string Message(this BookAvailability a) => a.Level switch
    {
        AvailabilityLevel.Green => $"{a.AvailableCount} of {a.TotalCopies} available",
        AvailabilityLevel.Orange => "Reserved",
        _ when a.HasNoCopies => "No copies",
        _ => "All copies borrowed"
    };
}

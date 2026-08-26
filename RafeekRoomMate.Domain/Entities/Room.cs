using RafeekRoomMate.Domain.Entities.Enums;

namespace RafeekRoomMate.Domain.Entities
{
    public sealed class Room
    {
        private Room(){}

        public Room(Guid createdByUserId,string title,string description,string city, RoomType roomType,
            GenderPreference genderPreference, string area, decimal monthlyRent, DateTime? availableFrom, int maximumOccupants)
        {
            Id = Guid.NewGuid();
            CreatedByUserId = createdByUserId;
            Title = title;
            Description = description;
            City = city;
            Area = area;
            RoomType = roomType;
            GenderPreference = genderPreference;
            MonthlyRent = monthlyRent;
            AvailableFrom = availableFrom;
            MaximumOccupants = maximumOccupants;
            Status = RoomStatus.Available;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string title,string description,string city,string area,decimal monthlyRent,DateTime? availableFrom,int maximumOccupants,RoomType roomType,GenderPreference genderPreference)
        {
            Title = title;

            Description = description;

            City = city;

            Area = area;

            MonthlyRent = monthlyRent;

            AvailableFrom = availableFrom;

            MaximumOccupants = maximumOccupants;

            RoomType = roomType;

            GenderPreference = genderPreference;

            UpdatedAt = DateTime.UtcNow;
        }
        public void Close()
        {
            Status = RoomStatus.Closed;
        }

        public void Reopen()
        {
            Status = RoomStatus.Available;
        }   

        public void MarkAsFull()
        {
            Status = RoomStatus.Full;
        }

        public void MarkAsAvailable()
        {
            Status = RoomStatus.Available;
        }

        public Guid Id { get; private set; }

        // Creator
        public Guid CreatedByUserId { get; private set; }

        // Basic information
        public string Title { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        // Location
        public string City { get; private set; } = string.Empty;

        public string Area { get; private set; } = string.Empty;

        // Financial
        public decimal MonthlyRent { get; private set; }

        // Availability
        public DateTime? AvailableFrom { get; private set; }

        public int MaximumOccupants { get; private set; }
        public RoomType RoomType { get; private set; }
        public GenderPreference GenderPreference { get; private set; }
        // Status
        public RoomStatus Status { get; private set; }

        // Audit
        public DateTime CreatedAt { get; private set; }

        public DateTime? UpdatedAt { get; private set; }

        // Navigation properties
        public ICollection<RoomImage> Images { get; private set; } = new List<RoomImage>();

        public ICollection<RoomMember> Members { get; private set; } = new List<RoomMember>();
        public ICollection<RoomFavorite> Favorites { get; private set; } = new List<RoomFavorite>();
        public ICollection<RoomAppointment> Appointments { get; private set; } = new List<RoomAppointment>();
    }
}

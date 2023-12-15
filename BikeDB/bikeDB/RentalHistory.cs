using System;
using System.Collections.Generic;

namespace BikeDB.bikeDB;

public partial class RentalHistory
{
    public int Id { get; set; }

    public int BikeId { get; set; }

    public DateTime RentalTime { get; set; }

    public DateTime? ReturnTime { get; set; }

    public virtual Bike Bike { get; set; } = null!;
}

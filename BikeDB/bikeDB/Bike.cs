using System;
using System.Collections.Generic;

namespace BikeDB.bikeDB;

public partial class Bike
{
    public int Id { get; set; }

    public bool Used { get; set; }

    public virtual ICollection<RentalHistory> RentalHistories { get; set; } = new List<RentalHistory>();
}

using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Reader
{
    public int ReaderId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<BorrowingRecord> BorrowingRecords { get; set; } = new List<BorrowingRecord>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

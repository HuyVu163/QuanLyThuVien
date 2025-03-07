using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class BorrowingRecord
{
    public int RecordId { get; set; }

    public int ReaderId { get; set; }

    public int BookId { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;

    public virtual Reader Reader { get; set; } = null!;
}

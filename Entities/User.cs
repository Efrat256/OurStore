﻿using System;
using System.Collections.Generic;

namespace Entities;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

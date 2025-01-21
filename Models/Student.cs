using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 class Student
{
    public int Id { get; set; }
    public string Firstname { get; set; } = default;
    public string Lastname { get; set; } = default;
    public string? Class { get; set; }
    public int? Age { get; set; }
}

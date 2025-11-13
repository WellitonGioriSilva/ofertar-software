namespace api_ofertar.Enums
{
    public enum PaymentMethod
    {
        Single = 'S',
        Married = 'M',
        Divorced = 'D',
        Widowed = 'W'
    }
}

/*modelBuilder.Entity<Tither>()
    .Property(t => t.MaritalStatus)
    .HasConversion(
        v => (char)v,         // Enum → char no banco
        v => (MaritalStatus)v // char → Enum no C#
    )
    .HasColumnType("char(1)");
    
[Column("maritalStatus")]
public MaritalStatus MaritalStatus { get; set; }

*/


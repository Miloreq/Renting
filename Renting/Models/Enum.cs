using Microsoft.AspNetCore.Mvc;

namespace Renting.Models
{
    public enum Statusenum
    {
        Pending,
        Approved,
        Rejected,
        CheckedOut,
        Returned,
        Cancelled
    }
    public enum Stanenum
    {
        Nowy,
        Dobry,
        Urzywany,
        Uszkodzony
    }

    public enum TFenum
    { 
        True,
        False
    }
}

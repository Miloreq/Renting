using Renting.Models;
using System.Collections.Generic;

namespace Renting.ViewModels
{
    public class AssetDetailsViewModel
    {
        public Asset Asset { get; set; }
        public List<Rental> Rentals { get; set; }
    }
}
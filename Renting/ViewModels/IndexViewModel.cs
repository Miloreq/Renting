using System.Collections.Generic;
using Renting.Models;

namespace Renting.ViewModels
{
    public class IndexViewModel
    {
        public List<Asset> Assets { get; set; } = new();
        public List<Rental> Rental { get; set; } = new();
    }
}
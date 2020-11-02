using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using VroomDb.Entities;

namespace vroom.Views.ViewModels
{
    public class BikeViewModel
    {
        public Bike Bike { get; set; }

        public IEnumerable<Make> Makes { get; set; }
        public IEnumerable<Model> Models { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
       
        private List<Currency> CList = new List<Currency>();

        private List<Currency> CreateList()
        {
            CList.Add(new Currency("VND", "VND"));
            CList.Add(new Currency("USD", "USD"));
            CList.Add(new Currency("EUR", "EUR"));
            return CList;
        }
        public BikeViewModel()
        {
            Currencies = CreateList();
        }
    }

    public class Currency
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Currency(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

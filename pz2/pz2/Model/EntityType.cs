using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz2.Model
{
    public class EntityType:BindableBase
    {
        private string ime;
        private string slika;

        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                OnPropertyChanged("Ime");
            }
        }
        public string Slika
        {
            get { return slika; }
            set
            {
                slika = value;
                OnPropertyChanged("Slika");
            }
        }
    }
}

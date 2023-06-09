using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz2.Model
{
    public enum Tip { Vetrogenerator, Solarni_panel }
    public class Entity : ValidationBase
    {
        private string textId;
        private int id;
        private string naziv;
        private Tip tip;
        private double vrednost;
        private string slika;
        private string ime;


        public Entity()
        {
        }
        public Entity(int id)
        {
            this.Id = id;
        }
        public List<Tip> Tipovi => Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();

        public Entity(int id, string naziv, Tip tip, double vrednost)
        {
            this.Id = id;
            this.Naziv = naziv;
            this.Tip = tip;
            this.Vrednost = vrednost;
            if (this.Tip == Tip.Solarni_panel)
            {
                Ime = "Solarni Panel";
                this.slika = "SolarniPanel.jpg";
            }
            if (this.Tip == Tip.Vetrogenerator)
            {
                Ime = "Vetrogenerator";
                this.slika = "Vetrogenerator.jpg";
            }
        }

        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        public string Naziv
        {
            get { return naziv; }
            set
            {
                if (naziv != value)
                {
                    naziv = value;
                    OnPropertyChanged("Naziv");
                }
            }
        }
        public Tip Tip
        {
            get { return tip; }
            set
            {
                if (tip != value)
                {
                    tip = value;
                    OnPropertyChanged("Tip");
                }
            }
        }
        public double Vrednost
        {
            get { return vrednost; }
            set
            {
                if (vrednost != value)
                {
                    vrednost = value;
                    OnPropertyChanged("Vrednost");
                }
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

        public string Ime
        {
            get { return ime; }
            set
            {
                ime = value;
                OnPropertyChanged("Ime");
            }
        }

        public bool isValueValid()
        {
            bool isValueValid = true;
            if (Vrednost < 1 && Vrednost > 5)
            {

                isValueValid = false;
            }
            return isValueValid;
        }
        protected override void ValidateSelf()
        {
            int tempId;
            bool parsingSuccess = int.TryParse(this.textId, out tempId);

            if (this.DoesIdAlreadyExist)
            {
                this.ValidationErrors["Id"] = "Id already exists.";
            }

            if (!parsingSuccess)
            {
                this.ValidationErrors["Id"] = "Id must be an integer.";
            }
            else if (tempId < 0)
            {
                this.ValidationErrors["Id"] = "Id can't be negative.";
            }

            if (string.IsNullOrWhiteSpace(this.textId))
            {
                this.ValidationErrors["Id"] = "Id is required.";
            }

            if (string.IsNullOrWhiteSpace(this.naziv))
            {
                this.ValidationErrors["Name"] = "Name is required.";
            }
        }
    }
}

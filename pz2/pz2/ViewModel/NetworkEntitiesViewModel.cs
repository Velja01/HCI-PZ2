using pz2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace pz2.ViewModel
{
    public class NetworkEntitiesViewModel : BindableBase
    {
        public int id = 0;
        List<int> listaID = new List<int>();


        public ObservableCollection<Entity> Entities { get; set; }
        public static ObservableCollection<Entity> EntitiesShow { get; set; }
        public ObservableCollection<Entity> SearchedEntities { get; set; }
        public ObservableCollection<Entity> AllEntities { get; set; }

        
        public List<Tip> ComboBoxItems { get; set; } = Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();

        private Tip selectedEntityType;
        private Tip selectedEntityTypeSearch;

        public MyICommand AddEntity { get; set; }
        public MyICommand DeleteEntity { get; private set; }

        

        public MyICommand SearchEntities { get; set; }



        private Entity selectedEntity;
        private Entity pomocna;
        

        private string listaPrazna;
        private string upozorenje;
        private bool isNaziv;
        private bool isTip;
        private string nazivEntity;

        public NetworkEntitiesViewModel()
        {
            Entities = new ObservableCollection<Entity>();
            EntitiesShow = Entities;
            AllEntities = new ObservableCollection<Entity>();
            AddEntity = new MyICommand(OnAdd);
            DeleteEntity = new MyICommand(OnDelete, CanDelete);
            SearchedEntities = new ObservableCollection<Entity>();
            SearchEntities = new MyICommand(OnSearch);

            

        }
        
        private void OnSearch()
        {
            Tip tip = SelectedEntityTypeSearch;
            string naziv = NazivEntity;

            // Očistite prethodno pretražene entitete
            SearchedEntities.Clear();
            if (IsNaziv == true)
            {
                
                Upozorenje = "";
                foreach (var entity in AllEntities)
                {
                    if (entity.Naziv.Contains(NazivEntity))
                    {
                        SearchedEntities.Add(entity);
                        
                    }
                    if (entity.Ime.Contains(NazivEntity))
                    {
                        SearchedEntities.Add(entity);
                        
                    }
                }
            }
            if (IsTip == true)
            {
                Upozorenje = "";
                foreach (var entity in AllEntities)
                {
                    if (entity.Tip == tip)
                    {
                        SearchedEntities.Add(entity);
                        
                    }
                }
            }
            if (IsTip == false && IsNaziv == false)
            {
                Upozorenje = "Morate odabrati tip pretrage";
                
                
            }
            if (AllEntities.Count == 0)
            {
                ListaPrazna = "Lista je prazna";
            }
            if (SearchedEntities.Count != 0)
            {
                ListaPrazna = "";
            }
            EntitiesShow.Clear();
            foreach (var entity in SearchedEntities)
            {
                EntitiesShow.Add(entity);
            }
            OnPropertyChanged("EntitiesToShow");
        }
        public void OnDelete()
        {
            pomocna = selectedEntity;
            
            listaID.Add(selectedEntity.Id);
            Entities.Remove(selectedEntity);
            AllEntities.Remove(pomocna);
            NetworkDisplayViewModel.EntityList.Remove(pomocna);
            OnPropertyChanged("EntityList");


        }
        public Tip SelectedEntityType
        {
            get { return selectedEntityType; }
            set
            {
                selectedEntityType = value;
                OnPropertyChanged("SelectedEntityType");
            }
        }
        public void OnAdd()
        {
            Tip tip = SelectedEntityType;
            if (listaID.Count == 0)
            {
                id++;
                string naziv = "naziv" + id.ToString();
                Random random = new Random();
                double randomValue = random.NextDouble() * (5 - 1) + 1;
                double roundedValue = Math.Round(randomValue, 2);
                double vrednost = roundedValue;
                Entities.Add(new Entity(id, naziv, tip, vrednost));
                AllEntities.Add(new Entity(id, naziv, tip, vrednost));
                NetworkDisplayViewModel.EntityList.Add(new Entity(id, naziv, tip, vrednost));
            }
            else
            {
                int id1;
                id1 = listaID[0];
                listaID.RemoveAt(0);

                string naziv = "naziv" + id.ToString();
                Random random = new Random();
                double randomValue = random.NextDouble() * (5 - 1) + 1;
                double roundedValue = Math.Round(randomValue, 2);
                double vrednost = roundedValue;
                Entities.Add(new Entity(id1, naziv, tip, vrednost));
                AllEntities.Add(new Entity(id1, naziv, tip, vrednost));
                NetworkDisplayViewModel.EntityList.Add(new Entity(id1, naziv, tip, vrednost));
            }
            EntitiesShow.Clear();
            foreach (var entity in AllEntities)
            {
                EntitiesShow.Add(entity);
            }
            OnPropertyChanged("EntitiesToAdd");

        }
        public Entity SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                selectedEntity = value;
                DeleteEntity.RaiseCanExecuteChanged();
            }
        }

        public bool IsNaziv
        {
            get { return isNaziv; }
            set
            {
                isNaziv = value;
                OnPropertyChanged("IsNaziv");
            }
        }
        public bool IsTip
        {
            get { return isTip; }
            set
            {
                isTip = value;
                OnPropertyChanged("IsTip");
            }
        }

        public string NazivEntity
        {
            get { return nazivEntity; }

            set
            {
                nazivEntity = value;
                OnPropertyChanged("NazivEntity");
            }
        }

        public Tip SelectedEntityTypeSearch
        {
            get { return selectedEntityTypeSearch; }
            set
            {
                selectedEntityTypeSearch = value;
                OnPropertyChanged("SelectedEntityTypeSearch");
            }
        }

        public string Upozorenje
        {
            get { return upozorenje; }
            set
            {
                upozorenje = value;
                OnPropertyChanged("Upozorenje");
            }
        }

        public string ListaPrazna
        {
            get
            {
                return listaPrazna;
            }
            set
            {
                listaPrazna = value;
                OnPropertyChanged("ListaPrazna");
            }
        }
        private bool CanDelete()
        {
            return SelectedEntity != null;
        }

    }
}

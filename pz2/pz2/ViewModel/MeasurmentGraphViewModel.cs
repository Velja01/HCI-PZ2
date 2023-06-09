using NetworkService.Model;
using pz2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace pz2.ViewModel
{
    public class MeasurmentGraphViewModel:BindableBase
    {
        public ObservableCollection<CircleMarker> CircleMarkers { get; set; }

        public List<Tip> ComboBoxItems { get; set; } = Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();

        public MyICommand ShowEntitiesCommand { get; set; }

        private string selectedType;
        private string errorMessage;

        public MeasurmentGraphViewModel()
        {
            ShowEntitiesCommand = new MyICommand(OnShow);

            CircleMarkers = new ObservableCollection<CircleMarker>();
            for (int i = 0; i <= 4; i++)
            {
                CircleMarker marker = new CircleMarker();
                CircleMarkers.Add(marker);
            }

            //SelectedType = ComboBoxItems[0].ToString();
            SetValuesToCircleMarkers(LoadLastFiveUpdates(SelectedType));
        }

        private List<CircleMarker> LoadLastFiveUpdates(string selectedType)
        {
            if (!File.Exists("LogFile.txt"))
            {
                ErrorMessage = "Log file doesn't exist.";
                return null;
            }

            string[] lines = File.ReadAllLines("LogFile.txt");

            List<CircleMarker> lastFiveUpdates = new List<CircleMarker>();

            for (int i = lines.Count() - 1; i >= 0; i--)
            {
                string line = lines[i];

                string[] parts = line.Split(new[] { "\t", ":" }, StringSplitOptions.RemoveEmptyEntries);

                // Dobijanje datuma sa vremenom
                string dateTimeStr = parts[1].Trim();
                int index = int.Parse(parts[3].Substring(parts[3].Length-1));
                // Dobijanje vrednosti
                int value = int.Parse(parts[3]);

                /*string date = line.Substring(0, line.IndexOf(" "));
                line = line.Substring(line.IndexOf(" ") + 1);
                string time = line.Substring(0, line.IndexOf(" ") - 1);
                line = line.Substring(line.IndexOf(" ") + 1);
                string type = line.Substring(0, line.IndexOf(','));
                string val = line.Substring(line.IndexOf(',') + 2);
                */

                if (lastFiveUpdates.Count < 5 )
                {
                    
                    lastFiveUpdates.Add(new CircleMarker(value, dateTimeStr));
                }
            }

            return (lastFiveUpdates.Count == 5) ? lastFiveUpdates : null;
        }

        private void SetDefaultValuesToCircleMarkers()
        {
            for (int i = 0; i <= 4; i++)
            {
                //CircleMarkers[i].CmType = "Type";
                CircleMarkers[i].CmValue = 0;
                CircleMarkers[i].CmDate = "Date";
                //CircleMarkers[i].CmTime = "Time";
            }
        }

        private void SetValuesToCircleMarkers(List<CircleMarker> markers)
        {
            if (markers != null)
            {
                for (int i = 0; i <= 4; i++)
                {
                    //CircleMarkers[i].CmType = markers[4 - i].CmType;
                    CircleMarkers[i].CmValue = markers[4 - i].CmValue;
                    CircleMarkers[i].CmDate = markers[4 - i].CmDate;
                    //CircleMarkers[i].CmTime = markers[4 - i].CmTime;
                }
            }
            else
            {
                // Ako se u log fajlu nalazi manje od 5 promena vrednosti
                // Na kruzice se postavljaju default vrednosti
                SetDefaultValuesToCircleMarkers();
            }
        }

        public void OnShow()
        {
            if (SelectedType != null)
            {
                ErrorMessage = String.Empty;

                List<CircleMarker> markers = LoadLastFiveUpdates(SelectedType);

                SetValuesToCircleMarkers(markers);
            }
            else
            {
                ErrorMessage = "Type is required.";
            }
        }

        public string SelectedType
        {
            get { return selectedType; }
            set
            {
                selectedType = value;
                OnPropertyChanged("SelectedType");
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }
    }
}


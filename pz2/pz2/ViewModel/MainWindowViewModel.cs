using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using pz2.Model;
using System.IO;
using System.Windows;

namespace pz2.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public MyICommand<string> NavCommand { get; private set; }
        private NetworkEntitiesViewModel networkEntitiesViewModel=new NetworkEntitiesViewModel();
        private MeasurmentGraphViewModel measurmentGraphViewModel=new MeasurmentGraphViewModel();
        private NetworkDisplayViewModel networkDisplayViewModel=new NetworkDisplayViewModel();
        private int id;
        private double value;
        private bool file = false;
        private readonly Uri path = new Uri("LogFile.txt", UriKind.Relative);

        private BindableBase currentViewModel;

        public MainWindowViewModel()
        {

            
            currentViewModel = networkEntitiesViewModel;
            NavCommand = new MyICommand<string>(OnNav);
            createListener();


        }
        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(NetworkEntitiesViewModel.EntitiesShow.Count().ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            Console.WriteLine(incomming); //Na primer: "Objekat1:272"
                            string[] split = incomming.Split('_', ':');
                            int index = Int32.Parse(split[1]);
                            if (NetworkEntitiesViewModel.EntitiesShow.Count() > 0)
                                id = NetworkEntitiesViewModel.EntitiesShow[index].Id;
                            value = double.Parse(split[2]);
                            Entity v = new Entity(id);
                            if (id != -1)
                            {
                                NetworkEntitiesViewModel.EntitiesShow[index].Vrednost = value;
                                NetworkDisplayViewModel.EntityList.Add(v);
                                UpisUFajl();

                            }

                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
        private void UpisUFajl()
        {
            if (!file)
            {
                StreamWriter wr;
                using (wr = new StreamWriter(path.ToString()))
                {
                    wr.WriteLine("Date Time:\t" + DateTime.Now.ToString() + "\tObject" + id + "\tValue:\t" + value);
                }
            }
            else
            {
                StreamWriter wr;
                using (wr = new StreamWriter(path.ToString(), true))
                {
                    wr.WriteLine("Date Time:\t" + DateTime.Now.ToString() + "\tObject" + id + "\tValue:\t" + value);
                }
            }
            file = true;
        }
        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
            }
        }
        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "NetworkEntities":
                    CurrentViewModel = networkEntitiesViewModel;
                    break;
                case "NetworkDisplay":
                    CurrentViewModel = networkDisplayViewModel;
                    break;
                case "MeasurementGraph":
                    CurrentViewModel = measurmentGraphViewModel;
                    break;
            }
        }

    }
}

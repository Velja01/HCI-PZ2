using pz2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pz2.ViewModel
{
    public class NetworkDisplayViewModel: BindableBase
    {
        private Entity draggedItem = null;

        private bool dragging = false;

        public int draggingSource = -1;

        public ObservableCollection<Canvas> CanvasCollection { get; set; }
        public static ObservableCollection<Entity> EntityList { get; set; }
        public  ObservableCollection<Line1>  LineList { get; set; }

        public ObservableCollection<Brush> BorderBrushCollection { get; set; }

        private Entity selectedEntity;
        public MyICommand<object> SelectionChanged { get; set; }
        public MyICommand<object> DropEntity { get;set; }
        public MyICommand<object> MouseLeftButtonDown { get; set; }
        public MyICommand MouseLeftButtonUp { get; set; }
        public MyICommand<object> MouseRightButtonDown { get; set; }
        public MyICommand<object> OslobodiCanvas { get; set; }
        public Entity SelectedEntity
        {
            get { return selectedEntity; }
            set
            {
                selectedEntity = value;
                OnPropertyChanged("SelectedEntity");
            }
        }
        private bool isLineSourceSelected = false;

        private int sourceCanvasIndex = -1;

        private int destinationCanvasIndex = -1;

        private Line1 currentLine = new Line1();

        private Point linePoint1 = new Point();

        private Point linePoint2 = new Point();



        public NetworkDisplayViewModel()
        {
            EntityList = new ObservableCollection<Entity>();
            CanvasCollection = new ObservableCollection<Canvas>();
            for(int i = 0; i < 12; i++)
            {
                CanvasCollection.Add(new Canvas()
                {
                    Background = Brushes.LightGray,
                    AllowDrop = true
                }) ;
            }
            BorderBrushCollection = new ObservableCollection<Brush>();
            for (int i = 0; i < 12; i++)
            {
                BorderBrushCollection.Add(Brushes.DarkGray);
            }

            LineList=new ObservableCollection<Line1>();

            DropEntity = new MyICommand<object>(OnDrop);
            MouseLeftButtonDown = new MyICommand<object>(OnMouseLeftButtonDown);
            MouseLeftButtonUp = new MyICommand(OnMouseLeftButtonUp);
            SelectionChanged = new MyICommand<object>(OnSelectionChanged);
            OslobodiCanvas = new MyICommand<object>(OnOslobodiCanvas);
            MouseRightButtonDown = new MyICommand<object>(OnMouseRightButtonDown);
           

        }
        public void OnMouseRightButtonDown(object p)
        {
            int index = Convert.ToInt32(p);

            if (CanvasCollection[index].Resources["zauzet"] != null)
            {
                if (!isLineSourceSelected)
                {
                    sourceCanvasIndex = index;

                    linePoint1 = GetPointForCanvasIndex(sourceCanvasIndex);

                    currentLine.X1 = linePoint1.X;
                    currentLine.Y1 = linePoint1.Y;
                    currentLine.Source = sourceCanvasIndex;

                    isLineSourceSelected = true;
                }
                else
                {
                    destinationCanvasIndex = index;

                    if ((sourceCanvasIndex != destinationCanvasIndex) && !DoesLineAlreadyExist(sourceCanvasIndex, destinationCanvasIndex))
                    {
                        linePoint2 = GetPointForCanvasIndex(destinationCanvasIndex);

                        currentLine.X2 = linePoint2.X;
                        currentLine.Y2 = linePoint2.Y;
                        currentLine.Destination = destinationCanvasIndex;

                        LineList.Add(new Line1
                        {
                            X1 = currentLine.X1,
                            Y1 = currentLine.Y1,
                            X2 = currentLine.X2,
                            Y2 = currentLine.Y2,
                            Source = currentLine.Source,
                            Destination = currentLine.Destination
                        });

                        isLineSourceSelected = false;

                        linePoint1 = new Point();
                        linePoint2 = new Point();
                        currentLine = new Line1();
                    }
                    else
                    {
                        // Pocetak i kraj linije su u istom canvasu

                        isLineSourceSelected = false;

                        linePoint1 = new Point();
                        linePoint2 = new Point();
                        currentLine = new Line1();
                    }
                }
            }
            else
            {
                // Canvas na koji se postavlja tacka nije zauzet

                isLineSourceSelected = false;

                linePoint1 = new Point();
                linePoint2 = new Point();
                currentLine = new Line1();
            }
        }
        private bool DoesLineAlreadyExist(int source, int destination)
        {
            foreach (Line1 line in LineList)
            {
                if ((line.Source == source) && (line.Destination == destination))
                {
                    return true;
                }
                if ((line.Source == destination) && (line.Destination == source))
                {
                    return true;
                }
            }
            return false;
        }
        public void OnOslobodiCanvas(object p)
        {
            int index = Convert.ToInt32(p);

            if (CanvasCollection[index].Resources["zauzet"] != null)
            {
                // Crtanje linije se prekida ako je, izmedju postavljanja tacaka, entitet uklonjen sa canvas-a
                if (sourceCanvasIndex != -1)
                {
                    isLineSourceSelected = false;
                    sourceCanvasIndex = -1;
                    linePoint1 = new Point();
                    linePoint2 = new Point();
                    currentLine = new Line1();
                }

                DeleteLinesForCanvas(index);

                EntityList.Add((Entity)CanvasCollection[index].Resources["podaci"]);
                CanvasCollection[index].Background = Brushes.LightGray;
                CanvasCollection[index].Resources.Remove("zauzet");
                CanvasCollection[index].Resources.Remove("podaci");
                BorderBrushCollection[index] = Brushes.DarkGray;
            }
        }
        private void DeleteLinesForCanvas(int canvasIndex)
        {
            List<Line1> linesToDelete = new List<Line1>();

            for (int i = 0; i < LineList.Count; i++)
            {
                if ((LineList[i].Source == canvasIndex) || (LineList[i].Destination == canvasIndex))
                {
                    linesToDelete.Add(LineList[i]);
                }
            }

            foreach (Line1 line in linesToDelete)
            {
                LineList.Remove(line);
            }
        }
        private void OnSelectionChanged(object p)
        {
            if (!dragging)
            {
                dragging = true;
                draggedItem = selectedEntity;
                DragDrop.DoDragDrop((ListView)p, draggedItem, DragDropEffects.Move);
            }
        }
        private void OnMouseLeftButtonDown(object p)
        {
            if (!dragging)
            {
                int index = Convert.ToInt32(p);
                if (CanvasCollection[index].Resources["zauzet"] != null)
                {
                    dragging = true;
                    draggedItem = (Entity)(CanvasCollection[index].Resources["podaci"]);
                    draggingSource = index;
                    DragDrop.DoDragDrop(CanvasCollection[index], draggedItem, DragDropEffects.Move);
                }
            }
        }
        public void OnMouseLeftButtonUp()
        {
            draggedItem = null;
            SelectedEntity = null;
            dragging = false;
            draggingSource = -1;
        }
        public void OnDrop(object p)
        {
            if (draggedItem != null)
            {
                int index = Convert.ToInt32(p);

                if (CanvasCollection[index].Resources["zauzet"] == null)
                {
                    string nazivSlike = draggedItem.Slika.ToString();
                    string folderPath = @"C:\Users\veljk\Desktop\pz2novi\pz2\pz2\slike\";
                    string imagePath = System.IO.Path.Combine(folderPath, nazivSlike);
                    BitmapImage slika=new BitmapImage();
                    slika.BeginInit();
                    slika.UriSource= new Uri(imagePath);
                    slika.EndInit();

                    CanvasCollection[index].Background = new ImageBrush(slika);
                    CanvasCollection[index].Resources.Add("zauzet", true);
                    CanvasCollection[index].Resources.Add("podaci", draggedItem);
                    BorderBrushCollection[index] = Brushes.DarkGray;
                    if (draggingSource != -1)
                    {
                        CanvasCollection[draggingSource].Background = Brushes.LightGray;
                        CanvasCollection[draggingSource].Resources.Remove("zauzet");
                        CanvasCollection[draggingSource].Resources.Remove("podaci");
                        BorderBrushCollection[draggingSource] = Brushes.DarkGray;

                        UpdateLinesForCanvas(draggingSource, index);

                        // Crtanje linije se prekida ako je, izmedju postavljanja tacaka, entitet pomeren na drugo polje
                        if (sourceCanvasIndex != -1)
                        {
                            isLineSourceSelected = false;
                            sourceCanvasIndex = -1;
                            linePoint1 = new Point();
                            linePoint2 = new Point();
                            currentLine = new Line1();
                        }

                        draggingSource = -1;
                    }

                    if (EntityList.Contains(draggedItem))
                    {
                        EntityList.Remove(draggedItem);
                    }
                }

            }
        }
        private void UpdateLinesForCanvas(int sourceCanvas, int destinationCanvas)
        {
            for (int i = 0; i < LineList.Count; i++)
            {
                if (LineList[i].Source == sourceCanvas)
                {
                    Point newSourcePoint = GetPointForCanvasIndex(destinationCanvas);
                    LineList[i].X1 = newSourcePoint.X;
                    LineList[i].Y1 = newSourcePoint.Y;
                    LineList[i].Source = destinationCanvas;
                }
                else if (LineList[i].Destination == sourceCanvas)
                {
                    Point newDestinationPoint = GetPointForCanvasIndex(destinationCanvas);
                    LineList[i].X2 = newDestinationPoint.X;
                    LineList[i].Y2 = newDestinationPoint.Y;
                    LineList[i].Destination = destinationCanvas;
                }
            }
        }
        private Point GetPointForCanvasIndex(int canvasIndex)
        {
            double x = 0, y = 0;

            for (int row = 0; row <= 3; row++)
            {
                for (int col = 0; col <= 2; col++)
                {
                    int currentIndex = row * 3 + col;

                    if (canvasIndex == currentIndex)
                    {
                        //x = 50 + (col * 100);
                        //y = 50 + (row * 100);
                        x = 44 + (col * 88);
                        y = 44 + (row * 88);
                        break;
                    }
                }
            }
            return new Point(x, y);
        }
    }
}

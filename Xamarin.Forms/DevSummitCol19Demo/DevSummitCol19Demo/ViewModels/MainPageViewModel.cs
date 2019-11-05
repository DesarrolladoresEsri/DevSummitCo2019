using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DevSummitCol19Demo.ViewModels
{
  public class MainPageViewModel : ViewModelBase
  {
    private Map _map;
    private GraphicsOverlayCollection _graphicOverlyaCollection;
    private Viewpoint _newViewpoint;

    /// <summary>
    /// 
    /// </summary>
    public Map Map
    {
      get => _map;
      set => SetProperty(ref _map, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public GraphicsOverlayCollection GraphicsOverlayCollection
    {
      get => _graphicOverlyaCollection;
      set => SetProperty(ref _graphicOverlyaCollection, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public Viewpoint NewViewpoint
    {
      get => _newViewpoint;
      set => SetProperty(ref _newViewpoint, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public ICommand AddGraphicsCommand { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public ICommand CenterMapCommand { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public ICommand ClearEventsCommand { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    private MapPoint CentralPoint { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="navigationService"></param>
    public MainPageViewModel(INavigationService navigationService)
        : base(navigationService)
    {
      Title = "Demo Nugets EsriDevSummit Colombia";

      Map = new Map(Basemap.CreateStreets());

      GraphicsOverlayCollection = new GraphicsOverlayCollection()
      {
        new GraphicsOverlay() { Id = "Hotel Cosmos 100"},
        new GraphicsOverlay() { Id = "Eventos"}
      };
      CentralPoint = new MapPoint(-74.054424, 4.685715, SpatialReferences.Wgs84);

      var graphic = new Graphic()
      {
        Geometry = CentralPoint,
        Symbol = new SimpleMarkerSymbol
        {
          Color = Color.DarkGreen,
          Style = SimpleMarkerSymbolStyle.Diamond,
          Size = 20
        }
      };

      var textGraphic = new Graphic()
      {
        Geometry = CentralPoint,
        Symbol = new TextSymbol
        {
          Text = "Hotel Cosmos 100",
          Color = Color.Black,
          Size = 15,
          OffsetY = 20
        }
      };

      GraphicsOverlayCollection.First().Graphics.Add(graphic);
      GraphicsOverlayCollection.First().Graphics.Add(textGraphic);
      Map.InitialViewpoint = new Viewpoint(CentralPoint, 5000);

      AddGraphicsCommand = new DelegateCommand<MapPoint>(AddGraphicsAction);
      ClearEventsCommand = new DelegateCommand(ClearEventsAction);
      CenterMapCommand = new DelegateCommand(CenterMapAction);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>
    private void AddGraphicsAction(MapPoint location)
    {
      var graphic = new Graphic()
      {
        Geometry = location,
        Symbol = new SimpleMarkerSymbol
        {
          Color = Color.Blue,
          Style = SimpleMarkerSymbolStyle.Cross,
          Size = 20
        }
      };
      var graphicOverlay = GraphicsOverlayCollection.Where(go => go.Id == "Eventos").FirstOrDefault();
      if (graphicOverlay != null)
      {
        graphicOverlay.Graphics.Add(graphic);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ClearEventsAction()
    {
      var graphicOverlay = GraphicsOverlayCollection.Where(go => go.Id == "Eventos").FirstOrDefault();
      if (graphicOverlay != null)
      {
        graphicOverlay.Graphics.Clear();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CenterMapAction()
    {
      NewViewpoint = new Viewpoint(CentralPoint, 5000);
    }

  }
}

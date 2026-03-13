using System.ComponentModel;
using Eco.Core.Controller;
using Eco.Core.Systems;
using Eco.Gameplay.Aliases;
using Eco.Gameplay.Civics.GameValues;
using Eco.Shared.Localization;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.Shared.View;

namespace CravingsBoard;

/// <summary>
/// Transient ViewModel for ViewEditor — player picker for tracked cravings.
/// Uses GamePickerList(typeof(IAlias)) for player/alias selection.
/// </summary>
[Serialized, AutogenClass]
public class CookConfigViewModel : IController, IViewController, IHasUniversalID, INotifyPropertyChanged
{
    private int _controllerID;
    public ref int ControllerID => ref _controllerID;

    [SyncToView, Autogen, AutoRPC, LocDisplayName("Tracked Players"),
     LocDescription("Select players whose cravings you want to see.")]
    public GamePickerList PlayerFilter { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public CookConfigViewModel()
    {
        PlayerFilter = new GamePickerList(typeof(IAlias));
    }
}

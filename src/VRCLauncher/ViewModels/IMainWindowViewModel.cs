using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using VRCLauncher.Models;

namespace VRCLauncher.ViewModels
{
    public interface IMainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        ReactiveProperty<string> Uri { get; }
        ReactiveProperty<string> WorldId { get; }
        ReactiveProperty<string> InstanceId { get; }
        ReactiveProperty<InstanceType> InstanceType { get; }
        ReactiveProperty<string?> InstanceOwnerId { get; }
        ReactiveProperty<Region> Region { get; }
        ReactiveProperty<string?> Nonce { get; }
        ReactiveCommand LaunchVRCommand { get; }
        ReactiveCommand LaunchDesktopCommand { get; }
        Dictionary<InstanceType, string> InstanceTypeItems { get; }
    }
}

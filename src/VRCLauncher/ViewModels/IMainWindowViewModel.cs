using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
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
        ReactiveProperty<string?> Nonce { get; }
        ReactiveCommand<Window> LaunchVRCommand { get; }
        ReactiveCommand<Window> LaunchDesktopCommand { get; }
        Dictionary<InstanceType, string> InstanceTypeItems { get; }
    }
}

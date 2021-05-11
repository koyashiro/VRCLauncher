using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using VRCLauncher.Models;
using VRCLauncher.Utils;

namespace VRCLauncher.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private bool _disposedValue;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            var args = Environment.GetCommandLineArgs();
            var uri = args.Length > 1 ? args[1] : string.Empty;
            Uri = new ReactiveProperty<string>(uri).AddTo(Disposable);

            WorldId = new ReactiveProperty<string>(string.Empty).AddTo(Disposable);
            InstanceId = new ReactiveProperty<string>().AddTo(Disposable);
            InstanceType = new ReactiveProperty<InstanceType>(Models.InstanceType.Public).AddTo(Disposable);
            InstanceOwnerId = new ReactiveProperty<string?>().AddTo(Disposable);
            Nonce = new ReactiveProperty<string>(string.Empty).AddTo(Disposable);

            var launchParameterObservables = Observable.Merge(
                WorldId.ToUnit(),
                InstanceId.ToUnit(),
                InstanceType.ToUnit(),
                InstanceOwnerId.ToUnit(),
                Nonce.ToUnit());

            Uri.Subscribe(_ => UpdateLaunchParameterIfNeeded());
            launchParameterObservables.Subscribe(_ => UpdateUriIfNeeded());

            var canLaunchCommand = launchParameterObservables.Select(_ =>
                {
                    var launchParameter = new LaunchParameter(
                        WorldId.Value,
                        InstanceId.Value,
                        InstanceType.Value,
                        InstanceOwnerId.Value,
                        Nonce.Value
                    );
                    return launchParameter.IsValid();
                });
            void launchCommandAction(object parameter, Action<string, string> launchAction)
            {
                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (parameter is not Window window)
                {
                    throw new ArgumentException($"{parameter} is not Window", nameof(parameter));
                }

                var config = Config.Load();
                launchAction(config.VRChatPath, Uri.Value);
                window.Close();
            }

            LaunchVRCommand = new ReactiveCommand(canLaunchCommand).AddTo(Disposable);
            LaunchVRCommand.Subscribe(parameter => launchCommandAction(parameter, Launcher.LaunchVR));

            LaunchDesktopCommand = new ReactiveCommand(canLaunchCommand).AddTo(Disposable);
            LaunchVRCommand.Subscribe(parameter => launchCommandAction(parameter, Launcher.LaunchDesktop));
        }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ReactiveProperty<string> Uri { get; }
        public ReactiveProperty<string> WorldId { get; }
        public ReactiveProperty<string> InstanceId { get; }
        public ReactiveProperty<InstanceType> InstanceType { get; }
        public ReactiveProperty<string?> InstanceOwnerId { get; }
        public ReactiveProperty<string> Nonce { get; }

        public ReactiveCommand LaunchVRCommand { get; }
        public ReactiveCommand LaunchDesktopCommand { get; }

        public Dictionary<InstanceType, string> InstanceTypeItems => new()
        {
            { Models.InstanceType.Public, "Public" },
            { Models.InstanceType.FriendPlus, "Friend+" },
            { Models.InstanceType.FriendOnly, "Friend Only" },
            { Models.InstanceType.InvitePlus, "Invite+" },
            { Models.InstanceType.InviteOnly, "Invite" },
        };

        private void UpdateLaunchParameterIfNeeded()
        {
            if (LaunchParameter.TryParse(Uri.Value, out var launchParameter))
            {
                WorldId.Value = launchParameter.WorldId;
                InstanceId.Value = launchParameter.InstanceId;
                InstanceType.Value = launchParameter.InstanceType;
                InstanceOwnerId.Value = launchParameter.InstanceOwnerId;
                Nonce.Value = launchParameter.Nonce;
            }
        }

        private void UpdateUriIfNeeded()
        {
            var launchParameter = new LaunchParameter(
                WorldId.Value,
                InstanceId.Value,
                InstanceType.Value,
                InstanceOwnerId.Value,
                Nonce.Value
            );

            if (launchParameter.IsValid())
            {
                Uri.Value = launchParameter.ToString();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Disposable.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

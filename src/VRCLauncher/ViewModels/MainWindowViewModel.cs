using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using VRCLauncher.Models;
using VRCLauncher.Services;
using VRCLauncher.Wrappers;

namespace VRCLauncher.ViewModels
{
    public class MainWindowViewModel : BindableBase, IMainWindowViewModel
    {
        private bool _disposedValue;
        private readonly ILaunchService _launchService;
        private readonly IWindowWrapper _windowWrapper;

        public MainWindowViewModel(ILaunchService launchService, IWindowWrapper windowWrapper)
        {
            _launchService = launchService;
            _windowWrapper = windowWrapper;

            var args = Environment.GetCommandLineArgs();
            var uri = args.Length > 1 ? args[1] : string.Empty;
            Uri = new ReactiveProperty<string>(uri).AddTo(Disposable);

            WorldId = new ReactiveProperty<string>(string.Empty).AddTo(Disposable);
            InstanceId = new ReactiveProperty<string>(string.Empty).AddTo(Disposable);
            InstanceType = new ReactiveProperty<InstanceType>(Models.InstanceType.Public).AddTo(Disposable);
            InstanceOwnerId = new ReactiveProperty<string?>().AddTo(Disposable);
            Region = new ReactiveProperty<Region>(Models.Region.None).AddTo(Disposable);
            Nonce = new ReactiveProperty<string?>().AddTo(Disposable);

            var launchParameterObservables = Observable.Merge(
                WorldId.ToUnit(),
                InstanceId.ToUnit(),
                InstanceType.ToUnit(),
                InstanceOwnerId.ToUnit(),
                Region.ToUnit(),
                Nonce.ToUnit());

            Uri.Subscribe(_ => UpdateLaunchParameterIfNeeded());
            launchParameterObservables.Subscribe(_ => UpdateUriIfNeeded());

            var canLaunchCommand = launchParameterObservables.Select(_ =>
                {
                    var launchParameter = new LaunchParameter
                    {
                        WorldId = WorldId.Value,
                        InstanceId = InstanceId.Value,
                        InstanceType = InstanceType.Value,
                        InstanceOwnerId = InstanceOwnerId.Value,
                        Region = Region.Value,
                        Nonce = Nonce.Value,
                    };
                    return launchParameter.IsValid();
                });
            void launchCommandAction(object parameter, Action<string> launchAction)
            {
                launchAction(Uri.Value);
                _windowWrapper.Close();
            }

            LaunchVRCommand = new ReactiveCommand(canLaunchCommand, false).AddTo(Disposable);
            LaunchVRCommand.Subscribe(parameter => launchCommandAction(parameter, _launchService.LaunchVR));

            LaunchDesktopCommand = new ReactiveCommand(canLaunchCommand, false).AddTo(Disposable);
            LaunchDesktopCommand.Subscribe(parameter => launchCommandAction(parameter, _launchService.LaunchDesktop));
        }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ReactiveProperty<string> Uri { get; }
        public ReactiveProperty<string> WorldId { get; }
        public ReactiveProperty<string> InstanceId { get; }
        public ReactiveProperty<InstanceType> InstanceType { get; }
        public ReactiveProperty<string?> InstanceOwnerId { get; }
        public ReactiveProperty<Region> Region { get; }
        public ReactiveProperty<string?> Nonce { get; }

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
                Region.Value = launchParameter.Region;
                Nonce.Value = launchParameter.Nonce;
            }
        }

        private void UpdateUriIfNeeded()
        {
            var launchParameter = new LaunchParameter
            {
                WorldId = WorldId.Value,
                InstanceId = InstanceId.Value,
                InstanceType = InstanceType.Value,
                InstanceOwnerId = InstanceOwnerId.Value,
                Region = Region.Value,
                Nonce = Nonce.Value,
            };

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

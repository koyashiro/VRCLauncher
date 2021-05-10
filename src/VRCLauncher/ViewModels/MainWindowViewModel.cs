using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.ComponentModel;
using System.IO;
using System.Reactive.Disposables;
using System.Windows;
using VRCLauncher.Models;
using VRCLauncher.Utils;

namespace VRCLauncher.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private bool _disposedValue;

        public event PropertyChangedEventHandler PropertyChanged;


        public MainWindowViewModel(string? uri = null)
        {
            if (uri is null)
            {
                Uri = new ReactiveProperty<string>().AddTo(Disposable);
            }
            else
            {
                Uri = new ReactiveProperty<string>(uri).AddTo(Disposable);
            }

            if (LaunchParameter.TryParse(uri, out var launchParameter))
            {
                WorldId = new ReactiveProperty<string>(launchParameter.WorldId).AddTo(Disposable);
                InstanceId = new ReactiveProperty<string>(launchParameter.InstanceId).AddTo(Disposable);
                InstanceType = new ReactiveProperty<InstanceType>(launchParameter.InstanceType).AddTo(Disposable);
                InstanceOwnerId = new ReactiveProperty<string?>(launchParameter.InstanceOwnerId).AddTo(Disposable);
                Nonce = new ReactiveProperty<string>(launchParameter.Nonce).AddTo(Disposable);
            }
            else
            {
                WorldId = new ReactiveProperty<string>().AddTo(Disposable);
                InstanceId = new ReactiveProperty<string>().AddTo(Disposable);
                InstanceType = new ReactiveProperty<InstanceType>(VRCLauncher.InstanceType.Public).AddTo(Disposable);
                InstanceOwnerId = new ReactiveProperty<string?>().AddTo(Disposable);
                Nonce = new ReactiveProperty<string>().AddTo(Disposable);
            }

            const string VRCHAT_BIN = "VRChat.exe";
            var vrchatPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, VRCHAT_BIN);
            vrchatPath = @"C:\Program Files (x86)\Steam\steamapps\common\VRChat\VRChat.exe";

            LaunchVRCommand = new ReactiveCommand().AddTo(Disposable);
            LaunchVRCommand.Subscribe(parameter =>
            {
                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (parameter is not Window window)
                {
                    throw new ArgumentException($"{parameter} is not Window", nameof(parameter));
                }

                Launcher.LaunchVR(vrchatPath, Uri.Value);
                window.Close();
            });


            LaunchDesktopCommand = new ReactiveCommand().AddTo(Disposable);
            LaunchDesktopCommand.Subscribe(parameter =>
           {
                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (parameter is not Window window)
                {
                    throw new ArgumentException($"{parameter} is not Window", nameof(parameter));
                }

                Launcher.LaunchDesktop(vrchatPath, Uri.Value);
                window.Close();
           });
        }

        private CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ReactiveProperty<string> Uri { get; }
        public ReactiveProperty<string> WorldId { get; }
        public ReactiveProperty<string> InstanceId { get; }
        public ReactiveProperty<InstanceType> InstanceType { get; }
        public ReactiveProperty<string?> InstanceOwnerId { get; }
        public ReactiveProperty<string> Nonce { get; }

        public LaunchParameter LaunchParameter => new(
            WorldId.Value,
            InstanceId.Value,
            InstanceType.Value,
            InstanceOwnerId.Value,
            Nonce.Value
        );

        public ReactiveCommand LaunchVRCommand { get; }
        public ReactiveCommand LaunchDesktopCommand { get; }

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

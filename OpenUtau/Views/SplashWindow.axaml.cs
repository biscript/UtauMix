using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using OpenUtau.Classic;
using OpenUtau.Core;
using Serilog;

namespace OpenUtau.App.Views {
    public partial class SplashWindow : Window {
        private DispatcherTimer? animationTimer;

        public SplashWindow() {
            InitializeComponent();

            // Устанавливаем начальные параметры для анимации
            this.Opacity = 0;
            this.RenderTransform = new ScaleTransform(0.8, 0.8);
            this.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);

            // Настройка логотипов
            if (ThemeManager.IsDarkMode) {
                LogoTypeLight.IsVisible = false;
                LogoTypeDark.IsVisible = true;
            } else {
                LogoTypeLight.IsVisible = true;
                LogoTypeDark.IsVisible = false;
            }

            this.Cursor = new Cursor(StandardCursorType.AppStarting);
            this.Opened += SplashWindow_Opened;
        }

        private void SplashWindow_Opened(object? sender, EventArgs e) {
            AnimateIn(); // запускаем анимацию появления
            Start();
        }

        private void AnimateIn() {
            animationTimer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
            };

            double duration = 0.6; // секунды
            double elapsed = 0;

            animationTimer.Tick += (s, e) => {
                elapsed += 0.016;
                double t = Math.Min(elapsed / duration, 1.0);

                // плавное появление + увеличение
                this.Opacity = t;
                this.RenderTransform = new ScaleTransform(0.8 + 0.2 * t, 0.8 + 0.2 * t);

                if (t >= 1.0) {
                    animationTimer.Stop();
                }
            };

            animationTimer.Start();
        }

        private void Start() {
            var mainThread = Thread.CurrentThread;
            var mainScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Run(() => {
                Log.Information("Initializing OpenUtau.");
                ToolsManager.Inst.Initialize();
                SingerManager.Inst.Initialize();
                DocManager.Inst.Initialize(mainThread, mainScheduler);
                DocManager.Inst.PostOnUIThread = action => Avalonia.Threading.Dispatcher.UIThread.Post(action);
                Log.Information("Initialized OpenUtau.");
                InitAudio();
            }).ContinueWith(t => {
                if (t.IsFaulted) {
                    Log.Error(t.Exception?.Flatten(), "Failed to Start.");
                    OpenUtau.App.Views.MessageBox.ShowError(this, t.Exception, "Failed to Start OpenUtau")
                        .ContinueWith(_ => { Close(); });
                    return;
                }

                if (App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop) {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    desktop.MainWindow = mainWindow;
                    mainWindow.InitProject();
                    Close();
                }
            }, CancellationToken.None, TaskContinuationOptions.None, mainScheduler);
        }

        private static void InitAudio() {
            Log.Information("Initializing audio.");
            if (!OS.IsWindows() || Core.Util.Preferences.Default.PreferPortAudio) {
                try { PlaybackManager.Inst.AudioOutput = new Audio.MiniAudioOutput(); }
                catch (Exception e1) { Log.Error(e1, "Failed to init MiniAudio"); }
            } else {
                try { PlaybackManager.Inst.AudioOutput = new Audio.NAudioOutput(); }
                catch (Exception e2) { Log.Error(e2, "Failed to init NAudio"); }
            }
            Log.Information("Initialized audio.");
        }
    }
}

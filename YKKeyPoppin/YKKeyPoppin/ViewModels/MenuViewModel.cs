namespace YKKeyPoppin.ViewModels
{
    using System.Collections.Generic;
    using YKKeyPoppin.Models;
    using YKToolkit.Bindings;

    internal class MenuViewModel : NotificationObject
    {
        public MenuViewModel()
        {
            this._viewModels = new List<IMenuContentViewModel>()
            {
                this.Top5KeysViewModel,
                //this.AggregationViewModel,
                this.GraphViewModel,
                this.KeyConfViewModel,
            };

            KeyHook.Current.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(KeyInfo info)
        {
            RaisePropertyChanged("Top5Keys");
        }

        private IMenuContentViewModel _top5KeysViewModel = new Top5KeysViewModel();
        public IMenuContentViewModel Top5KeysViewModel { get { return this._top5KeysViewModel; } }

        private IMenuContentViewModel _aggregationViewModel = new AggregationViewModel();
        public IMenuContentViewModel AggregationViewModel { get { return this._aggregationViewModel; } }

        private IMenuContentViewModel _graphViewModel = new GraphViewModel();
        public IMenuContentViewModel GraphViewModel { get { return this._graphViewModel; } }

        private IMenuContentViewModel _keyConfViewModel = new KeyConfViewModel();
        public IMenuContentViewModel KeyConfViewModel { get { return this._keyConfViewModel; } }

        private IMenuContentViewModel PreviousViewModel { get; set; }

        private List<IMenuContentViewModel> _viewModels;

        private IMenuContentViewModel CurrentViewModel { get { return this._viewModels[this.SelectedIndex]; } }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return this._selectedIndex; }
            private set { SetProperty(ref this._selectedIndex, value); }
        }

        private bool _isNext;
        public bool IsNext
        {
            get { return this._isNext; }
            private set { SetProperty(ref this._isNext, value); }
        }

        private DelegateCommand _previousCommand;
        public DelegateCommand PreviousCommand
        {
            get
            {
                return this._previousCommand ?? (this._previousCommand = new DelegateCommand(_ =>
                {
                    this.PreviousViewModel = this.CurrentViewModel;
                    this.IsNext = false;
                    this.SelectedIndex = this.SelectedIndex != 0 ? this.SelectedIndex - 1 : this._viewModels.Count - 1;
                }));
            }
        }

        private DelegateCommand _nextCommand;
        public DelegateCommand NextCommand
        {
            get
            {
                return this._nextCommand ?? (this._nextCommand = new DelegateCommand(_ =>
                {
                    this.PreviousViewModel = this.CurrentViewModel;
                    this.IsNext = true;
                    this.SelectedIndex = this.SelectedIndex < this._viewModels.Count - 1 ? this.SelectedIndex + 1 : 0;
                }));
            }
        }

        private DelegateCommand _transitionCompletedCommand;
        public DelegateCommand TransitionCompletedCommand
        {
            get
            {
                return this._transitionCompletedCommand ?? (this._transitionCompletedCommand = new DelegateCommand(_ =>
                {
                    if (this.PreviousViewModel != null) this.PreviousViewModel.Unloaded();
                    this.PreviousViewModel = null;

                    if (this.CurrentViewModel != null) this.CurrentViewModel.Loaded();
                }));
            }
        }

        private DelegateCommand _changeThemeCommand;
        public DelegateCommand ChangeThemeCommand
        {
            get
            {
                return this._changeThemeCommand ?? (this._changeThemeCommand = new DelegateCommand(_ =>
                {
                    YKToolkit.Controls.ThemeManager.Instance.ChangeNextTheme();
                }));
            }
        }

        private DelegateCommand _exitCommand;
        public DelegateCommand ExitCommand
        {
            get
            {
                return this._exitCommand ?? (this._exitCommand = new DelegateCommand(_ =>
                {
                    App.Instance.End();
                }));
            }
        }
    }
}

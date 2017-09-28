namespace YKKeyPoppin.ViewModels
{
    using System.Collections.Generic;
using YKKeyPoppin.Models;
using YKToolkit.Bindings;

    internal class MenuViewModel : NotificationObject
    {
        public MenuViewModel()
        {
            this._viewModels = new List<object>()
            {
                this.Top5KeysViewModel,
                //this.AggregationViewModel,
                this.GraphViewModel,
            };

            KeyHook.Current.KeyUp += OnKeyUp;
        }

        private void OnKeyUp(KeyInfo info)
        {
            RaisePropertyChanged("Top5Keys");
        }

        private Top5KeysViewModel _top5KeysViewModel = new Top5KeysViewModel();
        public Top5KeysViewModel Top5KeysViewModel { get { return this._top5KeysViewModel; } }

        private AggregationViewModel _aggregationViewModel = new AggregationViewModel();
        public AggregationViewModel AggregationViewModel { get { return this._aggregationViewModel; } }

        private GraphViewModel _graphViewModel = new GraphViewModel();
        public GraphViewModel GraphViewModel { get { return this._graphViewModel; } }

        private List<object> _viewModels;

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
                    if (this._viewModels[this.SelectedIndex] == this.GraphViewModel)
                    {
                        this.GraphViewModel.Loaded();
                    }
                    else
                    {
                        this.GraphViewModel.UnLoaded();
                    }
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

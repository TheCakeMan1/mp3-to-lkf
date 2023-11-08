using ReactiveUI;

namespace LKF_CODER.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _Folder_in = "";
        private string _Folder_out = "";
        private string _Title = "";
        private string _Author = "";
        private int _Nomer = 1;
        private bool _Ca = false;

        public string Folder_in
        {
            get => _Folder_in;
            set => this.RaiseAndSetIfChanged(ref _Folder_in, value);
        }

        public void cha()
        {
            if (_Ca == false)
            {
                string name = string.Empty;
                System.IO.DriveInfo[] DI = System.IO.DriveInfo.GetDrives();
                foreach (System.IO.DriveInfo di in DI)
                {
                    if (di.DriveType.ToString() == "Removable")
                        name = di.Name;
                }
                _Folder_out = name;
                _Ca = true;
            }
        }

        public string Folder_out
        {
            get
            {
                cha();
                return _Folder_out;
            }
            set => this.RaiseAndSetIfChanged(ref _Folder_out, value);
        }

        public string Title
        {
            get => _Title;
            set => this.RaiseAndSetIfChanged(ref _Title, value);
        }

        public string Author
        {
            get => _Author;
            set { }
        }
        public int Nomer
        {
            get => _Nomer;
            set { }
        }
    }
}
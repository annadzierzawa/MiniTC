using System.Linq;


namespace MVVM_start.ViewModel
{
    using MiniTC.Model;
    using MVVM_start.ViewModel.BaseClass;
    using System.IO;
    using System.Windows.Input;

    class ViewModel : ViewModelBase
    {
        private Model model = new Model();
        public string PathRight
        {
            get { return model.rightPanelPath; }
            set { model.activePanel = ActivePanel.RIGHT; model.rightPanelPath = value; onPropertyChanged(nameof(PathRight)); }
        }


        public string PathLeft
        {
            get { return model.leftPanelPath; }
            set { model.activePanel = ActivePanel.LEFT; model.leftPanelPath = value; onPropertyChanged(nameof(PathRight)); }
        }

        //CopyButtonClicked

        private ICommand copyButtonClicked = null;
        public ICommand CopyButtonClicked
        {
            get
            {
                if (copyButtonClicked == null)
                    copyButtonClicked = new RelayCommand(
                        arg => { 
                            copyFile();
                            //todo odswiec listy plikow
                        },
                        arg => arePathsValidForCopying()
            );
                return copyButtonClicked;
            }
        }

        private bool arePathsValidForCopying()
        {
            if (model.leftPanelPath != null && model.rightPanelPath != null)
            {
                FileAttributes attrLeft = File.GetAttributes(model.leftPanelPath);
                FileAttributes attrRight = File.GetAttributes(model.rightPanelPath);
                if (model.activePanel == ActivePanel.RIGHT)
                {
                    if (attrLeft.HasFlag(FileAttributes.Directory) && !attrRight.HasFlag(FileAttributes.Directory))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (attrRight.HasFlag(FileAttributes.Directory) && !attrLeft.HasFlag(FileAttributes.Directory))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private void copyFile()
        {
            if (model.activePanel == ActivePanel.RIGHT)
            {
                if (arePathsValidForCopying())
                {
                    File.Copy(model.rightPanelPath,model.leftPanelPath+ "\\" + model.rightPanelPath.Split("\\").Last());
                }
            }
            else
            {
                if (arePathsValidForCopying())
                { 
                    File.Copy(model.leftPanelPath, model.rightPanelPath+ "\\" + model.leftPanelPath.Split("\\").Last());
                }
            }
        }

        public ViewModel()
        {

        }

    }
}

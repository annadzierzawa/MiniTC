
namespace MiniTC.Model
{
    class Model
    {
        public string leftPanelPath;

        public string rightPanelPath;

        public ActivePanel activePanel;
    }

    enum ActivePanel
    { 
        LEFT,RIGHT
    }
}

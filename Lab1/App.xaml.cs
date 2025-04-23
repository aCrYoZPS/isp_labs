namespace Lab1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            const int newHeight = 330;
            const int newWidth = 350;

            var newWindow = new Window(new AppShell())
            {
                Height = newHeight,
                Width = newWidth,
                MinimumHeight = newHeight,
                MinimumWidth = newWidth,
            };

            return newWindow;
        }
    }
}
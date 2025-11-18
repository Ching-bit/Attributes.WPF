using System.Windows;

namespace Attributes.WPF.Demo
{
    [WithDependencyProperty(typeof(double), "MyNumber", 10.0)]
    [WithDependencyProperty(typeof(string), "MyString", "my DenpendencyProperty")]
    [WithDependencyProperty(typeof(MyModel), "MyTestModel", null)]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MyTestModel = new MyModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyNumber++;
            MyString += " test";
            MyTestModel.Value++;
        }

    }
}
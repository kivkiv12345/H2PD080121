using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SchemaClasses;

namespace GUILayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            createEditForm<Student>();
            createEditForm<Teacher>();
        }

        public void createEditForm<T>() where T : class
        {
            Type tclass = typeof(T);

            ContentArea contentArea = new ContentArea();

            foreach (PropertyInfo prop in tclass.GetProperties())
            {
                Label propLabel = new Label();
                propLabel.Content = prop.Name;

                contentArea.formWidgetPanel.Children.Add(propLabel);
            }

            void changeContentView(object sender, RoutedEventArgs e)
            {
                this.ContentView.Content = contentArea;
            }

            Button stackButton = new Button();
            stackButton.Click += changeContentView;
            stackButton.Content = tclass.Name;

            this.editorButtonStack.Children.Add(stackButton);
        }
    }
}

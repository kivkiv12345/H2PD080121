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

            StackPanel contentArea = new StackPanel();

            PropertyInfo[] tclassProps = tclass.GetProperties();

            int longest = 0;

            foreach (PropertyInfo prop in tclassProps)
                if (prop.Name.Length > longest)
                    longest = prop.Name.Length;

            foreach (PropertyInfo prop in tclassProps)
            {
                StackPanel horizontalPanel = new StackPanel();
                horizontalPanel.Orientation = Orientation.Horizontal;

                TextBox propLabel = new TextBox();
                propLabel.IsReadOnly = true;
                propLabel.Text = prop.Name;
                propLabel.MinWidth = longest * 8;

                TextBox propInput = new TextBox();
                propInput.Width = 100;

                horizontalPanel.Children.Add(propLabel);
                horizontalPanel.Children.Add(propInput);

                contentArea.Children.Add(horizontalPanel);

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

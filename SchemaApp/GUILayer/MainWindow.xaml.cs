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

        public void createEditForm<T>() where T : Person
        {
            Type tclass = typeof(T);

            StackPanel contentArea = new StackPanel();

            PropertyInfo[] tclassProps = tclass.GetProperties();

            int longest = 0;
            const int inputFieldWidth = 100;
            const int propLabelWidthMult = 8;

            foreach (PropertyInfo prop in tclassProps)
                if (prop.Name.Length > longest)
                    longest = prop.Name.Length;

            List<FieldStackPanel> fieldStackList = new List<FieldStackPanel>();

            foreach (PropertyInfo prop in tclassProps)
            {
                FieldStackPanel horizontalPanel = new FieldStackPanel();
                horizontalPanel.Orientation = Orientation.Horizontal;

                TextBox propLabel = new TextBox();
                horizontalPanel.FieldNameBox = propLabel;
                propLabel.IsReadOnly = true;
                propLabel.Text = prop.Name;
                propLabel.MinWidth = longest * propLabelWidthMult;

                // TODO Kevin: We might want the possibility to add a dropdown for enums here.
                TextBox propInput = new TextBox();
                horizontalPanel.InputFieldBox = propInput;
                propInput.Width = inputFieldWidth;

                horizontalPanel.Children.Add(propLabel);
                horizontalPanel.Children.Add(propInput);

                contentArea.Children.Add(horizontalPanel);

                fieldStackList.Add(horizontalPanel);

            }

            void saveEditForm(object sender, RoutedEventArgs e)
            {
                string firstName = "", lastName = "";

                foreach (FieldStackPanel field in fieldStackList)
                {
                    if (field.FieldNameBox.Text == "FirstName")
                    {
                        firstName = field.InputFieldBox.Text;
                    } 
                    else if (field.FieldNameBox.Text == "LastName")
                    {
                        lastName = field.InputFieldBox.Text;
                    }
                }

                PersonController.CreatePerson<T>(firstName, lastName);
            }

            Button saveButton = new Button();
            saveButton.MinWidth = longest * propLabelWidthMult + inputFieldWidth;
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Click += saveEditForm;
            saveButton.Content = "Save";

            contentArea.Children.Add(saveButton);

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

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

            InitDataModelForms<Student>();
            InitDataModelForms<Teacher>();
            InitDataModelForms<Subject>();
            InitDataModelForms<CampusTeam>();

            editorButtonStack.IsEnabled = false;  // Will be enabled when a successful connection to the database has been established.
        }

        private void InitDataModelForms<T>() where T : DataModel, new()
        {
            Type tclass = typeof(T);

            StackPanel contentArea = new StackPanel();

            DataModelOptions showAndCreateView = new DataModelOptions();
            ModelDataGrid dataGrid = new ModelDataGrid();

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

                T instance = DataController.CreateInstance<T>();
                Type type = instance.GetType();

                foreach (FieldStackPanel field in fieldStackList)
                {
                    try
                    {
                        type.GetProperty(field.FieldNameBox.Text).SetValue(instance, field.InputFieldBox.Text);
                    } catch
                    {
                        Console.WriteLine("Failed to write value");
                    }
                    
                }
                DataController.Save(instance);
            }

            Button saveButton = new Button();
            saveButton.MinWidth = longest * propLabelWidthMult + inputFieldWidth;
            saveButton.HorizontalAlignment = HorizontalAlignment.Left;
            saveButton.Click += saveEditForm;
            saveButton.Content = "Save";

            contentArea.Children.Add(saveButton);

            void showModelEditForm(object sender, RoutedEventArgs e)
            {
                this.ContentView.Content = contentArea;
            }

            void showModelInstances(object sender, RoutedEventArgs e)
            {
                dataGrid.modelDataGrid.ItemsSource = DataController.Filter<T>();
                this.ContentView.Content = dataGrid;
            }

            void showModelOptions(object sender, RoutedEventArgs e)
            {
                this.ContentView.Content = showAndCreateView;
            }

            Button showEditFormButton = showAndCreateView.CreateButton;
            showEditFormButton.Click += showModelEditForm;
            showEditFormButton.Content += $" {tclass.Name}";

            Button showRowFormButton = showAndCreateView.ShowButton;
            showRowFormButton.Click += showModelInstances;
            showRowFormButton.Content += $" {tclass.Name}";

            Button stackButton = new Button();
            stackButton.Click += showModelOptions;
            stackButton.Content = tclass.Name;

            this.editorButtonStack.Children.Add(stackButton);
        }
    }
}

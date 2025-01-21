using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        StudentDataAccess _data = new StudentDataAccess();
        string cs = "Server=127.0.0.1;Database=school;Uid=root;Pwd=Vmware!;";
        private bool firsttime = true;

        public MainWindow()
        {
            InitializeComponent();
            createOrUpdateButton.IsEnabled= false;

        }

        private void GetButton_Click(object sender, RoutedEventArgs e)
        {
            if (InputId.Text == "")
            {
                var list = _data.GetStudents(cs);
                dataStudentGrid.ItemsSource = list;
            }
            else if (int.TryParse(InputId.Text, out int a))
            { 
                int studentId = Int32.Parse( InputId.Text.Trim());
                var student = _data.GetStudent(studentId, cs);
                var data = new List<Student>();
                data.Add(student);
                if (student == null) {
                    InputId.Text = "Studente non trovato";
                } else {
                    dataStudentGrid.ItemsSource = data;
                }
            }

        }

        private void GetAllButton_Click(object sender, RoutedEventArgs e)
        {
            var list = _data.GetStudents(cs);
            dataStudentGrid.ItemsSource = list;
        }

        private void DataStudentGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void checkboxCreate_Checked(object sender, RoutedEventArgs e)
        {
            checkboxUpdate.IsChecked = false;
            createOrUpdateButton.Content = "Create";
            createOrUpdateButton.IsEnabled = true;
            id_input_textbox.IsEnabled = false;
        }

        private void checkboxUpdate_Checked(object sender, RoutedEventArgs e)
        {
            checkboxCreate.IsChecked = false;
            createOrUpdateButton.Content = "Update";
            createOrUpdateButton.IsEnabled = true;
            id_input_textbox.IsEnabled = true;
        }

        public int? TryParseNullable(string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }

        private void createOrUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            int outId;
            int? outAge;
            string id = id_input_textbox.Text;
            string firstname = firstname_input_textbox.Text;
            string lastname = lastname_input_textbox.Text;
            string classStudent = class_input_textbox.Text ;
            string age = age_input_textbox.Text;
            if(int.TryParse(id, out outId) || (bool)checkboxCreate.IsChecked)
            {
                Student studente = new Student();
                studente.Firstname = firstname;
                studente.Lastname = lastname;
                studente.Class = classStudent;
                studente.Age = TryParseNullable(age);
                studente.Id = outId;
                if ((bool)checkboxCreate.IsChecked)
                {
                    _data.InsertStudent(studente, cs);
                }
                if ((bool)checkboxUpdate.IsChecked) 
                {
                    try {
                        _data.UpdateStudent(studente, cs);
                    } catch (Exception)
                    {
                        id_input_textbox.Text = "Studente non trovato";
                    }
                }
            } else {
                id_input_textbox.Text = "Formato id o eta' errato";
            }
            
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            id_input_textbox.Text = string.Empty;
            firstname_input_textbox.Text= string.Empty;
            lastname_input_textbox.Text=string.Empty;
            class_input_textbox.Text=string.Empty ;
            age_input_textbox.Text=string .Empty ;
        }

        private void id_input_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void firstname_input_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void lastname_input_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void class_input_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void age_input_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void InputDeleteId_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int outId;
            if (int.TryParse(InputDeleteId.Text, out outId)){
                if(!_data.DeleteStudent(outId, cs))
                {
                    InputDeleteId.Text = "Studente non trovato";
                }
                
            } else
            {
                InputDeleteId.Text = "Formato id non corretto";
            }
            
        }
    }
}
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HrApp;
using HrApp.Services;
using HrApp.Domain;
using HrApp.Entities;
using HrApp.Repositories;
using System.Threading.Tasks;
using TimeTrackerApp.Extra;
using System.Diagnostics;

namespace TimeTrackerApp
{
    /// <summary>
    /// Interaction logic for LoggerWindow.xaml
    /// </summary>
    public partial class LoggerWindow : Window
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();
        readonly Stopwatch stopWatch = new Stopwatch();

        List<TimeInfo> DataGridList = new List<TimeInfo>();
        EmployeeEntity Employee;
        List<Commit> Commits = new List<Commit>();
        List<ProjectEntity> AllProjects = new List<ProjectEntity>();
        List<ProjectEntity> LoggedProjects = new List<ProjectEntity>(); 

        string message = "";
        string currentTime = string.Empty;
        
        bool isChecked = true;

        public LoggerWindow()
        {
            InitializeComponent();
            SetUp();
        }

        private void LoadStopwatch()
        {
            //manual time input
            TimeLabel.Visibility = Visibility.Collapsed;
            HoursTextBox.Visibility = Visibility.Collapsed;
            MinuteTextBox.Visibility = Visibility.Collapsed;
            HourLabel.Visibility = Visibility.Collapsed;
            MinuteLabel.Visibility = Visibility.Collapsed;
            SaveButton.Visibility = Visibility.Collapsed;
            DataGrid.Visibility = Visibility.Collapsed;

            //stopwatch
            DescriptionLabel.Margin = new Thickness(174, 589, 0, 0);
            DescriptionTextBox.Margin = new Thickness(174, 657, 0, 0);
            ClockLabel.Visibility = Visibility.Visible;
            StartButton.Visibility = Visibility.Visible;
            SecondsLabel.Visibility = Visibility.Visible;
        }

        private void UnloadStopwatch()
        {
            //collapsing stopwatch
            ClockLabel.Visibility = Visibility.Collapsed;
            SecondsLabel.Visibility = Visibility.Collapsed;
            StartButton.Visibility = Visibility.Collapsed;

            //manual time input
            TimeLabel.Visibility = Visibility.Visible;
            HoursTextBox.Visibility = Visibility.Visible;
            MinuteTextBox.Visibility = Visibility.Visible;
            HourLabel.Visibility = Visibility.Visible;
            MinuteLabel.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Visible;
            DataGrid.Visibility = Visibility.Visible;
            DescriptionLabel.Margin = new Thickness(179, 389, 0, 0);
            DescriptionTextBox.Margin = new Thickness(179, 457, 0, 0);

        }

        private async void SetUp()
        {
            StopwatchSlider.IsChecked = true;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

            AllProjects = await GetProjects();
            List<string> NamesList = new List<string>();
            foreach (var pro in AllProjects)
            {
                NamesList.Add(pro.Name);
            }
            DropdownList.ItemsSource = NamesList;
            DropdownList.SelectedIndex = 0;

            Employee = await GetEmployee("5e20785d2bd93500011dbf6f");

        }

        /// <summary>
        /// Sign out the current user
        /// </summary>
      /*  private async void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            var accounts = await App.PublicClientApp.GetAccountsAsync();
            if (accounts.Any())
            {
                try
                {
                    await App.PublicClientApp.RemoveAsync(accounts.FirstOrDefault());

                    MainWindow mainWindow = new MainWindow();
                    //mainWindow.Show();
                    message = "User has signed-out";
                    this.Close();
                }
                catch (MsalException ex)
                {
                    ErrorLabel.Visibility = Visibility.Visible;
                    ErrorLabel.Content = $"Error signing-out user: {ex.Message}";
                }
            }
        }*/
       
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!stopWatch.IsRunning)
            {
                stopWatch.Start();
                timer.Start();
                DropdownList.IsEnabled = false;
                CommitButton.IsEnabled = false;
            }
            else
            {
                stopWatch.Stop();
                DescriptionLabel.Visibility = Visibility.Visible;
                DescriptionTextBox.Visibility = Visibility.Visible;
                DiscardButton.Visibility = Visibility.Visible;
                CommitButton.IsEnabled = true;
                DropdownList.IsEnabled = true;
            }
            StartButton.Content = stopWatch.IsRunning ? "Stop" : "Resume";
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (stopWatch.IsRunning)
            {
                TimeSpan ts = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:",
                    ts.Hours, ts.Minutes);
                var secs = String.Format("{0:00}", ts.Seconds);
                ClockLabel.Content = currentTime;
                SecondsLabel.Content = secs;
            }
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.ResultText.Text = message;
        }

        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            if(isChecked)
            {
                string description;
                if (DescriptionTextBox.Text == "")
                {
                    DisplayError("Description field is required!");
                    return;
                }
                else
                    description = DescriptionTextBox.Text;

                var projectId = DropdownList.SelectedIndex;
                var project = AllProjects[projectId];
                var time = stopWatch.Elapsed;

                LoggOne(project, time, description);
                SetFresh();
            }
            else
            {
                LoggMultiple();
                SetFresh();
            }
            
        }
        private async void LoggMultiple()
        {
            TimeTrackerService service = new TimeTrackerService()
            {
                CommitRepository = new CommitRepository(),
                ProjectRepository = new ProjectRepository(),
                EmployeeRepository = new EmployeesRepository()
            };

            await service.LogHours(LoggedProjects, Commits);
        }
        private async void LoggOne(ProjectEntity project, TimeSpan time, string description)
        {
            TimeTrackerService service = new TimeTrackerService()
            {
                CommitRepository = new CommitRepository(),
                ProjectRepository = new ProjectRepository(),
                EmployeeRepository = new EmployeesRepository()
            };

            await service.LogHours(Employee, project, time, description);
        }
        private async Task<List<ProjectEntity>> GetProjects()
        {
            ProjectRepository projectRepository = new ProjectRepository();
            var projects = await projectRepository.GetAllProjects();
            return projects;
        }
        private async Task<EmployeeEntity> GetEmployee(string id)
        {
            EmployeesRepository repo = new EmployeesRepository();
            var emp = await repo.GetEmployeeById(id);
            return emp;
        }
        private void DisplayError(string message)
        {
            ErrorLabel.Visibility = Visibility.Visible;
            ErrorLabel.Content = message;
        }
        private void SetFresh()
        {
            DescriptionTextBox.Text = "";
            StartButton.Content = "Start";
            ClockLabel.Content = "00:00:";
            SecondsLabel.Content = "00";
            stopWatch.Stop();
            stopWatch.Reset();
            timer.Stop();
            currentTime = string.Empty;
            HoursTextBox.Text = "";
            MinuteTextBox.Text = "";
            DropdownList.SelectedIndex = 0;
            DataGrid.ItemsSource = null;
            ErrorLabel.Visibility = Visibility.Collapsed;
            ErrorLabel.Content = "";
            DataGridList = new List<TimeInfo>();
            LoggedProjects = new List<ProjectEntity>();
            Commits = new List<Commit>();
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            SetFresh();
        }

        private void StopwatchSlider_Unchecked(object sender, RoutedEventArgs e)
        {
            UnloadStopwatch();
            SetFresh();
            isChecked = false;
        }
        private void StopwatchSlider_Checked(object sender, RoutedEventArgs e)
        {           
            if(!isChecked)
            {
                LoadStopwatch();
                SetFresh();
            }
            isChecked = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var h = HoursTextBox.Text;
            var m = MinuteTextBox.Text;
            if (int.TryParse(h, out int hours) && int.TryParse(m, out int minutes))
            {
                if(hours <= 0 && minutes <= 0)                 
                {
                    DisplayError("Time can not be less than 1 min!");
                    return;
                }
                else if(minutes < 0 || minutes > 59)
                {
                    DisplayError("Minutes can not be less than 1 or more than 59!");
                    return;
                }
                else if (hours < 0 || hours > 23)
                {
                    DisplayError("Hours can not be less than 1 or more than 23!");
                    return;
                }

                string description;
                if (DescriptionTextBox.Text == "")
                {
                    DisplayError("Description field is required!");
                    return;
                }
                else
                    description = DescriptionTextBox.Text;

                var time = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);

                var projectId = DropdownList.SelectedIndex;
                var project = AllProjects[projectId];


                Commit commit = new Commit(Employee, description, time.TotalHours);
                Commits.Add(commit);

                ProjectEntity entity = AllProjects[projectId];
                LoggedProjects.Add(entity);

                DataGridList.Add(new TimeInfo(project.Name, time));

                DataGrid.ItemsSource = null;
                DataGrid.ItemsSource = DataGridList;
                DescriptionTextBox.Text = "";
                HoursTextBox.Text = "";
                MinuteTextBox.Text = "";
                DropdownList.SelectedIndex = 0;
            }
        }
    }
}
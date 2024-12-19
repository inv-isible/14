using System;
using System.Collections.Generic;
using System.Linq;
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

namespace _13
{
    public partial class AgentPage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;
        public AgentPage()
        {
            InitializeComponent();
            var currentAgents = KharrasovGlazkiSaveEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgents;
            ComboType.SelectedIndex = 0;
            ComboAgentType.SelectedIndex = 0;
            UpdateAgents();
        }
        public void UpdateAgents()
        {
            var currentAgents = KharrasovGlazkiSaveEntities.GetContext().Agent.ToList();
            currentAgents = currentAgents.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())
            || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())
            || p.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(TBoxSearch.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""))).ToList();

            if (ComboType.SelectedIndex == 1)
            {
                currentAgents = currentAgents.OrderBy(p => p.Title).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentAgents = currentAgents.OrderBy(p => p.DiscInt).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.DiscInt).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentAgents = currentAgents.OrderBy(p => p.Priority).ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentAgents = currentAgents.OrderByDescending(p => p.Priority).ToList();
            }

            if (ComboAgentType.SelectedIndex == 1)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeText.ToString() == "ЗАО").ToList();
            }
            if (ComboAgentType.SelectedIndex == 2)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeText.ToString() == "МКК").ToList();
            }
            if (ComboAgentType.SelectedIndex == 3)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeText.ToString() == "МФО").ToList();
            }
            if (ComboAgentType.SelectedIndex == 4)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeText.ToString() == "ОАО").ToList();
            }
            if (ComboAgentType.SelectedIndex == 5)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeText.ToString() == "ООО").ToList();
            }
            if (ComboAgentType.SelectedIndex == 6)
            {
                currentAgents = currentAgents.Where(p => p.AgentTypeText.ToString() == "ПАО").ToList();
            }
            AgentListView.ItemsSource = currentAgents.ToList();
            AgentListView.ItemsSource = currentAgents;
            TableList = currentAgents;
            ChangePage(0, 0);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }

            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();

                AgentListView.ItemsSource = CurrentPageList;
                AgentListView.Items.Refresh();
            }
        }
        private void TBoxSerch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
        private void ComboAgentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }
        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }
        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }
        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if (Visibility == Visibility.Visible)
            {
                KharrasovGlazkiSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentListView.ItemsSource = KharrasovGlazkiSaveEntities.GetContext().Agent.ToList();
                UpdateAgents();
            }
        }
        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count > 1)
                ChangePriorityButton.Visibility = Visibility.Visible;
            else
                ChangePriorityButton.Visibility = Visibility.Hidden;
        }

        private void ChangePriorityButton_Click(object sender, RoutedEventArgs e)
        {
            int maxPriority = 0;
            foreach (Agent agent in AgentListView.SelectedItems)
            {
                if (agent.Priority > maxPriority)
                    maxPriority = agent.Priority;
            }
            SetWindow myWindow = new SetWindow(maxPriority);
            myWindow.ShowDialog();
            MessageBox.Show(myWindow.TBPriority.Text);

            if (string.IsNullOrEmpty(myWindow.TBPriority.Text))
            {
                MessageBox.Show("Изменений не произошло");
            }
            else
            {
                int newPriority = Convert.ToInt32(myWindow.TBPriority.Text);
                foreach (Agent agent in AgentListView.SelectedItems)
                {
                    agent.Priority = newPriority;
                }
                try
                {
                    KharrasovGlazkiSaveEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена!");
                    UpdateAgents();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
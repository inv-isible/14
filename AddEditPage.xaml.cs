using Microsoft.Win32;
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
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent _currentAgent = new Agent();
        private ProductSale _currentProductSale = new ProductSale();
        //private Product _currentProduct = new Product();
        private CollectionViewSource _productsView;


        public AddEditPage(Agent SelectedAgent)
        {

            InitializeComponent();
            if (SelectedAgent != null)
            {
                _currentAgent = SelectedAgent;
            }
            DataContext = _productsView;
            DataContext = _currentAgent;
            AgentTypeComboBox.ItemsSource = KharrasovGlazkiSaveEntities.GetContext().AgentType.ToList();
            AgentTypeComboBox.DisplayMemberPath = "Title"; // Отображаемые названия
            AgentTypeComboBox.SelectedValuePath = "ID";   // Идентификатор для привязки
            AgentTypeComboBox.SelectedValue = _currentAgent.AgentTypeID; // Устанавливаем начальное значение            realize.ItemsSource = KharrasovGlazkiSaveEntities.GetContext().ProductSale.ToList();

            int selectAgentID = _currentAgent.ID;
            var filtrSale = KharrasovGlazkiSaveEntities.GetContext().ProductSale.Where(sale => sale.AgentID == selectAgentID).ToList();
            Realze.ItemsSource = filtrSale;
            Realze.DisplayMemberPath = "Datacount";
            Realze.SelectedValuePath = "AgentID";
            //int selectProductID = _currentProduct.ID;

            _productsView = new CollectionViewSource();

            var products = KharrasovGlazkiSaveEntities.GetContext().Product.ToList();
            _productsView.Source = products;
            Products.ItemsSource = _productsView.View; // Привязываем View к ComboBox
            Products.DisplayMemberPath = "Title";
            Products.SelectedValuePath = "ID";
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора");

            if (ComboType == null)
                errors.AppendLine("Укажите типа агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");

            if (_currentAgent.Priority <= 0)
                errors.AppendLine("дукажите положительный приоритет агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
                errors.AppendLine("Укажите ИННН агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");

            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = _currentAgent.Phone.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) || (ph[1] == '3' && ph.Length != 12))
                    errors.AppendLine("укажите правильно телефон агента");
            }

            if (string.IsNullOrWhiteSpace(_currentAgent.Email))
                errors.AppendLine("Укажите почту агента");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentAgent.ID == 0)
                KharrasovGlazkiSaveEntities.GetContext().Agent.Add(_currentAgent);
            try
            {
                KharrasovGlazkiSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void ChangePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentAgent.Logo = myOpenFileDialog.FileName;
                Logo.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;
            var currentSaleAgent = KharrasovGlazkiSaveEntities.GetContext().ProductSale.ToList();
            currentSaleAgent = currentSaleAgent.Where(p => p.AgentID == currentAgent.ID).ToList();
            if (currentSaleAgent.Count > 0)
                MessageBox.Show("Невозможно выполнить удаление, так как существует записи на эту услугу");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        KharrasovGlazkiSaveEntities.GetContext().Agent.Remove(currentAgent);
                        KharrasovGlazkiSaveEntities.GetContext().SaveChanges();
                        Manager.MainFrame.GoBack();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void LogoImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentAgent.Logo = myOpenFileDialog.FileName;
                Logo.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(searchprod.Text)) // Проверка на пустоту TextBox
                errors.AppendLine("Укажите продукт");
            if (string.IsNullOrWhiteSpace(ProductCountTB.Text))
                errors.AppendLine("Укажите количество продуктов");
            bool isProductCountDigits = int.TryParse(ProductCountTB.Text, out int productCount); //Проверка на число
            if (!isProductCountDigits || productCount <= 0)
                errors.AppendLine("Укажите численное положительное продуктов");
            if (string.IsNullOrWhiteSpace(saleData.Text))
                errors.AppendLine("Укажите дату продажи");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            // Находим продукт по названию из TextBox
            Product selectedProduct = _productsView.View.Cast<Product>()
                .FirstOrDefault(p => p.Title.ToLower() == searchprod.Text.ToLower());

            if (selectedProduct == null)
            {
                MessageBox.Show("Товар не найден!");
                return;
            }


            _currentProductSale.AgentID = _currentAgent.ID;
            _currentProductSale.ProductID = selectedProduct.ID; // Используем ID найденного товара
            _currentProductSale.ProductCount = productCount; //Используем проверенное число
            _currentProductSale.SaleDate = Convert.ToDateTime(saleData.Text);
            if (_currentProductSale.ID == 0)
                KharrasovGlazkiSaveEntities.GetContext().ProductSale.Add(_currentProductSale);

            try
            {
                KharrasovGlazkiSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void delete_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (Realze.SelectedItem != null) // Проверка на наличие выбранного элемента
                    {
                        ProductSale selectedHistory = (ProductSale)Realze.SelectedItem; // Получаем выбранный объект
                        KharrasovGlazkiSaveEntities.GetContext().ProductSale.Remove(selectedHistory);
                        KharrasovGlazkiSaveEntities.GetContext().SaveChanges();
                        MessageBox.Show("Информация удалена!");
                        Manager.MainFrame.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите запись для удаления.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void searchprod_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = searchprod.Text.ToLower();
            var filteredProducts = _productsView.View.Cast<object>()
                .Where(o =>
                {
                    Product p = o as Product;
                    return p != null && p.Title.ToLower().Contains(searchText);
                })
                .Select(o => (o as Product).Title)
                .ToList();

            suggestionsList.ItemsSource = filteredProducts;
            suggestionsPopup.IsOpen = filteredProducts.Count > 0; // Открываем Popup, если есть результаты
        }

        private void ProductCountTB_GotFocus(object sender, RoutedEventArgs e)
        {
            ProductCountLabel.Visibility = Visibility.Collapsed;
        }

        private void ProductCountTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProductCountTB.Text))
            {
                ProductCountLabel.Visibility = Visibility.Visible;
            }
        }

        private void suggestionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suggestionsList.SelectedItem != null)
            {
                string selectedTitle = (string)suggestionsList.SelectedItem;
                searchprod.Text = selectedTitle;
                suggestionsPopup.IsOpen = false; // Закрываем Popup после выбора
            }
        }

        private void searchprod_LostFocus(object sender, RoutedEventArgs e)
        {
            suggestionsPopup.IsOpen = false; // Закрываем Popup при потере фокуса
        }
    }
}
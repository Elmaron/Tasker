using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
using System.Reflection;
using System.Text;


namespace ADHDWORKER.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            //Create or/and Load Database
            ADHDWORKER.Classes.DataBase.Initialize();
        }

        //Run, if Button "New Category is pressed
        //Create a New button and show it in the view
        public void NewCategoryHandler(object sender, RoutedEventArgs e)
        {
            var button = new Button {
                Content = category_textbox_name.Text,
            };

            button.Click += ChangeCategoryHandler;
            category_stackpanel_holder.Children.Add(button);
        }

        //Switch between categories with any press on a button with the name of a specific category
        private void ChangeCategoryHandler(object? sender, RoutedEventArgs e)
        {
            //Create Views for the button if a button is pressed
            if (sender is Button category_button)
            {
                var category_name = category_button.Content?.ToString();

                var removeTabViews = main_dockpanel.Children.Where(x => x is TabControl).ToList();
                foreach (var removeTabView in removeTabViews) main_dockpanel.Children.Remove(removeTabView);

                var tabcontrol = new TabControl
                {
                    Name = category_name,
                };
                var tabitem = new TabItem
                {
                    Header = "All",
                    Content = new TextBox { Text = category_name }
                };
                tabcontrol.Items.Add(tabitem);

                var createTabPanel = new StackPanel { };

                createTabPanel.Children.Add(new TextBox
                {
                    Name = "project_textbox_name",
                    PlaceholderText = "New Project...",
                    MaxLength = 30
                });

                createTabPanel.Children.Add(new Button
                {
                    Content = "Create"
                });

                tabitem = new TabItem
                {
                    Header = "+",
                    Content = createTabPanel
                };
                tabcontrol.Items.Add(tabitem);

                main_dockpanel.Children.Add(tabcontrol);

            }
        }
    }
}
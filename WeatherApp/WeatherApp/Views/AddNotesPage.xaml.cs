using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNotesPage : ContentPage
    {
        public AddNotesPage()
        {
            InitializeComponent();
            DatePicker.MinimumDate = DateTime.Now;
        }
    }
}
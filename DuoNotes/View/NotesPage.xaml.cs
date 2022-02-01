﻿using DuoNotes.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DuoNotes.View {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotesPage : ContentPage {
        public NotesPage() {
            InitializeComponent();
        }

        public NotesPage(string Id) {
            InitializeComponent();

            var vm = new NotesPageViewModel {
                RecivedSelectedNotebookID = Id
            };

            BindingContext = vm;
        }
    }
}
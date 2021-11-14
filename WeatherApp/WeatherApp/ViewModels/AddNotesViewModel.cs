using Contracts.RepositoryContracts.Notes;
using Managers.Notes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;

namespace WeatherApp.ViewModels
{
    public class AddNotesViewModel : BaseViewModel
    {
        readonly INotesManager NotesManager;
        readonly IPageDialogService PageDialogService;
        public AddNotesViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
            INotesManager notesManager) : base(navigationService)
        {
            NotesManager = notesManager;
            PageDialogService = pageDialogService;
        }

        private DateTime _SelectedDate;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set { SetProperty(ref _SelectedDate, value); }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        private string _Content;
        public string Content
        {
            get { return _Content; }
            set { SetProperty(ref _Content, value); }
        }

        private DelegateCommand _AddNotesCommand;
        public DelegateCommand AddNotesCommand =>
            _AddNotesCommand ?? (_AddNotesCommand = new DelegateCommand(ExecuteAddNotesCommand));

        async void ExecuteAddNotesCommand()
        {
            var result = await NotesManager.AddNote(new NotesContract()
            {
                Title = Title,
                Content = Content,
                Day = SelectedDate.Day,
                Month = SelectedDate.Month,
                Year = SelectedDate.Year
            });

            if (result)
            {
                await PageDialogService.DisplayAlertAsync(string.Empty, "Successfully Added", "OK");
                base.GoBackAsync();
            }
            else
            {
                await PageDialogService.DisplayAlertAsync(string.Empty, "Unable to add notes", "OK");
            }
        }
    }
}

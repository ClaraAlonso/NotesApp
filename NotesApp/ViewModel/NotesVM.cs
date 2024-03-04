using NotesApp.Model;
using NotesApp.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NotesApp.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        // Commands
        public NewNotebookCommand NewNotebookCommand { get; set; }
        public NewNoteCommand NewNoteCommand { get; set; }
        public EditCommand EditCommand { get; set; }
        public EndEditingCommand EndEditingCommand { get; set; }
        //public SpeechCommand SpeechCommand { get; set; }

        // Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectedNoteChanged;

        // Lista de objetos com seus observables que sao usados na listagem da view
        public ObservableCollection<Note> Notes { get; set; }
        public ObservableCollection<Notebook> Notebooks { get; set; }

        // Propriedades usadas na view
        private Notebook selectedNotebook;
        public Notebook SelectedNotebook
        {
            get { return selectedNotebook; }
            set
            {
                selectedNotebook = value;
                OnPropertyChanged("SelectedNotebook");
                ReadNotes();
            }
        }

        private Visibility isRenameNotebookVisible;
        public Visibility IsRenameNotebookVisible
        {
            get { return isRenameNotebookVisible; }
            set 
            { 
                isRenameNotebookVisible = value;
                OnPropertyChanged("IsRenameNotebookVisible");
            }
        }

        private Note selectedNote;
        public Note SelectedNote
        {
            get { return selectedNote; }
            set 
            { 
                selectedNote = value;
                OnPropertyChanged("SelectedNote");
                SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
        }


        public NotesVM()
        {
            NewNotebookCommand = new NewNotebookCommand(this);
            NewNoteCommand = new NewNoteCommand(this);
            EditCommand = new EditCommand(this);
            EndEditingCommand = new EndEditingCommand(this);
            //SpeechCommand = new SpeechCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
            Notes = new ObservableCollection<Note>();

            isRenameNotebookVisible = Visibility.Collapsed;

            ReadNotebooks();
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "New notebook"
            };

            DatabaseHelper.Insert(newNotebook);
            ReadNotebooks();
        }

        public void CreateNote(int notebookId)
        {
            Note newNote = new Note()
            {
                NotebookId = notebookId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = "New note"
            };

            DatabaseHelper.Insert(newNote);
            ReadNotes();
        }

        public void ReadNotebooks()
        {          
            var notebooks = DatabaseHelper.Read<Notebook>();

            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
            }            
        }

        public void ReadNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == SelectedNotebook.Id);

                Notes.Clear();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }

        public void StartEditing() 
        {
            IsRenameNotebookVisible = Visibility.Visible;
        }

        public void StopEditing(Notebook notebook)
        {
            IsRenameNotebookVisible = Visibility.Collapsed;
            DatabaseHelper.Update(notebook);
            ReadNotebooks();    
        }
        //public async void SpeechToText() 
        //{
        //    var teste = await SpeechHelper.SpeechToTextAzureAsync();
        //}
    }
}

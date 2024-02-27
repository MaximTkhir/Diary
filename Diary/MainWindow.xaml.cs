using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace DailyPlanner
{
    public class Note
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
    public class JsonHandler
    {
        public static void Serialize<T>(List<T> data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonData);
        }

        public static List<T> Deserialize<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(jsonData);
            }

            return new List<T>();
        }
    }
    public partial class MainWindow : Window
    {
        private List<Note> notes;
        private string filePath = "notes.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            datePicker.SelectedDate = DateTime.Today;
            ShowNotes(DateTime.Today);
        }
        private void LoadData()
        {
            notes = JsonHandler.Deserialize<Note>(filePath);
        }
        private void SaveData()
        {
            JsonHandler.Serialize(notes, filePath);
        }
        private void ShowNotes(DateTime selectedDate)
        {
            List<Note> selectedNotes = notes.FindAll(note => note.Date.Date == selectedDate.Date);
            notesList.ItemsSource = selectedNotes;
        }
        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowNotes(datePicker.SelectedDate ?? DateTime.Today);
        }
        private void notesList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Note selectedNote = (Note)notesList.SelectedItem;
            if (selectedNote != null)
            {
                titleTextBox.Text = selectedNote.Title;
                descriptionTextBox.Text = selectedNote.Description;
            }
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note
            {
                Title = titleTextBox.Text,
                Description = descriptionTextBox.Text,
                Date = datePicker.SelectedDate ?? DateTime.Today
            };

            notes.Add(newNote);
            ShowNotes(newNote.Date);
            SaveData();
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            Note selectedNote = (Note)notesList.SelectedItem;
            if (selectedNote != null)
            {
                selectedNote.Title = titleTextBox.Text;
                selectedNote.Description = descriptionTextBox.Text;
                SaveData();
            }
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Note selectedNote = (Note)notesList.SelectedItem;
            if (selectedNote != null)
            {
                notes.Remove(selectedNote);
                ShowNotes(selectedNote.Date);
                SaveData();
            }
        }
    }
}
namespace Dziennik
{
    public partial class MainPage : ContentPage
    {
        private readonly string folderPath = "C:\\Aleks";

        public MainPage()
        {
            InitializeComponent();
            CheckAndCreateFolder();
            LoadTextFiles();
        }

        private void CheckAndCreateFolder()
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private void LoadTextFiles()
        {

            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

            foreach (string filePath in txtFiles)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                Button fileButton = new Button
                {
                    Text = fileNameWithoutExtension,
                    Margin = new Thickness(5)
                };
                fileButton.Clicked += (sender, e) =>
                {
                    OpenClass(filePath);
                };

                fileStackLayout.Children.Add(fileButton);
            }
        }
        public async void OnNewClass(object sender, EventArgs e)
        {
            string className = await DisplayPromptAsync("Nowa klasa", "Wpisz nazwę klasy: ", "Dodaj", "Anuluj");

            if (className != null)
            {
                string driveletter = "C:\\Aleks";
                string filename = className + ".txt";
                string filePath = Path.Combine(driveletter, filename);
                DisplayAlert("Utworzono klasę ", $"{filename}", "OK");
                File.WriteAllText(filePath, "");
            }
        }

        public void OnReloadClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private async void OpenClass(string filePath)
        {
            string text = File.ReadAllText(filePath);

            string selectedOption = await DisplayActionSheet("Wybierz Działanie", "Anuluj", null, "Wyświetl klasę", "Edytuj klasę","Losuj ucznia", "Usuń klasę");

            switch (selectedOption)
            {
                case "Wyświetl klasę":
                    DisplayAlert("Lista uczniow", $"Uczniowie klasy '{Path.GetFileNameWithoutExtension(filePath)}':\n{text}", "OK");
                    break;
                case "Edytuj klasę":
                    await Navigation.PushAsync(new NewPage1(filePath));
                    break;
                case "Losuj ucznia":
                    ShowRandomStudent(filePath);
                    break;
                case "Usuń klasę":
                    Usun(filePath);

                    break;
                default:
                    break;
            }
        }
        private void Usun(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                DisplayAlert("Komunikat", "Plik został usunięty pomyślnie!", "OK");
            }
            else
            {
                DisplayAlert("ERROR!", "Żądany plik nie został znaleziony!", "OK");
            }
        }
        private async void ShowRandomStudent(string filePath)
        {
            List<string> lines = File.ReadAllLines(filePath).ToList();

            if (lines.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(0, lines.Count);
                string randomStudent = lines[randomIndex];

                await DisplayAlert("Uczeń do pytania", randomStudent, "OK");
            }
            else
            {
                await DisplayAlert("Brak uczniów", "W klasie nie ma uczniów do losowania!", "OK");
            }
        }
    }
}
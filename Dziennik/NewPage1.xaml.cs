namespace Dziennik
{
    public partial class NewPage1 : ContentPage
    {
        public string _filePath;
        public int studentIdCounter = 0;

        public NewPage1(string filePath)
        {
            InitializeComponent();
            _filePath = filePath;

            List<string> lines = ReadTextFile(filePath);

            foreach (string line in lines)
            {
                Button button = new Button
                {
                    Text = line,
                    Margin = new Thickness(10),
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                button.Clicked += (sender, e) =>
                {
                    OpenStudent(filePath);
                    OpenStudent(line);
                };
                stackLayout.Children.Add(button);
            }
        
        }

        

        private List<Student> _students = new List<Student>();

        public class Student
        {
            public int LineNumber { get; set; }
            public string Name { get; set; }
        }

        public async void OpenStudent(string studentName)
        {
            try
            {
                string selectedOption = await DisplayActionSheet("Wybierz Dzialanie", "Anuluj", null, "Usun ucznia", "Zaznacz obecnosc");

                switch (selectedOption)
                {
                    case "Usun ucznia":
                        DeleteStudent(studentName);
                        break;
                    case "Zaznacz obecnosc":
                        string[] options = { "Obecny", "Nieobecny" };
                        string selectedPresence = await DisplayActionSheet("Zaznacz obecnosc ucznia", null, null, options);

                        if (!string.IsNullOrEmpty(selectedPresence))
                        {
                            
                            if (selectedPresence == "Obecny")
                            {
                                
                            }
                            else if (selectedPresence == "Nieobecny")
                            {
                             
                            }
                            await DisplayAlert("Komunikat", "Obecnosc ucznia zostala zaznaczona", "OK");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("B??d", $"Opis b??du: {ex.Message}", "OK");
            }
        }

        public void DeleteStudent(string studentName)
        {
            List<string> lines = File.ReadAllLines(_filePath).ToList();

            int index = lines.FindIndex(line => line.Contains(studentName));

            if (index != -1)
            {
                lines.RemoveAt(index);
                File.WriteAllLines(_filePath, lines);

                DisplayAlert("Komunikat", "Ucze? zosta? pomy?lnie usuni?ty", "OK");
            }
            else
            {
                DisplayAlert("Komunikat", "Nie mo?na odnale?? ucznia", "OK");
            }
        }

        private List<string> ReadTextFile(string filePath)
        {
            List<string> lines = new List<string>();
            if (File.Exists(filePath))
            {
                lines.AddRange(File.ReadAllLines(filePath));
            }
            return lines;
        }

        public async void OnNewStudent(object sender, EventArgs e)
        {
            try
            {
                string entry = await DisplayPromptAsync("Nowy Ucze?", "Wpisz imi? i nazwisko:", "OK", "Cancel");

                if (!string.IsNullOrEmpty(entry))
                {
                    string[] lines = File.ReadAllLines(_filePath);

                    int lineNumber = 1;
                    while (lines.Any(line => line.StartsWith($"{lineNumber}.")))
                    {
                        lineNumber++;
                    }

                    string newStudentLine = $"{lineNumber}. {entry}";
                    File.AppendAllText(_filePath, newStudentLine + Environment.NewLine);

                    await DisplayAlert("Komunikat", "Dodano nowego ucznia", "OK");
                }
                else
                {
                    await DisplayAlert("B??d", "Niepoprawna nazwa ucznia!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("B??d", $"Opis b??du: {ex.Message}", "OK");
            }
        }

        public void Wroc(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace US_Armed_Forces_Training
{
    public class TaskItem
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string ImageFile { get; set; }
    }

    public class ControlBlankItem
    {
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class ControlMatchingItem
    {
        public string LeftText { get; set; }
        public string RightText { get; set; }
        public string CorrectLetter { get; set; }
    }

    public class ControlEndingItem
    {
        public string SentenceText { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class MultiQuestionTask
    {
        public string Title { get; set; }
        public List<QuestionItem> Questions { get; set; }
    }

    public class QuestionItem
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class IntroSlide
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
    }

    public class TitleSlide
    {
        public string Title { get; set; }
        public string TrainingRules { get; set; }
        public string ControlRules { get; set; }
        public string ImageFile { get; set; }
    }

    public class ControlIntroSlide
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Rules { get; set; }
        public string ImageFile { get; set; }
    }

    public class DragDropSignTask
    {
        public string Title { get; set; }
        public List<DragSignItem> LeftItems { get; set; }
        public List<DragSignItem> RightItems { get; set; }
    }

    public class DragSignItem
    {
        public string ImageFile { get; set; }
        public string CorrectName { get; set; }
        public string DisplayName { get; set; }
        public int TargetIndex { get; set; } = -1;
    }

    public class ControlImagesTask
    {
        public string Title { get; set; }
        public List<string> ImageFiles { get; set; }
        public List<string> CorrectAnswers { get; set; }
        public List<string> Questions { get; set; }
    }

    public partial class MainWindow : Window
    {
        private List<object> allTasks = new List<object>();
        private List<object> controlTasks = new List<object>();
        private int currentIndex = 0;
        private int correctCount = 0;
        private bool isControlMode = false;
        private Dictionary<int, bool> controlResults = new Dictionary<int, bool>();
        private Dictionary<int, string> userAnswers = new Dictionary<int, string>();
        private List<TextBox> controlTextBoxes = new List<TextBox>();
        private List<ComboBox> controlComboBoxes = new List<ComboBox>();

        public MainWindow()
        {
            InitializeComponent();
            LoadAllTasks();
            LoadControlTasks();
            ShowCurrentTask();
        }

        private void LoadIntroSlide()
        {
            var intro = new IntroSlide
            {
                Title = "ВООРУЖЕННЫЕ СИЛЫ США",
                Subtitle = "US ARMED FORCES",
                Description = "The army warfighting functions\n\nАнглийский язык",
                ImageFile = "i.jpg"
            };
            allTasks.Add(intro);
        }

        private void LoadTitleSlide()
        {
            var titleSlide = new TitleSlide
            {
                Title = "ТЕМА: ВООРУЖЕННЫЕ СИЛЫ США",
                TrainingRules = "ОБУЧЕНИЕ:\n• Можно менять вариант ответа до тех пор, пока не найдёте правильный\n• Можно листать задания назад и вперёд\n• После правильного ответа — автоматический переход",
                ControlRules = "КОНТРОЛЬ:\n• Перебор ответов исключён\n• Листать назад нельзя\n• Результат показывается только в конце",
                ImageFile = "i.jpg"
            };
            allTasks.Add(titleSlide);
        }

        private void LoadAllTasks()
        {
            allTasks = new List<object>();

            LoadIntroSlide();
            LoadTitleSlide();

            // ЗАДАНИЕ 1: 7 картинок
            string[] pics1 = { "soldier", "sailor", "airman", "police_oficer", "marine", "engineer", "doctor" };
            string[] answers1 = { "Soldier", "Sailor", "Airman", "Police_officer", "Mariners", "Engineer", "Doctor" };
            string[] questions1 = {
                "Tao's in the army. He's a ________.",
                "Jane is in the navy. She's a ________.",
                "Tim is in the air force. He's an ________.",
                "Ann is in the police force. She is a ________.",
                "Max and John are in the marine corps. They are ________.",
                "David is in the corps of engineers. He is an ________.",
                "Mary is in the medical service corps. She is a ________."
            };
            var wordOptions = new List<string> { "Airman", "Doctor", "Engineer", "Police officer", "Sailor", "Soldier", "Mariners" };
            for (int i = 0; i < 7; i++)
                allTasks.Add(new TaskItem { Question = questions1[i], Options = wordOptions, CorrectAnswer = answers1[i], ImageFile = $"{pics1[i]}.jpg" });

            // ЗАДАНИЕ 2: тактические знаки (выпадающий список) — 10 заданий
            string[] allSigns = {
                "Infantry", "Armor", "field_artillery", "air_defense", "signal_corps",
                "engineers", "military_intelligence", "medical_corps", "military_police", "marine_corps"
            };
            var signOptions = new List<string>(allSigns);
            for (int i = 0; i < 10; i++)
            {
                allTasks.Add(new TaskItem
                {
                    Question = "Как называется этот тактический знак?",
                    Options = signOptions,
                    CorrectAnswer = allSigns[i],
                    ImageFile = $"{allSigns[i].ToLower().Replace(" ", "_")}.png"
                });
            }

            // ЗАДАНИЕ 3: 4 правильные формы глагола
            var verbOptions = new List<string> { "includes", "have", "are made up of" ,"consists of"};
            string[] q3 = {
                "The US Armed Forces ________ 5 armed services.",
                "The US Army ________ basic arms.",
                "Combat support arms ________ the Corps of Engineers, the Signal Corps.",
                "Services ________ the mission to provide administrative support."
            };
            string[] a3 = { "includes", "consists of", "are made up of", "have" };
            for (int i = 0; i < 4; i++)
                allTasks.Add(new TaskItem { Question = q3[i], Options = verbOptions, CorrectAnswer = a3[i] });

            // ЗАДАНИЕ 4: Drag & Drop для предложений
            var dragDropTask = new DragDropSignTask
            {
                Title = "Перетащите правильное слово в предложение:",
                LeftItems = new List<DragSignItem>
                {
                    new DragSignItem { DisplayName ="combat and combat support", CorrectName = "combat and combat support" },
                    new DragSignItem { DisplayName = "services", CorrectName = "services" },
                    new DragSignItem { DisplayName =  "commander-in-chief", CorrectName = "commander-in-chief" },
                    new DragSignItem { DisplayName = "striking force", CorrectName = "striking force" }
                },
                RightItems = new List<DragSignItem>
                {
                    new DragSignItem { DisplayName = "a) The President of the USA is the ________.", CorrectName = "commander-in-chief" },
                    new DragSignItem { DisplayName = "b) The Armed Forces is the ________.", CorrectName = "striking force" },
                    new DragSignItem { DisplayName = "c) The main msn of cbt arms is ________.", CorrectName = "combat and combat support" },
                    new DragSignItem { DisplayName = "d) The Ord C, the Cml C, the TC, the FC, the MI and the MSC are ________.", CorrectName = "services" }
                }
            };
            allTasks.Add(dragDropTask);

            // ЗАДАНИЕ 5: 6 предложений
            var missionOptions = new List<string> {
                "signal corps" , "corps of engineers", "medical service corps",
                "artillery", "transportation corps","military intelligence" 
            };
            string[] q5 = {
                "1. The mission of the ________ is to collect information about the enemy.",
                "2. The mission of the ________ is to build roads and bridges.",
                "3. The mission of the ________ is to operate the Army's communication systems.",
                "4. The mission of the ________ is to provide medical support.",
                "5. The mission of the ________ is to transport troops, equipment and supplies.",
                "6. The mission of the ________ is to provide fire support."
            };
            string[] a5 = {
                "military intelligence", "corps of engineers", "signal corps",
                "medical service corps", "transportation corps", "artillery"
            };
            for (int i = 0; i < 6; i++)
                allTasks.Add(new TaskItem { Question = q5[i], Options = missionOptions, CorrectAnswer = a5[i] });

            // ЗАДАНИЕ 6: 8 определений
            var defOptions = new List<string> { "infantry", "arms", "army", "military police corps", "regular army", "finance corps", "medical service corps", "military intelligence" };
            string[] q6 = {
                "Permanent army that is used both in peace and in war.",
                "The service that provides medical care for every soldier and his family.",
                "The main mission of this branch is the collection and analysis of intelligence.",
                "This branch performs police duty for the Army.",
                "The chief duty of this branch is to administer pay and account for funds.",
                "The general word for Infantry, Air Defense Artillery, Field Artillery, and Armor.",
                "The land military forces of a nation.",
                "Branch of the Army, trained, equipped and organized to fight on foot."
            };
            string[] a6 = { "regular army", "medical service corps", "military intelligence", "military police corps", "finance corps", "arms", "army", "infantry" };
            for (int i = 0; i < 8; i++)
                allTasks.Add(new TaskItem { Question = q6[i], Options = defOptions, CorrectAnswer = a6[i] });
        }

        private void LoadControlTasks()
        {
            controlTasks = new List<object>();

            // Вводный слайд для контрольной части
            var controlIntro = new ControlIntroSlide
            {
                Title = "КОНТРОЛЬНАЯ ЧАСТЬ",
                Description = "Всего заданий: 6",
                Rules = "ПРАВИЛА ВЫПОЛНЕНИЯ:\n\n• Перебор ответов исключён\n• Нельзя возвращаться к предыдущим заданиям\n• Результат будет показан в конце\n• Будьте внимательны!",
                ImageFile = "control_intro.jpg"
            };
            controlTasks.Add(controlIntro);

            // ЗАДАНИЕ 7: 8 предложений is/are
            var blanks = new List<ControlBlankItem>
            {
                new ControlBlankItem { Text = "1. US Armed forces _________ a striking force of NATO block.", CorrectAnswer = "is" },
                new ControlBlankItem { Text = "2. The CINC of the US Armed Forces _________ the President.", CorrectAnswer = "is" },
                new ControlBlankItem { Text = "3. Main Armed forces _________ Army, Navy, Air Force.", CorrectAnswer = "are" },
                new ControlBlankItem { Text = "4. Arms _________ Inf, Arty, Armor.", CorrectAnswer = "are" },
                new ControlBlankItem { Text = "5. Services _________ Ord C, Cml C, MSC, TC, FC, MI.", CorrectAnswer = "are" },
                new ControlBlankItem { Text = "6. Special arms _________ CE, Sig C, MPC.", CorrectAnswer = "are" },
                new ControlBlankItem { Text = "7. The main msn of arms _________ cbt and cbt spt.", CorrectAnswer = "is" },
                new ControlBlankItem { Text = "8. The main msn of services _________ cbt svc spt and admin in the field.", CorrectAnswer = "is" }
            };
            controlTasks.Add(blanks);

            // ЗАДАНИЕ 8: 6 пар букв
            var matching = new List<ControlMatchingItem>
            {

                new ControlMatchingItem { LeftText = "1. The US Armed Forces are under control of", RightText = "a) both arms and services.", CorrectLetter = "b" },
                new ControlMatchingItem { LeftText = "2. There are about", RightText = "b) the Department of defense.", CorrectLetter = "d" },
                new ControlMatchingItem { LeftText = "3. The Corps of Engineers, the Signal Corps and Military police Corps are referred to", RightText = "c) Army, Navy and Air Force.", CorrectLetter = "a" },
                new ControlMatchingItem { LeftText = "4. The main US Armed Forces are", RightText = "d) twenty arms and services in the US Army.", CorrectLetter = "e" },
                new ControlMatchingItem { LeftText = "5. The main mission of arms is", RightText = "e) combat and combat support.", CorrectLetter = "f" },
                new ControlMatchingItem { LeftText = "6. The 'Armed Forces of the United States' means", RightText = "f) Army, Navy, Air Force, Marine Corps and Coast Guard.", CorrectLetter = "c" }
            };
            controlTasks.Add(matching);

            // ЗАДАНИЕ 9: 4 окончания
            // ЗАДАНИЕ 9: 4 окончания
            var endings = new List<ControlEndingItem>
{
    new ControlEndingItem { SentenceText = "1. The mission of the engineers is _________", CorrectAnswer = "c) to operate the communication systems" },
    new ControlEndingItem { SentenceText = "2. The mission of the intelligence corps is _________", CorrectAnswer = "d) to provide medical support" },
    new ControlEndingItem { SentenceText = "3. The mission of the medical corps is _________", CorrectAnswer = "a) to build roads and bridges" },
    new ControlEndingItem { SentenceText = "4. The mission of the signals unit is _________", CorrectAnswer = "b) to collect the information about the enemy" }
};
            controlTasks.Add(endings);

            // ЗАДАНИЕ 10: перевод (ввод цифры)
            var translation = new List<ControlBlankItem>
            {
                new ControlBlankItem { Text = "squad → 1) взвод  2) отделение  3) рота", CorrectAnswer = "2" },
                new ControlBlankItem { Text = "platoon → 1) взвод  2) батальон  3) бригада", CorrectAnswer = "1" },
                new ControlBlankItem { Text = "company → 1) полк  2) дивизия  3) рота", CorrectAnswer = "3" },
                new ControlBlankItem { Text = "battalion → 1) батальон  2) армейский корпус  3) группа армий", CorrectAnswer = "1" },
                new ControlBlankItem { Text = "regiment → 1) бригада  2) полк  3) отделение", CorrectAnswer = "2" },
                new ControlBlankItem { Text = "brigade → 1) рота  2) батальон  3) бригада", CorrectAnswer = "3" },
                new ControlBlankItem { Text = "division → 1) дивизия  2) взвод  3) полк", CorrectAnswer = "1" },
                new ControlBlankItem { Text = "Army Corps → 1) армия  2) армейский корпус  3) группа армий", CorrectAnswer = "2" },
                new ControlBlankItem { Text = "Field Army → 1) армейский корпус  2) (общевойсковая) армия  3) дивизия", CorrectAnswer = "2" },
                new ControlBlankItem { Text = "Army Group → 1) группа армий  2) полк  3) батальон", CorrectAnswer = "1" }
            };
            controlTasks.Add(translation);

            // ЗАДАНИЕ 11: 5 картинок (сокращения)
            var imagesTask = new ControlImagesTask
            {
                Title = "Введите правильное сокращение для каждого подразделения:",
                ImageFiles = new List<string>
    {
        "picture1.jpg",
        "picture2.jpg",
        "picture3.jpg",
        "picture4.jpg",
        "picture5.jpg"
    },
                Questions = new List<string>
    {
        "",
        "",
        "",
        "",
        ""
    },
                CorrectAnswers = new List<string>
    {
        "Sqd",
        "Sec",
        "Plat",
        "Co",
        "Bn"
    }
            };
            controlTasks.Add(imagesTask);

            // ЗАДАНИЕ 12: 6 вопросов на одном слайде
            var multiTask = new MultiQuestionTask
            {
                Title = "Выберите правильный ответ:",
                Questions = new List<QuestionItem>
                {
                    new QuestionItem { Text = "1. What is the smallest and basic military unit?", Options = new List<string> { "a) platoon", "b) squad", "c) section" }, CorrectAnswer = "b) squad" },
                    new QuestionItem { Text = "2. What are small units?", Options = new List<string> { "a) regiment, company, battalion", "b) battery, platoon, brigade", "c) squad, section, troop" }, CorrectAnswer = "c) squad, section, troop" },
                    new QuestionItem { Text = "3. What does 'arm' mean?", Options = new List<string> { "a) рота", "b) род войск", "c) полк" }, CorrectAnswer = "b) род войск" },
                    new QuestionItem { Text = "4. What units form a battalion?", Options = new List<string> { "a) companies or army groups", "b) batteries or companies", "c) brigades or companies?" }, CorrectAnswer = "b) batteries or companies" },
                    new QuestionItem { Text = "5. What are large units of the US Army?", Options = new List<string> { "a) division, AC, FA, Army group", "b) AC, FA, battalion, regiment", "c) brigade, division, Army group" }, CorrectAnswer = "a) division, AC, FA, Army group" },
                    new QuestionItem { Text = "6. Whom are squads, sections and platoons commanded by?", Options = new List<string> { "a) general", "b) leader", "c) commander" }, CorrectAnswer = "b) leader" }
                }
            };
            controlTasks.Add(multiTask);
        }

        private void ShowCurrentTask()
        {
            var tasks = isControlMode ? controlTasks : allTasks;

            if (currentIndex >= tasks.Count)
            {
                if (isControlMode)
                {
                    int total = controlResults.Count(r => r.Value);
                    StatusText.Text = $"КОНТРОЛЬ ЗАВЕРШЁН! Результат: {total} из {tasks.Count}";
                    ShowControlResultTable();
                }
                else
                    StatusText.Text = $"ОБУЧЕНИЕ ЗАВЕРШЕНО! Результат: {correctCount} из {tasks.Count}";
                StatusBorder.Background = Brushes.Green;
                CheckButton.Visibility = Visibility.Collapsed;
                ControlNextBtn.Visibility = Visibility.Collapsed;
                return;
            }

            NormalPanel.Visibility = Visibility.Collapsed;
            TextInputPanel.Visibility = Visibility.Collapsed;
            ControlPanel.Visibility = Visibility.Collapsed;
            ImageArea.Visibility = Visibility.Collapsed;
            ControlNextBtn.Visibility = Visibility.Collapsed;

            int introSlidesCount = 0;
            foreach (var t in allTasks)
            {
                if (t is IntroSlide || t is TitleSlide)
                    introSlidesCount++;
                else
                    break;
            }

            var currentTask = tasks[currentIndex];

            // НОМЕР ЗАДАНИЯ
            if (isControlMode)
            {
                if (currentTask is ControlIntroSlide)
                    TaskNumberText.Text = "ВСТУПЛЕНИЕ";
                else
                    TaskNumberText.Text = $"Задание {currentIndex} из {controlTasks.Count - 1}";
            }
            else
            {
                if (currentTask is IntroSlide || currentTask is TitleSlide)
                    TaskNumberText.Text = "ВСТУПЛЕНИЕ";
                else
                    TaskNumberText.Text = $"Задание {currentIndex - introSlidesCount + 1} из {allTasks.Count - introSlidesCount}";
            }

            // СЧЁТЧИК РЕЗУЛЬТАТОВ
            if (isControlMode)
            {
                int totalReal = controlTasks.Count - 1;
                ScoreText.Text = $"{controlResults.Count}/{totalReal}";
            }
            else
                ScoreText.Text = $"{correctCount}/{tasks.Count}";

            // ВВОДНЫЙ СЛАЙД ОБУЧЕНИЯ
            if (currentTask is IntroSlide intro)
            {
                NormalPanel.Visibility = Visibility.Collapsed;
                TextInputPanel.Visibility = Visibility.Collapsed;
                ControlPanel.Visibility = Visibility.Collapsed;
                ImageArea.Visibility = Visibility.Visible;
                ControlNextBtn.Visibility = Visibility.Collapsed;
                QuestionText.Text = intro.Title + "\n\n" + intro.Subtitle + "\n\n" + intro.Description;
                ShowImage(intro.ImageFile);
                CheckButton.Visibility = Visibility.Collapsed;
                PrevButton.IsEnabled = false;
                NextButton.IsEnabled = true;
                StatusText.Text = "Нажмите 'ВПЕРЁД' для продолжения";
                StatusBorder.Background = Brushes.LightGray;
                return;
            }

            // СЛАЙД С ПРАВИЛАМИ ОБУЧЕНИЯ
            if (currentTask is TitleSlide titleSlide)
            {
                NormalPanel.Visibility = Visibility.Collapsed;
                TextInputPanel.Visibility = Visibility.Collapsed;
                ControlPanel.Visibility = Visibility.Collapsed;
                ImageArea.Visibility = Visibility.Visible;
                ControlNextBtn.Visibility = Visibility.Collapsed;
                QuestionText.Text = $"{titleSlide.Title}\n\n{titleSlide.TrainingRules}\n\n{titleSlide.ControlRules}";
                ShowImage(titleSlide.ImageFile);
                CheckButton.Visibility = Visibility.Collapsed;
                PrevButton.IsEnabled = true;
                NextButton.IsEnabled = true;
                StatusText.Text = "Ознакомьтесь с правилами и нажмите 'ВПЕРЁД'";
                StatusBorder.Background = Brushes.LightGray;
                return;
            }

            // КОНТРОЛЬНАЯ ЧАСТЬ
            if (isControlMode)
            {
                ControlPanel.Visibility = Visibility.Visible;
                ControlContainer.Children.Clear();
                controlTextBoxes.Clear();
                controlComboBoxes.Clear();
                ControlNextBtn.Visibility = Visibility.Visible;

                // Вводный слайд контроля
                if (currentTask is ControlIntroSlide introSlide)
                {
                    ControlTitle.Text = introSlide.Title;

                    var mainPanel = new StackPanel();

                    var descBlock = new TextBlock
                    {
                        Text = introSlide.Description,
                        FontSize = 16,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(0, 0, 0, 15),
                        Foreground = Brushes.DarkBlue
                    };
                    mainPanel.Children.Add(descBlock);

                    var rulesBlock = new TextBlock
                    {
                        Text = introSlide.Rules,
                        FontSize = 14,
                        Margin = new Thickness(0, 0, 0, 20),
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = Brushes.DarkRed
                    };
                    mainPanel.Children.Add(rulesBlock);

                    var image = new Image { Width = 260, Height = 200, Stretch = Stretch.Uniform, Margin = new Thickness(0, 10, 0, 10) };
                    try { image.Source = new BitmapImage(new Uri($"Images/{introSlide.ImageFile}", UriKind.Relative)); }
                    catch { image.Source = null; }
                    mainPanel.Children.Add(image);

                    var instructionBlock = new TextBlock
                    {
                        Text = "Нажмите 'ДАЛЕЕ' для начала контрольной работы",
                        FontSize = 12,
                        FontStyle = FontStyles.Italic,
                        Margin = new Thickness(0, 20, 0, 0),
                        Foreground = Brushes.Gray,
                        TextAlignment = TextAlignment.Center
                    };
                    mainPanel.Children.Add(instructionBlock);

                    ControlContainer.Children.Add(mainPanel);
                    CheckButton.Visibility = Visibility.Collapsed;
                    return;
                }
                // НОВОЕ ЗАДАНИЕ С КАРТИНКАМИ
                if (currentTask is ControlImagesTask imagesTask)
                {
                    ControlTitle.Text = imagesTask.Title;
                    ControlContainer.Children.Clear();
                    controlTextBoxes.Clear();

                    // Создаём ScrollViewer для возможности прокрутки, если нужно
                    var scrollViewer = new ScrollViewer
                    {
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
                    };

                    var mainPanel = new StackPanel();

                    // Создаём Grid для 5 картинок
                    var imagesGrid = new Grid();
                    for (int i = 0; i < 5; i++)
                    {
                        imagesGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    }
                    imagesGrid.Margin = new Thickness(0, 0, 0, 20);
                    imagesGrid.Height = 120;

                    // Добавляем картинки
                    for (int i = 0; i < imagesTask.ImageFiles.Count; i++)
                    {
                        var border = new Border
                        {
                            Background = new SolidColorBrush(Color.FromRgb(46, 92, 46)),
                            Margin = new Thickness(3),
                            Padding = new Thickness(3),
                            CornerRadius = new CornerRadius(5)
                        };

                        var image = new Image
                        {
                            Stretch = Stretch.Uniform,
                            Margin = new Thickness(2)
                        };

                        try
                        {
                            string imagePath = System.IO.Path.Combine("Images", imagesTask.ImageFiles[i]);
                            if (System.IO.File.Exists(imagePath))
                                image.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                            else
                                image.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/{imagesTask.ImageFiles[i]}", UriKind.RelativeOrAbsolute));
                        }
                        catch
                        {
                            // Если картинка не найдена, показываем текст-заглушку
                            image.Source = null;
                            image.Width = 80;
                            image.Height = 80;
                        }

                        border.Child = image;
                        Grid.SetColumn(border, i);
                        imagesGrid.Children.Add(border);
                    }
                    mainPanel.Children.Add(imagesGrid);

                    // Создаём сетку для полей ввода под картинками
                    var answersGrid = new Grid();
                    for (int i = 0; i < 5; i++)
                    {
                        answersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    }

                    for (int i = 0; i < imagesTask.Questions.Count; i++)
                    {
                        // Используем Border для создания рамки вместо StackPanel с CornerRadius
                        var border = new Border
                        {
                            Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                            BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(5),
                            Padding = new Thickness(5)
                        };

                        var stack = new StackPanel();

                        var label = new TextBlock
                        {
                            Text = imagesTask.Questions[i],
                            FontSize = 10,
                            FontWeight = FontWeights.Bold,
                            TextAlignment = TextAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 5),
                            Foreground = Brushes.Black
                        };
                        stack.Children.Add(label);

                        var textBox = new TextBox
                        {
                            Width = 80,
                            Height = 25,
                            TextAlignment = TextAlignment.Center,
                            Margin = new Thickness(0, 0, 0, 5),
                            HorizontalAlignment = HorizontalAlignment.Center
                        };
                        stack.Children.Add(textBox);
                        controlTextBoxes.Add(textBox);

                        border.Child = stack;
                        Grid.SetColumn(border, i);
                        answersGrid.Children.Add(border);
                    }
                    mainPanel.Children.Add(answersGrid);

                    scrollViewer.Content = mainPanel;
                    ControlContainer.Children.Add(scrollViewer);

                    PrevButton.IsEnabled = false;
                    NextButton.IsEnabled = false;
                    CheckButton.Visibility = Visibility.Collapsed;
                    return;
                }

                // Задания контроля
                if (currentTask is List<ControlBlankItem> blanks)
                {
                    if (blanks.Count == 8)
                        ControlTitle.Text = "Вставьте is / are:";
                    else if (blanks.Count == 10)
                        ControlTitle.Text = "Введите цифру правильного варианта ответа:";
                    else if (blanks.Count == 5)
                        ControlTitle.Text = "Введите правильное сокращение:";
                    else
                        ControlTitle.Text = "Введите ответ:";

                    foreach (var item in blanks)
                    {
                        var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                        stack.Children.Add(new TextBlock { Text = item.Text, Width = 450 });
                        var box = new TextBox { Width = 80, Height = 25 };
                        stack.Children.Add(box);
                        ControlContainer.Children.Add(stack);
                        controlTextBoxes.Add(box);
                    }
                }
                else if (currentTask is List<ControlMatchingItem> match)
                {
                    ControlTitle.Text = "Введите правильную букву (a-f):";
                    foreach (var item in match)
                    {
                        var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                        stack.Children.Add(new TextBlock { Text = item.LeftText, Width = 350 });
                        stack.Children.Add(new TextBlock { Text = "→", Width = 30 });
                        stack.Children.Add(new TextBlock { Text = item.RightText, Width = 200 });
                        var box = new TextBox { Width = 50, Height = 25 };
                        stack.Children.Add(box);
                        ControlContainer.Children.Add(stack);
                        controlTextBoxes.Add(box);
                    }
                }
                else if (currentTask is List<ControlEndingItem> ends)
                {
                    ControlTitle.Text = "Выберите правильное окончание:";
                    var opts = new List<string> { "a) to build roads and bridges", "b) to collect the information about the enemy", "c) to operate the communication systems", "d) to provide medical support" };
                    foreach (var item in ends)
                    {
                        var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                        stack.Children.Add(new TextBlock { Text = item.SentenceText, Width = 400 });
                        var combo = new ComboBox { Width = 250 };
                        combo.Items.Add(new ComboBoxItem { Content = "-- Выберите --", IsSelected = true });
                        foreach (var opt in opts) combo.Items.Add(new ComboBoxItem { Content = opt });
                        stack.Children.Add(combo);
                        ControlContainer.Children.Add(stack);
                        controlComboBoxes.Add(combo);
                    }
                }
                else if (currentTask is MultiQuestionTask multiTask)
                {
                    ControlTitle.Text = multiTask.Title;
                    foreach (var q in multiTask.Questions)
                    {
                        var stack = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 10) };
                        stack.Children.Add(new TextBlock { Text = q.Text, Width = 400, VerticalAlignment = VerticalAlignment.Center });
                        var combo = new ComboBox { Width = 200, Margin = new Thickness(10, 0, 0, 0) };
                        combo.Items.Add(new ComboBoxItem { Content = "-- Выберите ответ --", IsSelected = true });
                        foreach (var opt in q.Options) combo.Items.Add(new ComboBoxItem { Content = opt });
                        combo.Tag = q;
                        stack.Children.Add(combo);
                        ControlContainer.Children.Add(stack);
                        controlComboBoxes.Add(combo);
                    }
                }

                PrevButton.IsEnabled = false;
                NextButton.IsEnabled = false;
                CheckButton.Visibility = Visibility.Collapsed;
                return;
            }

            // DRAG & DROP (ЗАДАНИЕ 4 ОБУЧЕНИЯ)
            if (currentTask is DragDropSignTask dragTask)
            {
                ControlPanel.Visibility = Visibility.Visible;
                ControlContainer.Children.Clear();
                ControlNextBtn.Visibility = Visibility.Visible;
                CheckButton.Visibility = Visibility.Collapsed;
                ImageArea.Visibility = Visibility.Collapsed;

                // Убираем лишний текст
                QuestionText.Text = "";

                var mainPanel = new StackPanel();
                mainPanel.Children.Add(new TextBlock { Text = dragTask.Title, FontSize = 14, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 15) });

                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

                var leftPanel = new StackPanel();
                foreach (var item in dragTask.LeftItems)
                {
                    var border = new Border
                    {
                        Background = Brushes.LightBlue,
                        BorderBrush = Brushes.Navy,
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(5),
                        Margin = new Thickness(5),
                        Padding = new Thickness(10, 5, 10, 5),
                        Width = 180,
                        Height = 40,
                        Cursor = Cursors.Hand
                    };
                    border.Child = new TextBlock { Text = item.DisplayName, FontSize = 12, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
                    border.MouseLeftButtonDown += (s, e) => DragDrop.DoDragDrop(border, item.CorrectName, DragDropEffects.Move);
                    leftPanel.Children.Add(border);
                }
                Grid.SetColumn(leftPanel, 0);
                grid.Children.Add(leftPanel);

                var arrowBlock = new TextBlock
                {
                    Text = "→",
                    FontSize = 30,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetColumn(arrowBlock, 1);
                grid.Children.Add(arrowBlock);

                var rightPanel = new StackPanel();

                foreach (var item in dragTask.RightItems)
                {
                    var targetBorder = new Border
                    {
                        Background = Brushes.LightYellow,
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(2),
                        CornerRadius = new CornerRadius(5),
                        Margin = new Thickness(5),
                        Padding = new Thickness(10, 5, 10, 5),
                        MinHeight = 50,
                        AllowDrop = true
                    };

                    string sentenceText = item.DisplayName;
                    var stack = new StackPanel { Orientation = Orientation.Horizontal };
                    string[] parts = sentenceText.Split(new[] { "________" }, StringSplitOptions.None);

                    if (parts.Length > 0)
                        stack.Children.Add(new TextBlock { Text = parts[0], FontSize = 12, VerticalAlignment = VerticalAlignment.Center });

                    var answerBlock = new TextBlock
                    {
                        Text = "???",
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Blue,
                        Margin = new Thickness(5, 0, 5, 0),
                        MinWidth = 100
                    };
                    stack.Children.Add(answerBlock);

                    if (parts.Length > 1)
                        stack.Children.Add(new TextBlock { Text = parts[1], FontSize = 12, VerticalAlignment = VerticalAlignment.Center });

                    targetBorder.Child = stack;
                    string correctAnswer = item.CorrectName;

                    targetBorder.Drop += (s, e) =>
                    {
                        string dragged = e.Data.GetData(typeof(string)) as string;
                        if (dragged != null && dragged == correctAnswer)
                        {
                            answerBlock.Text = dragged;
                            answerBlock.Foreground = Brushes.Green;
                            targetBorder.Background = Brushes.LightGreen;
                            StatusText.Text = $"✅ Правильно! {dragged}";
                        }
                        else if (dragged != null)
                        {
                            answerBlock.Text = "❌";
                            answerBlock.Foreground = Brushes.Red;
                            targetBorder.Background = Brushes.LightCoral;
                            StatusText.Text = $"❌ Неправильно! {dragged} не подходит";
                        }
                    };
                    targetBorder.DragOver += (s, e) => { e.Effects = DragDropEffects.Move; e.Handled = true; };

                    rightPanel.Children.Add(targetBorder);
                }
                Grid.SetColumn(rightPanel, 2);
                grid.Children.Add(rightPanel);
                mainPanel.Children.Add(grid);

                var checkButton = new Button
                {
                    Content = "ПРОВЕРИТЬ ВСЕ СОПОСТАВЛЕНИЯ",
                    Height = 40,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Background = Brushes.Green,
                    Foreground = Brushes.White,
                    Margin = new Thickness(0, 20, 0, 0),
                    Cursor = Cursors.Hand
                };
                checkButton.Click += (s, e) =>
                {
                    int correctCountDrag = 0;
                    foreach (var child in rightPanel.Children)
                    {
                        if (child is Border border && border.Background == Brushes.LightGreen)
                            correctCountDrag++;
                    }
                    if (correctCountDrag == dragTask.RightItems.Count)
                    {
                        StatusBorder.Background = Brushes.Green;
                        StatusText.Text = $"✅ ПРАВИЛЬНО! Все {correctCountDrag} слов подобраны верно!";
                        this.correctCount++;
                        MoveNext();
                    }
                    else
                    {
                        StatusBorder.Background = Brushes.Red;
                        StatusText.Text = $"❌ НЕПРАВИЛЬНО! Правильно подобрано {correctCountDrag} из {dragTask.RightItems.Count}. Попробуйте ещё раз!";
                    }
                };
                mainPanel.Children.Add(checkButton);

                var scrollViewer = new ScrollViewer { Content = mainPanel, Height = 450 };
                ControlContainer.Children.Clear();
                ControlContainer.Children.Add(scrollViewer);
                return;
            }

            // ОБЫЧНЫЕ ЗАДАНИЯ (ВЫПАДАЮЩИЕ СПИСКИ)
            if (currentTask is TaskItem trainingTask)
            {
                NormalPanel.Visibility = Visibility.Visible;
                QuestionText.Text = trainingTask.Question;
                ShowImage(trainingTask.ImageFile);
                AnswerComboBox.Items.Clear();
                AnswerComboBox.Items.Add(new ComboBoxItem { Content = "-- Выберите ответ --", IsSelected = true });
                foreach (var opt in trainingTask.Options) AnswerComboBox.Items.Add(new ComboBoxItem { Content = opt });
                CheckButton.Visibility = Visibility.Visible;
            }

            PrevButton.IsEnabled = !isControlMode && currentIndex > 0;
            NextButton.IsEnabled = !isControlMode && currentIndex < tasks.Count - 1;
            StatusBorder.Background = Brushes.LightGray;
            StatusText.Text = "Выберите ответ и нажмите 'Проверить'";
        }

        private void ControlNextBtn_Click(object sender, RoutedEventArgs e)
        {
            var tasks = controlTasks;
            if (currentIndex >= tasks.Count) return;
            var currentTask = tasks[currentIndex];

            bool allFilled = true;
            bool isCorrect = true;
            string userAnswer = "";

            if (currentTask is ControlIntroSlide)
            {
                currentIndex++;
                ShowCurrentTask();
                return;
            }

            if (currentTask is TaskItem taskItem)
            {
                var selected = AnswerComboBox.SelectedItem as ComboBoxItem;
                if (selected == null || selected.Content.ToString() == "-- Выберите ответ --")
                {
                    StatusBorder.Background = Brushes.Red;
                    StatusText.Text = "Сначала выберите ответ!";
                    return;
                }
                userAnswer = selected.Content.ToString();
                isCorrect = (userAnswer == taskItem.CorrectAnswer);
                controlResults[currentIndex] = isCorrect;
                userAnswers[currentIndex] = userAnswer;
            }
            else if (currentTask is List<ControlBlankItem> blanks)
            {
                var answers = new List<string>();
                for (int i = 0; i < blanks.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(controlTextBoxes[i].Text))
                    {
                        allFilled = false;
                        break;
                    }
                    string ans = controlTextBoxes[i].Text.Trim();
                    answers.Add(ans);
                    if (!string.Equals(ans, blanks[i].CorrectAnswer, StringComparison.OrdinalIgnoreCase))
                        isCorrect = false;
                }
                if (!allFilled)
                {
                    StatusBorder.Background = Brushes.Red;
                    StatusText.Text = "Заполните все поля!";
                    return;
                }
                controlResults[currentIndex] = isCorrect;
                userAnswers[currentIndex] = string.Join(", ", answers);
            }
            else if (currentTask is List<ControlMatchingItem> match)
            {
                var answers = new List<string>();
                for (int i = 0; i < match.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(controlTextBoxes[i].Text))
                    {
                        allFilled = false;
                        break;
                    }
                    string letter = controlTextBoxes[i].Text.Trim().ToLower();
                    answers.Add(letter);
                    if (letter != match[i].CorrectLetter.ToLower())
                        isCorrect = false;
                }
                if (!allFilled)
                {
                    StatusBorder.Background = Brushes.Red;
                    StatusText.Text = "Заполните все поля!";
                    return;
                }
                controlResults[currentIndex] = isCorrect;
                userAnswers[currentIndex] = string.Join(", ", answers);
            }
            else if (currentTask is List<ControlEndingItem> ends)
            {
                var answers = new List<string>();
                for (int i = 0; i < ends.Count; i++)
                {
                    if (controlComboBoxes[i].SelectedIndex <= 0)
                    {
                        allFilled = false;
                        break;
                    }
                    string selected = (controlComboBoxes[i].SelectedItem as ComboBoxItem).Content.ToString();
                    answers.Add(selected);
                    if (selected != ends[i].CorrectAnswer)
                        isCorrect = false;
                }
                if (!allFilled)
                {
                    StatusBorder.Background = Brushes.Red;
                    StatusText.Text = "Выберите все варианты!";
                    return;
                }
                controlResults[currentIndex] = isCorrect;
                userAnswers[currentIndex] = string.Join(", ", answers);
            }
            else if (currentTask is MultiQuestionTask multiTask)
            {
                var answers = new List<string>();
                for (int i = 0; i < multiTask.Questions.Count; i++)
                {
                    if (controlComboBoxes[i].SelectedIndex <= 0)
                    {
                        allFilled = false;
                        break;
                    }
                    string selected = (controlComboBoxes[i].SelectedItem as ComboBoxItem).Content.ToString();
                    answers.Add(selected);
                    if (selected != multiTask.Questions[i].CorrectAnswer)
                        isCorrect = false;
                }
                if (!allFilled)
                {
                    StatusBorder.Background = Brushes.Red;
                    StatusText.Text = "Выберите все ответы!";
                    return;
                }
                controlResults[currentIndex] = isCorrect;
                userAnswers[currentIndex] = string.Join(", ", answers);
            }
            else if (currentTask is ControlImagesTask imagesTask)
            {
                var answers = new List<string>();
                for (int i = 0; i < imagesTask.Questions.Count; i++)
                {
                    if (controlTextBoxes.Count <= i || string.IsNullOrWhiteSpace(controlTextBoxes[i].Text))
                    {
                        allFilled = false;
                        break;
                    }
                    string ans = controlTextBoxes[i].Text.Trim();
                    answers.Add(ans);
                    if (!string.Equals(ans, imagesTask.CorrectAnswers[i], StringComparison.OrdinalIgnoreCase))
                        isCorrect = false;
                }
                if (!allFilled)
                {
                    StatusBorder.Background = Brushes.Red;
                    StatusText.Text = "Заполните все поля!";
                    return;
                }
                controlResults[currentIndex] = isCorrect;
                userAnswers[currentIndex] = string.Join(", ", answers);
            }
            currentIndex++;
            ShowCurrentTask();
        }

        private void ShowControlResultTable()
        {
            // Пропускаем первый элемент (вводный слайд)
            var realTasks = controlTasks.Skip(1).ToList();
            var realResults = new Dictionary<int, bool>();
            var realAnswers = new Dictionary<int, string>();

            for (int i = 0; i < realTasks.Count; i++)
            {
                int originalIndex = i + 1;
                if (controlResults.ContainsKey(originalIndex))
                    realResults[i] = controlResults[originalIndex];
                if (userAnswers.ContainsKey(originalIndex))
                    realAnswers[i] = userAnswers[originalIndex];
            }

            var resultWindow = new Window
            {
                Title = "Результат контрольного теста",
                Width = 1100,
                Height = 700,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Background = Brushes.White
            };

            var mainPanel = new StackPanel { Margin = new Thickness(20) };

            int total = realTasks.Count;
            int correct = realResults.Count(r => r.Value);
            int incorrect = total - correct;

            var titleBlock = new TextBlock
            {
                Text = $"Результат контрольного теста: {correct} из {total}",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = correct >= total * 0.7 ? Brushes.Green : Brushes.Orange,
                Margin = new Thickness(0, 0, 0, 15),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            mainPanel.Children.Add(titleBlock);

            var tablePanel = new StackPanel();

            var headerPanel = new WrapPanel { Margin = new Thickness(0, 0, 0, 5) };
            var headers = new (string name, int width)[]
            {
                ("№", 50),
                ("Ответ ученика", 350),
                ("Правильный ответ", 350),
                ("Результат", 80)
            };

            foreach (var header in headers)
            {
                var headerBorder = new Border
                {
                    Background = Brushes.LightGray,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Width = header.width,
                    MinHeight = 40
                };
                headerBorder.Child = new TextBlock
                {
                    Text = header.name,
                    FontWeight = FontWeights.Bold,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5),
                    TextWrapping = TextWrapping.Wrap
                };
                headerPanel.Children.Add(headerBorder);
            }
            tablePanel.Children.Add(headerPanel);

            for (int i = 0; i < realTasks.Count; i++)
            {
                bool isCorrect = realResults.ContainsKey(i) && realResults[i];
                string userAnswer = realAnswers.ContainsKey(i) ? realAnswers[i] : "—";
                string correctAnswer = GetCorrectAnswerForTask(realTasks[i]);

                var rowPanel = new WrapPanel();

                var numBorder = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Width = 50,
                    MinHeight = 40
                };
                numBorder.Child = new TextBlock
                {
                    Text = (i + 1).ToString(),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5)
                };
                rowPanel.Children.Add(numBorder);

                var userBorder = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Width = 350,
                    MinHeight = 40
                };
                userBorder.Child = new TextBlock
                {
                    Text = userAnswer,
                    TextAlignment = TextAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5),
                    TextWrapping = TextWrapping.Wrap
                };
                rowPanel.Children.Add(userBorder);

                var correctBorder = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Width = 350,
                    MinHeight = 40
                };
                correctBorder.Child = new TextBlock
                {
                    Text = correctAnswer,
                    TextAlignment = TextAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5),
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.Green
                };
                rowPanel.Children.Add(correctBorder);

                var resultBorder = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Width = 80,
                    MinHeight = 40
                };
                resultBorder.Child = new TextBlock
                {
                    Text = isCorrect ? "✅" : "❌",
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Padding = new Thickness(5),
                    Foreground = isCorrect ? Brushes.Green : Brushes.Red,
                    FontSize = 16
                };
                rowPanel.Children.Add(resultBorder);

                tablePanel.Children.Add(rowPanel);
            }

            var scrollViewer = new ScrollViewer
            {
                Content = tablePanel,
                Height = 500,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto
            };
            mainPanel.Children.Add(scrollViewer);

            var closeButton = new Button
            {
                Content = "Закрыть",
                Width = 100,
                Height = 35,
                Margin = new Thickness(0, 15, 0, 0),
                Background = Brushes.Navy,
                Foreground = Brushes.White,
                Cursor = Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            closeButton.Click += (s, e) => resultWindow.Close();
            mainPanel.Children.Add(closeButton);

            resultWindow.Content = mainPanel;
            resultWindow.ShowDialog();
        }

        private string GetCorrectAnswerForTask(object task)
        {
            if (task is List<ControlBlankItem> blanks)
                return string.Join(", ", blanks.Select(b => b.CorrectAnswer));
            else if (task is List<ControlMatchingItem> match)
                return string.Join(", ", match.Select(m => m.CorrectLetter));
            else if (task is List<ControlEndingItem> ends)
                return string.Join(", ", ends.Select(e => e.CorrectAnswer));
            else if (task is MultiQuestionTask multi)
                return string.Join(", ", multi.Questions.Select(q => q.CorrectAnswer));
            else if (task is TaskItem taskItem)
                return taskItem.CorrectAnswer;
            return "?";
        }

        private void ShowImage(string file)
        {
            if (string.IsNullOrEmpty(file)) { ImageArea.Visibility = Visibility.Collapsed; return; }
            ImageArea.Visibility = Visibility.Visible;
            try { TaskImage.Source = new BitmapImage(new Uri($"Images/{file}", UriKind.Relative)); }
            catch { TaskImage.Source = null; }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (isControlMode) return;
            var task = allTasks[currentIndex] as TaskItem;
            if (task == null) return;

            var sel = AnswerComboBox.SelectedItem as ComboBoxItem;
            if (sel == null || sel.Content.ToString() == "-- Выберите ответ --")
            {
                StatusBorder.Background = Brushes.Red;
                StatusText.Text = "Выберите ответ!";
                return;
            }

            if (sel.Content.ToString() == task.CorrectAnswer)
            {
                correctCount++;
                StatusBorder.Background = Brushes.Green;
                StatusText.Text = "✅ ПРАВИЛЬНО!";
                MoveNext();
            }
            else
            {
                StatusBorder.Background = Brushes.Red;
                StatusText.Text = "❌ НЕПРАВИЛЬНО!";
            }
        }

        private void MoveNext()
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, a) => { timer.Stop(); currentIndex++; ShowCurrentTask(); };
            timer.Start();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isControlMode && currentIndex > 0) { currentIndex--; ShowCurrentTask(); }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isControlMode && currentIndex < allTasks.Count - 1) { currentIndex++; ShowCurrentTask(); }
        }

        private void Part1Btn_Click(object sender, RoutedEventArgs e)
        {
            isControlMode = false;
            currentIndex = 0;
            correctCount = 0;
            controlResults.Clear();
            userAnswers.Clear();
            ModeText.Text = "ЧАСТЬ 1: ОБУЧЕНИЕ";
            ScoreText.Text = $"0/{allTasks.Count - 2}";
            ShowCurrentTask();
        }

        private void Part2Btn_Click(object sender, RoutedEventArgs e)
        {
            isControlMode = true;
            currentIndex = 0;
            controlResults.Clear();
            userAnswers.Clear();
            ModeText.Text = "ЧАСТЬ 2: КОНТРОЛЬ";
            ScoreText.Text = $"0/{controlTasks.Count - 1}";
            ShowCurrentTask();
        }
    }
}
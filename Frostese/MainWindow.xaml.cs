using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
using System;
using System.Timers;
using System.Windows.Controls.Primitives;

namespace Frostese
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer _soundPlayer;
        private double _playbackSpeed;
        private double _tempo;
        private string sentenceToSpeak;

        private Timer aTimer;
        private int currPosition;
        private char[] _splitSentence;
        private bool isSpeaking = false;
        Random random;
        private bool darkModeBool = false;
        DraggablePopup draggablePopup;
        private int _lowRandVal = 0;
        private int _highRandVal = 0;

        public MainWindow()
        {
            _soundPlayer = new SoundPlayer();
            random = new Random();
            
            InitializeComponent();

            draggablePopup = this.dragPopup;
        }


        private void stopSpeak(object sender, RoutedEventArgs e)
        {
            if (aTimer != null)
                StopTimer();
        }
        private void onSpeakBtnClick(object sender, RoutedEventArgs e)
        {
            draggablePopup.txtToShow.Text = "";
            this.txtToShow.Text = "";
            _playbackSpeed = (this.playBackSpeed.Value);
            _tempo = this.tempo.Value;
            if (_tempo == 0)
                _tempo = 100;

            sentenceToSpeak = this.txtToSpeak.Text;
            if (sentenceToSpeak.Length == 0)
                return;
            _splitSentence = sentenceToSpeak.ToCharArray();

            if (isSpeaking)
                StopTimer();
            StartSpeaking();
        }

        private void onClickChatBtn(object sender, RoutedEventArgs e)
        {
            if(draggablePopup.grd.Visibility == Visibility.Collapsed)
            {
                draggablePopup.grd.Visibility = Visibility.Visible;
            }
            else
            {
                draggablePopup.grd.Visibility = Visibility.Collapsed;
            }
        }

        private void StartSpeaking()
        {
            isSpeaking = true;
            currPosition = 0;
            aTimer = new System.Timers.Timer(_tempo);
            aTimer.Elapsed += OnTimedEventAsync;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private async void OnTimedEventAsync(object sender, ElapsedEventArgs e)
        {
            var character = _splitSentence[currPosition].ToString();
            if(character == "c" && currPosition+1 <= _splitSentence.Length - 1)
            {
                if (_splitSentence[currPosition + 1].ToString() == "h")
                {
                    character += "h";
                    currPosition++;
                }
            }
            if (character == "p" && currPosition + 1 <= _splitSentence.Length - 1)
            {
                if (_splitSentence[currPosition + 1].ToString() == "h")
                {
                    character += "h";
                    currPosition++;
                }
            }
            if (character == "s" && currPosition + 1 <= _splitSentence.Length - 1)
            {
                if (_splitSentence[currPosition + 1].ToString() == "h")
                {
                    character += "h";
                    currPosition++;
                }
            }
            if (character == "t" && currPosition + 1 <= _splitSentence.Length - 1)
            {
                if (_splitSentence[currPosition + 1].ToString() == "h")
                {
                    character += "h";
                    currPosition++;
                }
            }
            if (character == "w" && currPosition + 1 <= _splitSentence.Length - 1)
            {
                if (_splitSentence[currPosition + 1].ToString() == "h")
                {
                    character += "h";
                    currPosition++;
                }
            }

            await Speak(character);
            currPosition++;
            if (currPosition == _splitSentence.Length)
            {
                StopTimer();
            }
        }

        private void StopTimer()
        {
            isSpeaking = false;
            aTimer.Stop();
        }

        private async Task Speak(string character)
        {   
            //_soundPlayer = new SoundPlayer(Frostese.Properties.Resources.a);
            try
            {
                System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
                Stream s = a.GetManifestResourceStream($"Frostese.Sounds.{character.ToLower()}.wav");
                this.Dispatcher.Invoke(() =>
                {
                    draggablePopup.txtToShow.Text += character;
                    this.txtToShow.Text += character;
                });
                
                if (s == null)
                {

                    return;
                }
                byte[] b = streamToByteArray(s);
                //int SampleRate = BitConverter.ToInt32(b, 24);
                double newSR = _playbackSpeed + random.NextDouble()/4;
                Console.WriteLine(newSR);
                var intSR = (int)Math.Round(newSR, 0);
                Array.Copy(BitConverter.GetBytes(intSR), 0, b, 24, 4);

                aTimer.Interval = _tempo+ random.Next(_lowRandVal, _highRandVal);
                Console.WriteLine(aTimer.Interval);
                _soundPlayer = new SoundPlayer(new MemoryStream(b));
                _soundPlayer.Play();
                


            }
            catch(Exception e)
            {

            }
        }

        public static byte[] streamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        private void darkMode_Click(object sender, RoutedEventArgs e)
        {
            if (darkModeBool)
            {
                darkModeBool = false;
                var color = new Color();
                color.R = 200;
                color.G = 200;
                color.B = 200;
                color.A = 255;
                Brush brush = new SolidColorBrush(color);
                this.mainGrid.Background = brush;
            }
            else
            {
                darkModeBool = true;
                var color = new Color();
                color.R = 0;
                color.G = 0;
                color.B = 0;
                color.A = 255;
                Brush brush = new SolidColorBrush(color);
                this.mainGrid.Background = brush;
            }
        }

        private void cp_Background(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp_bg.SelectedColor.HasValue)
            {
                Color C = cp_bg.SelectedColor.Value;
                draggablePopup.border.Background = new SolidColorBrush(C);
                draggablePopup.polyBorder.Fill = new SolidColorBrush(C);
                draggablePopup.polyBorder.Stroke = new SolidColorBrush(C);
            }

        }

        private void cp_Text(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp_txt.SelectedColor.HasValue)
            {
                Color C = cp_txt.SelectedColor.Value;
                draggablePopup.txtToShow.Foreground = new SolidColorBrush(C);
            }

        }
        private void cp_Border(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (cp_border.SelectedColor.HasValue)
            {
                Color C = cp_border.SelectedColor.Value;
                draggablePopup.border.BorderBrush = new SolidColorBrush(C);
                draggablePopup.l1.Stroke = new SolidColorBrush(C);
                draggablePopup.l2.Stroke = new SolidColorBrush(C);
            }

        }

        private void OnFontWeightSelection(object sender, SelectionChangedEventArgs e)
        {
            var cbItem = ((ComboBox)sender).SelectedItem as ComboBoxItem;
            draggablePopup.txtToShow.FontWeight = cbItem.FontWeight;
        }

        private void OnFontSelection(object sender, SelectionChangedEventArgs e)
        {
            var cbItem = ((ComboBox)sender).SelectedItem as ComboBoxItem;
            draggablePopup.txtToShow.FontFamily = cbItem.FontFamily;
        }

        private void randDelaySlider_HigherValueChanged(object sender, RoutedEventArgs e)
        {
            _lowRandVal = (int)this.randDelaySlider.LowerValue;
        }

        private void randDelaySlider_LowerValueChanged(object sender, RoutedEventArgs e)
        {
            _highRandVal = (int)this.randDelaySlider.HigherValue;
        }
    }
}


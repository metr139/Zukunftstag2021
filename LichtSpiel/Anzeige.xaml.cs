using System.Windows;

namespace LichtSpiel
{
    public partial class Anzeige : Window
    {
        private readonly Logik _logik;

        public Anzeige()
        {
            InitializeComponent();

            _logik = new Logik(this);
            _logik.LichtSpielAufstarten();
        }
        
        private async void Start_Knopf_Geklickt(object sender, RoutedEventArgs e)
        {
            await _logik.Start_Knopf_Geklickt();
        }

        private void BenutzerKnopfRot_Geklickt(object sender, RoutedEventArgs e)
        {
            _logik.BenutzerKnopfRot_Geklickt();
        }

        private void BenutzerKnopfBlau_Geklickt(object sender, RoutedEventArgs e)
        {
            _logik.BenutzerKnopfBlau_Geklickt();
        }

        private void BenutzerKnopfGruen_Geklickt(object sender, RoutedEventArgs e)
        {
            _logik.BenutzerKnopfGruen_Geklickt();
        }
    }
}

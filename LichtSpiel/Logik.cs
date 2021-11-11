using System;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace LichtSpiel
{
    public class Logik
    {
        private readonly Anzeige _anzeige;
        private Liste<Farbe> _generierteListe;
        private Liste<Farbe> _benutzerListe;

        public Logik(Anzeige anzeige)
        {
            _anzeige = anzeige;
        }

        public void LichtSpielAufstarten()
        {
            _anzeige.BenutzerKnopfRot.IsEnabled = false;
            _anzeige.BenutzerKnopfBlau.IsEnabled = false;
            _anzeige.BenutzerKnopfGruen.IsEnabled = false;
            _anzeige.BenutzerKnopfGelb.IsEnabled = false;

            _anzeige.KnopfRot.Background = Brushes.DarkRed;
            _anzeige.BenutzerKnopfRot.Background = Brushes.DarkRed;

            _anzeige.KnopfBlau.Background = Brushes.SteelBlue;
            _anzeige.BenutzerKnopfBlau.Background = Brushes.SteelBlue;

            _anzeige.KnopfGruen.Background = Brushes.Green;
            _anzeige.BenutzerKnopfGruen.Background = Brushes.Green;

            _anzeige.KnopfGelb.Background = Brushes.Yellow;
            _anzeige.BenutzerKnopfGelb.Background = Brushes.Yellow;
        }

        public async Task Start_Knopf_Geklickt()
        {
            KnoepfeAbschalten();
            await Task.Delay(400);

            _generierteListe = ZufaelligeFarbliste();
            _benutzerListe = new Liste<Farbe>();

            await FarbenAbspielen(_generierteListe);
            KnoepfeEinschalten();
        }

        private void KnoepfeEinschalten()
        {
            _anzeige.KnopfStart.IsEnabled = true;
            _anzeige.BenutzerKnopfRot.IsEnabled = true;
            _anzeige.BenutzerKnopfBlau.IsEnabled = true;
            _anzeige.BenutzerKnopfGruen.IsEnabled = true;
            
            _anzeige.BenutzerKnopfGelb.IsEnabled = true;
        }

        private void KnoepfeAbschalten()
        {
            _anzeige.KnopfStart.IsEnabled = false;
            BenutzerKnoepfeAbschalten();
        }

        private void BenutzerKnoepfeAbschalten()
        {
            _anzeige.BenutzerKnopfRot.IsEnabled = false;
            _anzeige.BenutzerKnopfBlau.IsEnabled = false;
            _anzeige.BenutzerKnopfGruen.IsEnabled = false;
            _anzeige.BenutzerKnopfGelb.IsEnabled = false;
        }

        private async Task FarbenAbspielen(Liste<Farbe> farbliste)
        {
            foreach (Farbe farbe in farbliste)
            {
                await EineFarbeAbspielen(farbe);
            }
        }

        private async Task EineFarbeAbspielen(Farbe farbe)
        {
            Storyboard animation = _anzeige.FindResource("KnopfAnimation") as Storyboard;
            Button aktuellerKnopf = null;

            switch (farbe)
            {
                case Farbe.Rot:
                    {
                        aktuellerKnopf = _anzeige.KnopfRot;
                        break;
                    }
                case Farbe.Blau:
                    {
                        aktuellerKnopf = _anzeige.KnopfBlau;
                        break;
                    }
                case Farbe.Gruen:
                    {
                        aktuellerKnopf = _anzeige.KnopfGruen;
                        break;
                    }
                case Farbe.Gelb:
                    {
                        aktuellerKnopf = _anzeige.KnopfGelb;
                        break;
                    }
            }

            Storyboard.SetTarget(animation, aktuellerKnopf);
            await AnimationAbspielen(animation);
        }

        private async Task AnimationAbspielen(Storyboard animation)
        {
            var animationKomplett = new TaskCompletionSource<bool>();
            EventHandler kompletierer = (s, e) => animationKomplett.SetResult(true);

            animation.Completed += kompletierer;

            animation.Begin();
            await animationKomplett.Task;

            animation.Completed -= kompletierer;
        }

        private Liste<Farbe> ZufaelligeFarbliste()
        {
            var neueFarbenListe = new Liste<Farbe>();
            var zufallsGenerator = new FarbenZufallsGenerator();

            Farbe farbe = zufallsGenerator.GibFarbe();
            neueFarbenListe.Hinzufuegen(farbe);
            farbe = zufallsGenerator.GibFarbe();
            neueFarbenListe.Hinzufuegen(farbe);
            farbe = zufallsGenerator.GibFarbe();
            neueFarbenListe.Hinzufuegen(farbe);

            farbe = zufallsGenerator.GibFarbe();
            neueFarbenListe.Hinzufuegen(farbe);
            farbe = zufallsGenerator.GibFarbe();
            neueFarbenListe.Hinzufuegen(farbe);

            return neueFarbenListe;
        }

        public void BenutzerKnopfRot_Geklickt()
        {
            _benutzerListe.Hinzufuegen(Farbe.Rot);
            BenutzerEingabeUeberpruefen();
        }

        public void BenutzerKnopfBlau_Geklickt()
        {
            _benutzerListe.Hinzufuegen(Farbe.Blau);
            BenutzerEingabeUeberpruefen();
        }

        public void BenutzerKnopfGruen_Geklickt()
        {
            _benutzerListe.Hinzufuegen(Farbe.Gruen);
            BenutzerEingabeUeberpruefen();
        }

        public void BenutzerKnopfGelb_Geklickt()
        {
            _benutzerListe.Hinzufuegen(Farbe.Gelb);
            BenutzerEingabeUeberpruefen();
        }

        private void BenutzerEingabeUeberpruefen()
        {
            if (_benutzerListe.Laenge == _generierteListe.Laenge)
            {
                for (int i = 0; i < _benutzerListe.Laenge; i++)
                {
                    if (_benutzerListe[i] != _generierteListe[i])
                    {
                        UngueltigeEingabe();
                        BenutzerKnoepfeAbschalten();
                        return;
                    }
                }

                RichtigeEingabe();
                BenutzerKnoepfeAbschalten();
                return;
            }
        }

        private void UngueltigeEingabe()
        {
            TonAbspielen("LichtSpiel.sounds.boo.wav");
            BildAnzeigen("False.png");
        }

        private void RichtigeEingabe()
        {
            TonAbspielen("LichtSpiel.sounds.applause.wav");
            BildAnzeigen("True.png");
        }

        private void BildAnzeigen(string bildName)
        {
            string bildPfad = "pack://application:,,,/images/" + bildName;
            _anzeige.StatusBild.Source = new BitmapImage(new Uri(bildPfad));
        }

        private void TonAbspielen(string tonName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream(tonName);

            SoundPlayer player = new SoundPlayer(stream);
            player.Load();
            player.Play();
        }
    }
}
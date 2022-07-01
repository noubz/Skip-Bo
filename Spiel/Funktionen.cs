using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace Skip_Bo
{
    class Funktionen
    {
        // Funktionen für die Karten- und Stapelauswahl des Windows-Forms
        public static string ablegen_von = null, ablegen_auf = null;
        public static PictureBox ablegen_von_aktuell = null, ablegen_auf_aktuell = null;
        public static int ablegen_von_Nummer = 0, ablegen_auf_Nummer = 0;
        public static string ablegen_von_karte = null;

        #region Funktionen zum berechnen der Listen
        // Text-Variblen definieren
        public static List<string> kartenstock { get; set; }
        public static List<List<string>> ablegestapel { get; set; }

        // Funktion zur Erstellung eines Skip-Bo decks
        public static List<string> generiere_Kartendeck()
        {
            List<string> deck = new List<string>(); // Neue Liste/Deck

            // Schleife für die karten 1-12 (12-mal)
            for (int i = 0; i < 12; i++) {
                foreach (int karte in Enumerable.Range(1, 12))
                {
                    deck.Add(karte.ToString());
                }
            }
            // Schleife für Skip-Bo Karten (18 Stück)
            for (int i = 0; i < 18; i++) {
                deck.Add("skip-bo");
            }

            Random random = new Random(); // Objektverweis für die Random klasse
            var mischen = deck.OrderBy(a => random.Next());
            deck = mischen.ToList(); // Karten mischen

            return deck; // Rückgabe des Decks
        }

        // Funktion zur Erstellung eines Kartenstapels aus dem Kartenstock
        public static List<string> erstelle_Stapel(int anzahl_karten)
        {
            List<string> stapel = new List<string>(); // Neue Liste/Stapel

            // Schleife, um die Kartenanzahl dem Stapel hinzuzufügen
            for (int i = 0; i < anzahl_karten; i++) {
                stapel.Add(kartenstock[0]);
                kartenstock.RemoveAt(0); // Entfernen der Karte aus dem Kartenstock
            }

            return stapel; // Rückgabe des Stapels
        }

        // Funktion zur Ergänzung eines Stapels, um (anzahl_Karten) Karten
        public static void ergaenze_Stapel(List<string> stapel, int anzahl_karten)
        {
            for (int i = 0; i < anzahl_karten; i++)
            {
                stapel.Add(kartenstock[0]);
                kartenstock.RemoveAt(0);
            }
        }

        // Funktion zur Ablage einer Karte
        public static Tuple<bool, bool> karte_Ablegen()
        {
            bool zug_zuende = false, zugMöglich = false;
            int stapel_der_kartenNummer = 0;
            List<string> ablegen_von_liste = new List<string>();

            if (ablegen_von == "spielerstapel") { ablegen_von_liste = new List<string>(Zug.spielerstapel[Zug.aktuellerSpieler]); }
            else if (ablegen_von == "hilfsstapel") {
                stapel_der_kartenNummer = ablegen_von_Nummer;
                ablegen_von_Nummer = 0;
                ablegen_von_liste = new List<string>(Zug.hilfsstapel[Zug.aktuellerSpieler][stapel_der_kartenNummer]);
            }
            else { ablegen_von_liste = new List<string>(Zug.spielerhand[Zug.aktuellerSpieler]); }

            if (ablegen_auf == "hilfsstapel" && ablegen_von != "spielerstapel")
            {
                if (Zug.hilfsstapel[Zug.aktuellerSpieler][ablegen_auf_Nummer][0] == "Leer") {
                    Zug.hilfsstapel[Zug.aktuellerSpieler][ablegen_auf_Nummer][0] = ablegen_von_karte;
                    ablegen_von_liste.RemoveAt(ablegen_von_Nummer);
                }
                else {
                    Zug.hilfsstapel[Zug.aktuellerSpieler][ablegen_auf_Nummer].Insert(0, ablegen_von_karte);
                    ablegen_von_liste.RemoveAt(ablegen_von_Nummer);
                }
                zug_zuende = true;
                zugMöglich = true;
            }

            else if (ablegen_auf == "ablegestapel")
            {
                if (ablegestapel[ablegen_auf_Nummer][0] == "Leer" && (ablegen_von_karte == "1" || ablegen_von_karte == "skip-bo")) {
                    ablegestapel[ablegen_auf_Nummer][0] = ablegen_von_liste[ablegen_von_Nummer];
                    ablegen_von_liste.RemoveAt(ablegen_von_Nummer);
                    zugMöglich = true;
                }
                else if (ablegestapel[ablegen_auf_Nummer][0] != "Leer" && (ablegen_von_karte == (ablegestapel[ablegen_auf_Nummer].Count() + 1).ToString() || ablegen_von_karte == "skip-bo")) {
                    ablegestapel[ablegen_auf_Nummer].Insert(0, ablegen_von_liste[ablegen_von_Nummer]);
                    ablegen_von_liste.RemoveAt(ablegen_von_Nummer);
                    zugMöglich = true;
                }
            }

            if (ablegen_von == "spielerhand") { Zug.spielerhand[Zug.aktuellerSpieler] = new List<string>(ablegen_von_liste); }
            else if (ablegen_von == "spielerstapel") { Zug.spielerstapel[Zug.aktuellerSpieler] = new List<string>(ablegen_von_liste); }
            else if (ablegen_von == "hilfsstapel") { Zug.hilfsstapel[Zug.aktuellerSpieler][stapel_der_kartenNummer] = new List<string>(ablegen_von_liste); }

            return new Tuple<bool, bool>(zug_zuende, zugMöglich);
        }
        #endregion

        #region Funktionen für Windows-Forms

        // Definition der Box-Listen
        public static List<PictureBox> handkarten_boxen { get; set; }
        public static List<PictureBox> hilfsstapel_boxen { get; set; }
        public static List<PictureBox> ablegestapel_boxen { get; set; }

        // Definition der Box-Variablen
        public static PictureBox spielerstapel_box { get; set; }
        public static Label infobox_box { get; set; }

        // Funktion, um des Pfad eines Bildes zu bekommen
        public static Image bekomme_image(string karte)
        {
            string dateiname = karte + ".png";
            string pfad = Path.Combine(Environment.CurrentDirectory, "Bilder", dateiname);
            Image image = Image.FromFile(pfad);

            return image;
        }

        public static void karte_markieren(PictureBox karte, bool karte_ausgewählt, string farbe)
        {
            int Position_x = karte.Location.X;
            int Position_y = karte.Location.Y;
            Image hintergrund = bekomme_image("Back_" + farbe);

            if (karte_ausgewählt)
            {
                karte.Location = new Point(Position_x, Position_y + 10);
                karte.BackgroundImage = hintergrund;
            }
            else
            {
                karte.Location = new Point(Position_x, Position_y - 10);
                karte.BackgroundImage = hintergrund;
            }
        }

        public static bool ablegen_von_ausgewählt = false, ablegen_auf_ausgewählt = false;

        public static void ablegen_von_auswählen(object sender, List<PictureBox> boxen, List<string> stapel)
        {
            Form1.auswahl_sound.Play();
            if (ablegen_von_ausgewählt)
            {
                karte_markieren(ablegen_von_aktuell, ablegen_von_ausgewählt, "schwarz");
                ablegen_von_ausgewählt = false;
            }
            karte_markieren(sender as PictureBox, ablegen_von_ausgewählt, "schwarz");
            ablegen_von_ausgewählt = true;
            ablegen_von_Nummer = boxen.IndexOf(sender as PictureBox);
            ablegen_von_karte = stapel[ablegen_von_Nummer];
            ablegen_von_aktuell = sender as PictureBox;
        }

        public static void ablegen_auf_auswählen(object sender, List<PictureBox> boxen)
        {
            Form1.auswahl_sound.Play();
            if (ablegen_auf_ausgewählt)
            {
                karte_markieren(ablegen_auf_aktuell, ablegen_auf_ausgewählt, "schwarz");
                ablegen_auf_ausgewählt = false;
            }
            karte_markieren(sender as PictureBox, ablegen_auf_ausgewählt, "grau");
            ablegen_auf_ausgewählt = true;
            ablegen_auf_Nummer = boxen.IndexOf(sender as PictureBox);
            ablegen_auf_aktuell = sender as PictureBox;
        }

        public static void ablegen_von_hilfsstapel(object sender, List<PictureBox> boxen, List<List<string>> stapel)
        {
            Form1.auswahl_sound.Play();
            if (ablegen_von_ausgewählt)
            {
                karte_markieren(ablegen_von_aktuell, ablegen_von_ausgewählt, "schwarz");
                ablegen_von_ausgewählt = false;
            }
            karte_markieren(sender as PictureBox, ablegen_von_ausgewählt, "schwarz");
            ablegen_von_ausgewählt = true;
            ablegen_von_Nummer = boxen.IndexOf(sender as PictureBox);
            ablegen_von_karte = stapel[ablegen_von_Nummer][0];
            ablegen_von_aktuell = sender as PictureBox;
        }


        public static PictureBox animationsKarte;
        public static void karte_Animieren(PictureBox karte, Point ziel)
        {
            System.Media.SoundPlayer sound = new System.Media.SoundPlayer();
            sound.SoundLocation = System.IO.Path.Combine(Environment.CurrentDirectory, "Sounds", "woosh.wav"); sound.Load();
            sound.Play();

            int schritte = 20;

            animationsKarte.Image = karte.Image; animationsKarte.Location = karte.Location;
            animationsKarte.Refresh(); 
            karte.Visible = false;

            int y_proTick = (karte.Location.Y - ziel.Y) / schritte;
            int x_proTick = (karte.Location.X - ziel.X) / schritte;

            for (int i = 0; i < schritte; i++) {
                animationsKarte.Location = new Point(animationsKarte.Location.X - x_proTick, animationsKarte.Location.Y - y_proTick);
                animationsKarte.Update();
                animation_Refresh();
            }

            int x_übrig = (karte.Location.X - ziel.X) % schritte;
            int y_übrig = (karte.Location.Y - ziel.Y) % schritte;

            animationsKarte.Location = new Point(animationsKarte.Location.X - x_übrig, animationsKarte.Location.Y - y_übrig);
            animationsKarte.Update();

            karte.Visible = true;
        }

        public static List<Label> alle_Labels = new List<Label>();
        public static void animation_Refresh()
        {
            int y_pos = animationsKarte.Location.Y;
            int height = animationsKarte.Height;

            if (y_pos < 255) { foreach (PictureBox karte in ablegestapel_boxen) { karte.Refresh(); } }
            if (y_pos > 330 - height && y_pos < 590) { foreach (PictureBox karte in hilfsstapel_boxen) { karte.Refresh(); } }
            if (y_pos > 590 - height) { foreach (PictureBox karte in handkarten_boxen) { karte.Refresh(); } }
            
            if (ablegen_von == "spielerstapel" && y_pos > 590) { spielerstapel_box.Refresh(); }
            alle_Labels[0].Refresh();
            alle_Labels[1].Refresh();
        }
        #endregion
    }
}

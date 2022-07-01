using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Drawing;
using System.Xml.Serialization;
using System.Media;

namespace Skip_Bo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); // Inizialisierung des Windows-Forms

            // Hintergrundmusik
            musik = new MediaPlayer();
            musik.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sounds", "ingame_musik.mp3")));
            musik.Volume = 0.8;
            musik.MediaEnded += new EventHandler(Musik_MediaEnded);

            // Click-Sound
            click_sound = new SoundPlayer();
            click_sound.SoundLocation = System.IO.Path.Combine(Environment.CurrentDirectory, "Sounds", "click.wav");
            click_sound.Load();

            // Auswahl Sound
            auswahl_sound = new SoundPlayer();
            auswahl_sound.SoundLocation = System.IO.Path.Combine(Environment.CurrentDirectory, "Sounds", "Auswahl.wav");
            auswahl_sound.Load();

            // Alle Labels
            Funktionen.alle_Labels = new List<Label> { übrige_spielerstapelkarten, hilfsstapel_label };

            // Zuweisung der Box-Listen für Spiel
            Funktionen.handkarten_boxen = new List<PictureBox>() { handkarte_1_box, handkarte_2_box, handkarte_3_box, handkarte_4_box, handkarte_5_box };
            Funktionen.hilfsstapel_boxen = new List<PictureBox>() { hilfsstapel_1_box, hilfsstapel_2_box, hilfsstapel_3_box, hilfsstapel_4_box };
            Funktionen.ablegestapel_boxen = new List<PictureBox>() { ablegestapel_1_box, ablegestapel_2_box, ablegestapel_3_box, ablegestapel_4_box };
            Zug.spielerstapel_anzeige = new List<Tuple<Label, PictureBox>>() { Tuple.Create(spielerstapel_anzeige_1_label, spielerstapel_anzeige_1_picBox), Tuple.Create(spielerstapel_anzeige_2_label, spielerstapel_anzeige_2_picBox), Tuple.Create(spielerstapel_anzeige_3_label, spielerstapel_anzeige_3_picBox) };

            // Zuweisung für ablegestapel Label
            Zug.ablegestapel_anzeigen = new List<Label>() { ablegestapel_anzeiger_1, ablegestapel_anzeiger_2, ablegestapel_anzeiger_3, ablegestapel_anzeiger_4 };

            // Zuweisung der Animations-Karte
            Funktionen.animationsKarte = animation_card;

            // Zuweisung der Box-Variablen für Spiel
            Funktionen.spielerstapel_box = spielerstapel_box;
            Funktionen.infobox_box = aktuellerSpieler_label;
            Zug.übrige_spielerstapelkarten = übrige_spielerstapelkarten;

            // Variablen für hauptmenü
            Szenen.hauptmenü_panel = hauptmenü_panel;

            // Variablen für Optionen
            Szenen.optionen_panel = optionen_panel;
            Szenen.optionen_animationen_button = optionen_musik_button;
            Szenen.optionen_musik_trackBar = optionen_lautstärke_trackbar;

            // Variablen für end_menü
            Szenen.end_menü = End_menü;
            Szenen.end_menü_label = gewinner_label;

            // Variablen für spieleinstellungen
            Szenen.spieleinstellungen_panel = Spieleinstellungen_panel;
            Szenen.spieleinstellungen_spielerstapel = spieleinstellungen_spielerstapel;
            Szenen.spieleinstellungen_spielerzahl = spieleinstellungen_spieleranzahl;
            Szenen.spieleinstellungen_button = spieleinstellungen_eingabenBestätigen_button;
            Szenen.spieleinstellungen_spielernamen_eingabe = new List<Tuple<Label, TextBox>>() {
                Tuple.Create(spieleinstellungen_spieler1_label, spieleinstellungen_spieler1_eingabe),
                Tuple.Create(spieleinstellungen_spieler2_label, spieleinstellungen_spieler2_eingabe),
                Tuple.Create(spieleinstellungen_spieler3_label, spieleinstellungen_spieler3_eingabe),
                Tuple.Create(spieleinstellungen_spieler4_label, spieleinstellungen_spieler4_eingabe)
            };

            // Variablen für den Zurücksetzen Screen
            Szenen.zurücksetzen_panel = Zurücksetzen_panel;
            Szenen.zurücksetzen_laden_label = zurücksetzen_punkte_label;
            Szenen.zurücksetzen_progressBar = zurücksetzen_progressBar;

            Szenen.hauptmenü(false);
            musik.Play();
        }

        public static MediaPlayer musik { get; set; }
        static SoundPlayer click_sound { get; set; }
        public static SoundPlayer auswahl_sound { get; set; }
        public static bool animationen_aktiviert = true;

        #region Spiel

        #region Spielerstapel und Handkarten Mouseclick
        private void spielerstapel_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Zug.ablegen_von_auswählen(sender, new List<PictureBox>() { Funktionen.spielerstapel_box }, Zug.spielerstapel[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "spielerstapel";
            }
        }

        private void handkarte_1_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Zug.ablegen_von_auswählen(sender, Funktionen.handkarten_boxen, Zug.spielerhand[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "spielerhand";
            }
        }

        private void handkarte_2_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Zug.ablegen_von_auswählen(sender, Funktionen.handkarten_boxen, Zug.spielerhand[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "spielerhand";
            }
        }

        private void handkarte_3_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Zug.ablegen_von_auswählen(sender, Funktionen.handkarten_boxen, Zug.spielerhand[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "spielerhand";
            }
        }

        private void handkarte_4_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Zug.ablegen_von_auswählen(sender, Funktionen.handkarten_boxen, Zug.spielerhand[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "spielerhand";
            }
        }

        private void handkarte_5_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Zug.ablegen_von_auswählen(sender, Funktionen.handkarten_boxen, Zug.spielerhand[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "spielerhand";
            }
        }
        #endregion
        #region Hilfsstapel Mouseclick
        private void hilfsstapel_1_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Funktionen.ablegen_auf == "hilfsstapel")
                {
                    Funktionen.karte_markieren(Funktionen.ablegen_auf_aktuell, true, "schwarz");
                    Funktionen.ablegen_auf_ausgewählt = false;
                    Funktionen.ablegen_auf = null;
                }
                Zug.ablegen_von_hilfsstapel(sender, Funktionen.hilfsstapel_boxen, Zug.hilfsstapel[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "hilfsstapel";
            }
            if (e.Button == MouseButtons.Right)
            {
                if (Funktionen.ablegen_von == "hilfsstapel")
                {
                    Funktionen.karte_markieren(Funktionen.ablegen_von_aktuell, true, "schwarz");
                    Funktionen.ablegen_von_ausgewählt = false;
                    Funktionen.ablegen_von = null;
                }
                Zug.ablegen_auf_auswählen(sender, Funktionen.hilfsstapel_boxen);
                Funktionen.ablegen_auf = "hilfsstapel";
            }
        }

        private void hilfsstapel_2_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Funktionen.ablegen_auf == "hilfsstapel")
                {
                    Funktionen.karte_markieren(Funktionen.ablegen_auf_aktuell, true, "schwarz");
                    Funktionen.ablegen_auf_ausgewählt = false;
                    Funktionen.ablegen_auf = null;
                }
                Zug.ablegen_von_hilfsstapel(sender, Funktionen.hilfsstapel_boxen, Zug.hilfsstapel[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "hilfsstapel";
            }
            if (e.Button == MouseButtons.Right)
            {
                if (Funktionen.ablegen_von == "hilfsstapel")
                {
                    Funktionen.karte_markieren(Funktionen.ablegen_von_aktuell, true, "schwarz");
                    Funktionen.ablegen_von_ausgewählt = false;
                    Funktionen.ablegen_von = null;
                }
                Zug.ablegen_auf_auswählen(sender, Funktionen.hilfsstapel_boxen);
                Funktionen.ablegen_auf = "hilfsstapel";
            }
        }

        private void hilfsstapel_3_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Funktionen.ablegen_auf == "hilfsstapel")
                {
                    Funktionen.karte_markieren(Funktionen.ablegen_auf_aktuell, true, "schwarz");
                    Funktionen.ablegen_auf_ausgewählt = false;
                    Funktionen.ablegen_auf = null;
                }
                Zug.ablegen_von_hilfsstapel(sender, Funktionen.hilfsstapel_boxen, Zug.hilfsstapel[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "hilfsstapel";
            }
            if (e.Button == MouseButtons.Right)
            {
                if (Funktionen.ablegen_von == "hilfsstapel")
                {
                    Funktionen.karte_markieren(Funktionen.ablegen_von_aktuell, true, "schwarz");
                    Funktionen.ablegen_von_ausgewählt = false;
                    Funktionen.ablegen_von = null;
                }
                Zug.ablegen_auf_auswählen(sender, Funktionen.hilfsstapel_boxen);
                Funktionen.ablegen_auf = "hilfsstapel";
            }
        }

        private void hilfsstapel_4_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Funktionen.ablegen_auf == "hilfsstapel")
                {
                    Funktionen.karte_markieren(Funktionen.ablegen_auf_aktuell, true, "schwarz");
                    Funktionen.ablegen_auf_ausgewählt = false;
                    Funktionen.ablegen_auf = null;
                }
                Zug.ablegen_von_hilfsstapel(sender, Funktionen.hilfsstapel_boxen, Zug.hilfsstapel[Zug.aktuellerSpieler]);
                Funktionen.ablegen_von = "hilfsstapel";
            }
            if (e.Button == MouseButtons.Right)
            {
                if (Funktionen.ablegen_von == "hilfsstapel")
                {
                    Funktionen.karte_markieren(Funktionen.ablegen_von_aktuell, true, "schwarz");
                    Funktionen.ablegen_von_ausgewählt = false;
                    Funktionen.ablegen_von = null;
                }
                Zug.ablegen_auf_auswählen(sender, Funktionen.hilfsstapel_boxen);
                Funktionen.ablegen_auf = "hilfsstapel";
            }
        }
        #endregion
        #region Ablegestapel Mouseclick
        private void ablegestapel_1_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Funktionen.ablegen_auf_auswählen(sender, Funktionen.ablegestapel_boxen);
                Funktionen.ablegen_auf = "ablegestapel";
            }
        }

        private void ablegestapel_2_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Funktionen.ablegen_auf_auswählen(sender, Funktionen.ablegestapel_boxen);
                Funktionen.ablegen_auf = "ablegestapel";
            }
        }

        private void ablegestapel_3_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Funktionen.ablegen_auf_auswählen(sender, Funktionen.ablegestapel_boxen);
                Funktionen.ablegen_auf = "ablegestapel";
            }
        }

        private void ablegestapel_4_box_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Funktionen.ablegen_auf_auswählen(sender, Funktionen.ablegestapel_boxen);
                Funktionen.ablegen_auf = "ablegestapel";
            }
        }

        #endregion

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Funktionen.ablegen_von_ausgewählt && Funktionen.ablegen_auf_ausgewählt)
            {
                (bool spiel_Ende, bool zugMöglich) = Zug.mache_Zug();

                if (!zugMöglich)
                {
                    SoundPlayer sound = new SoundPlayer();
                    sound.SoundLocation = System.IO.Path.Combine(Environment.CurrentDirectory, "Sounds", "wrong.wav"); sound.Load();

                    Funktionen.ablegen_von_aktuell.BackgroundImage = Funktionen.bekomme_image("Back_rot");
                    Funktionen.ablegen_auf_aktuell.BackgroundImage = Funktionen.bekomme_image("Back_rot");

                    sound.Play();

                    Funktionen.ablegen_von_aktuell.Refresh(); Funktionen.ablegen_auf_aktuell.Refresh();
                    System.Threading.Thread.Sleep(500);
                }
                else if (animationen_aktiviert) { Funktionen.karte_Animieren(Funktionen.ablegen_von_aktuell, Funktionen.ablegen_auf_aktuell.Location); }

                if (spiel_Ende) { Szenen.endMenü(false); }

                Zug.aktualisiere_Spielfeld();
            }
        }
        #endregion

        #region End Menü
        private void back_to_menu_button_Click(object sender, EventArgs e)
        {
            click_sound.Play();
            Szenen.spiel(true);
            Szenen.zurücksetzen();
        }
        #endregion

        #region Spieleinstellungen
        private void spieleinstellungen_spieleranzahl_ValueChanged(object sender, EventArgs e)
        {
            int spielerzahl = Convert.ToInt32(Szenen.spieleinstellungen_spielerzahl.Value);

            for (int i = 0; i < spielerzahl; i++) {
                (Label label, TextBox textBox) = Szenen.spieleinstellungen_spielernamen_eingabe[i];

                label.Visible = true; label.Enabled = true;
                textBox.Visible = true; textBox.Enabled = true;

                label.Update(); textBox.Update();
            }
            for (int i = spielerzahl; i < 4; i++)
            {
                (Label label, TextBox textBox) = Szenen.spieleinstellungen_spielernamen_eingabe[i];

                label.Visible = false; label.Enabled = false;
                textBox.Visible = false; textBox.Enabled = false;

                textBox.Text = null;
                label.Update(); textBox.Update();
            }
        }

        private void spieleinstellungen_eingabenBestätigen_button_Click(object sender, EventArgs e)
        {
            click_sound.Play();
            bool eingabe_korrekt = true;
            List<TextBox> leere_Boxen = new List<TextBox>();

            for (int i = 0; i < Szenen.spieleinstellungen_spielerzahl.Value; i++) {
                if (Szenen.spieleinstellungen_spielernamen_eingabe[i].Item2.Text == "") { eingabe_korrekt = false; leere_Boxen.Add(Szenen.spieleinstellungen_spielernamen_eingabe[i].Item2); }
            }

            if (eingabe_korrekt) { Zug.spielvorbereitung(); }
            else
            {
                SoundPlayer sound = new SoundPlayer();
                sound.SoundLocation = System.IO.Path.Combine(Environment.CurrentDirectory, "Sounds", "wrong.wav"); sound.Load();

                Szenen.spieleinstellungen_button.BackColor = System.Drawing.Color.FromName("Red");
                Szenen.spieleinstellungen_button.Refresh();
                foreach (TextBox textBox in leere_Boxen) { textBox.BackColor = System.Drawing.Color.FromName("Red"); textBox.Refresh(); }

                sound.Play();
                System.Threading.Thread.Sleep(500);

                Szenen.spieleinstellungen_button.BackColor = System.Drawing.SystemColors.Control;
                foreach (TextBox textBox in leere_Boxen) { textBox.BackColor = System.Drawing.Color.FromName("White"); textBox.Refresh(); }
            }
        }
        #endregion

        #region Hauptmenü
        private void hauptmenü_spielstart_button_Click(object sender, EventArgs e)
        {
            click_sound.Play();
            Szenen.hauptmenü(true);

            Szenen.spieleinstellungen_panel.Visible = true; Szenen.spieleinstellungen_panel.Enabled = true;
            Szenen.spieleinstellungen_panel.Refresh();
        }

        private void hauptmenü_optionen_button_Click(object sender, EventArgs e)
        {
            click_sound.Play();
            Szenen.optionen(false);
        }

        private void hauptmenü_spielBeenden_button_Click(object sender, EventArgs e)
        {
            click_sound.Play();
            Application.Exit();
        }
        #endregion

        #region Optionen
        private void optionen_button_Click(object sender, EventArgs e)
        {
            click_sound.Play();
            Szenen.optionen(true);
        }

        private void optionen_lautstärke_trackbar_ValueChanged(object sender, EventArgs e)
        {
            int value = optionen_lautstärke_trackbar.Value;
            musik.Volume = (double)value / 10;
        }

        private void optionen_musik_button_Click(object sender, EventArgs e)
        {
            click_sound.Play();
            if (animationen_aktiviert) {
                animationen_aktiviert = false;
                Szenen.optionen_animationen_button.BackColor = System.Drawing.Color.FromName("Red");
                Szenen.optionen_animationen_button.Text = "Aus";
            }
            else {
                animationen_aktiviert = true;
                Szenen.optionen_animationen_button.BackColor = System.Drawing.Color.FromName("LawnGreen");
                Szenen.optionen_animationen_button.Text = "An";
            }
            Szenen.optionen_animationen_button.Refresh();
        }
        #endregion

        private void Musik_MediaEnded(object sender, EventArgs e)
        {
            musik.Position = TimeSpan.Zero;
            musik.Play();
        }
    }
}

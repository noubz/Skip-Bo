using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace Skip_Bo
{
    class Szenen : Form1
    {
        // Variable für die aktuelle_Szene
        public static string aktuelle_Szene { get; set; }


        // HAUPTMENÜ
        public static Panel hauptmenü_panel { get; set; }
        public static void hauptmenü(bool verschwinden)
        {
            if (verschwinden) {
                hauptmenü_panel.Visible = false;
                hauptmenü_panel.Enabled = false;
                hauptmenü_panel.Refresh();
            }
            else {
                hauptmenü_panel.Visible = true; hauptmenü_panel.Enabled = true;
                hauptmenü_panel.Refresh();
            }
        }

        // OPTIONEN
        public static Panel optionen_panel { get; set; }
        public static TrackBar optionen_musik_trackBar { get; set; }
        public static Button optionen_animationen_button { get; set; }
        public static void optionen(bool verschwinden)
        {
            if (verschwinden)
            {
                for (int i = 787; i < 1200; i += 20)
                {
                    optionen_panel.Location = new Point(i, 0);
                    optionen_panel.Refresh();
                    hauptmenü_panel.Refresh();
                }
                optionen_panel.Location = new Point(1200, 0);
                optionen_panel.Enabled = false;
                optionen_panel.Refresh();

                hauptmenü(false);
            }
            else
            {
                hauptmenü_panel.Enabled = false; hauptmenü_panel.Refresh();
                optionen_panel.Enabled = true;

                for (int i = 1200; i > 786; i -= 10)
                {
                    optionen_panel.Location = new Point(i, 0);
                    optionen_panel.Refresh();
                }
            }
        }

        // SPIEL
        public static void spiel(bool verschwinden)
        {
            if (verschwinden) {
                Funktionen.spielerstapel_box.Enabled = false;
                foreach (PictureBox karte in Funktionen.handkarten_boxen) { karte.Enabled = false;}
                foreach (PictureBox karte in Funktionen.hilfsstapel_boxen) { karte.Enabled = false; }
                foreach (PictureBox karte in Funktionen.ablegestapel_boxen) { karte.Enabled = false; }
            }
            else {
                Funktionen.spielerstapel_box.Enabled = true;
                foreach (PictureBox karte in Funktionen.handkarten_boxen) { karte.Enabled = true; }
                foreach (PictureBox karte in Funktionen.hilfsstapel_boxen) { karte.Enabled = true; }
                foreach (PictureBox karte in Funktionen.ablegestapel_boxen) { karte.Enabled = true; }
            }
        }

        // ENDMENÜ
        public static Panel end_menü { get; set; }
        public static Label end_menü_label { get; set; }
        public static void endMenü(bool verschwinden)
        {
            if (verschwinden) {
                end_menü.Visible = false; end_menü.Enabled = false;
                end_menü.Location = new Point(1200, end_menü.Location.Y);
                end_menü.Refresh();

                hauptmenü(false);
            }
            else {
                musik.Stop();
                SoundPlayer gewinn_sound = new SoundPlayer();
                gewinn_sound.SoundLocation = System.IO.Path.Combine(Environment.CurrentDirectory, "Sounds", "Gewinn_sound.wav"); gewinn_sound.Load();
                gewinn_sound.Play();

                spiel(true);

                end_menü.Visible = true; end_menü.Enabled = true;
                end_menü_label.Text = Zug.spielernamen[Zug.aktuellerSpieler];
                int Pos_y = end_menü.Location.Y;

                for (int i = 1200; i > -1; i -= 30)
                {
                    end_menü.Location = new Point(i, Pos_y);
                    end_menü.Refresh();
                }
            }
        }

        // SPIELEINSTELLUNGEN
        public static Panel spieleinstellungen_panel { get; set; }
        public static NumericUpDown spieleinstellungen_spielerstapel { get; set; }
        public static NumericUpDown spieleinstellungen_spielerzahl { get; set; }
        public static Button spieleinstellungen_button { get; set; }
        public static List<Tuple<Label,TextBox>> spieleinstellungen_spielernamen_eingabe { get; set; }
        public static void spieleinstellungen()
        {
            List<string> spielernamen = new List<string>();
            for (int i = 0; i < spieleinstellungen_spielerzahl.Value; i++) { spielernamen.Add(spieleinstellungen_spielernamen_eingabe[i].Item2.Text); }

            Zug.anzahl_Karten = Convert.ToInt32(spieleinstellungen_spielerstapel.Value);
            Zug.spielernamen = spielernamen;

            spieleinstellungen_panel.Visible = false;
            spieleinstellungen_panel.Enabled = false;

            spiel(false);
        }

        // ZURÜCKSETZEN
        public static Panel zurücksetzen_panel { get; set; }
        public static ProgressBar zurücksetzen_progressBar { get; set; }
        public static Label zurücksetzen_laden_label { get; set; }
        public static void zurücksetzen()
        {
            endMenü(true);

            zurücksetzen_panel.Visible = true; zurücksetzen_panel.Enabled = true;
            zurücksetzen_panel.Location = new Point(0, 0);
            zurücksetzen_panel.Refresh();


            Thread thread_ladeanimation = new Thread(new ThreadStart(label_animation));
            Thread zurücksetzen = new Thread(new ThreadStart(thread_zurücksetzen));

            thread_ladeanimation.Start();
            zurücksetzen.Start();

            zurücksetzen.Join();

            zurücksetzen_panel.Visible = false; zurücksetzen_panel.Enabled = false;
            zurücksetzen_panel.Location = new Point(0, 863);
            zurücksetzen_panel.Refresh();

            thread_ladeanimation.Abort(); zurücksetzen_laden_label.Text = ".";
            zurücksetzen_progressBar.Value = 0;

            musik.Position = TimeSpan.Zero;
            musik.Play();

            hauptmenü(false);
        }
        
        public static void thread_zurücksetzen()
        {
            Thread.Sleep(500);

            zurücksetzen_progressBar.Value = 10;
            Zug.spielernamen = new List<string>();
            Zug.hilfsstapel = new List<List<List<string>>>();
            Zug.spielerstapel = new List<List<string>>(); 
            Zug.spielerhand = new List<List<string>>(); 

            Thread.Sleep(100);
            zurücksetzen_progressBar.Update();

            zurücksetzen_progressBar.Value = 20;
            Zug.aktuellerSpieler = 0;
            Zug.anzahl_Karten = 0;

            Thread.Sleep(100);
            zurücksetzen_progressBar.Update();

            zurücksetzen_progressBar.Value = 34;
            Funktionen.kartenstock = new List<string>();
            Funktionen.ablegestapel = null;

            Thread.Sleep(1000);
            zurücksetzen_progressBar.Update();

            Funktionen.spielerstapel_box.Image = null; zurücksetzen_progressBar.Value = 39;
            zurücksetzen_progressBar.Value = 54;
            foreach (PictureBox karte in Funktionen.handkarten_boxen) { karte.Image = null; karte.Visible = true; karte.Enabled = true; }
            zurücksetzen_progressBar.Value = 70;
            foreach (PictureBox karte in Funktionen.hilfsstapel_boxen) { karte.Image = null; }
            foreach (PictureBox karte in Funktionen.ablegestapel_boxen) { karte.Image = null; }
            foreach (Label nummer in Zug.ablegestapel_anzeigen) { nummer.Visible = false; }

            Thread.Sleep(1000);
            zurücksetzen_progressBar.Update();

            Funktionen.infobox_box.Text = "player"; zurücksetzen_progressBar.Value = 71;
            foreach (Tuple<Label,PictureBox> tuple in Zug.spielerstapel_anzeige) {
                tuple.Item1.Text = null;
                tuple.Item2.Image = null;
            }
            zurücksetzen_progressBar.Value = 80;

            spieleinstellungen_spielerstapel.Value = 30;
            spieleinstellungen_spielerzahl.Value = 2;

            spieleinstellungen_spielernamen_eingabe[0].Item2.Text = null;
            spieleinstellungen_spielernamen_eingabe[1].Item2.Text = null;

            Thread.Sleep(100);
            zurücksetzen_progressBar.Update();

            zurücksetzen_progressBar.Value = 100;
            for (int i = 2; i < 4; i++)
            {
                (Label label, TextBox textBox) = spieleinstellungen_spielernamen_eingabe[i];

                label.Visible = false; label.Enabled = false;
                textBox.Visible = false; textBox.Enabled = false;

                textBox.Text = null;
            }

            Thread.Sleep(2000);
            zurücksetzen_progressBar.Update();
        }

        public static void label_animation()
        {
            zurücksetzen_laden_label.Text = null;

            for (int i = 0; i < 10; i++) {
                zurücksetzen_laden_label.Text = "."; zurücksetzen_laden_label.Refresh();
                Thread.Sleep(250);
                zurücksetzen_laden_label.Text = ".."; zurücksetzen_laden_label.Refresh();
                Thread.Sleep(250);
                zurücksetzen_laden_label.Text = "..."; zurücksetzen_laden_label.Refresh();
                Thread.Sleep(250);
            }
        }
    }
}

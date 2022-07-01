using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Skip_Bo
{
    class Zug : Funktionen
    {
        // Generelle Variablen definieren
        public static List<string> spielernamen = new List<string>();
        public static List<List<List<string>>> hilfsstapel = new List<List<List<string>>>();
        public static List<List<string>> spielerstapel = new List<List<string>>();
        public static List<List<string>> spielerhand = new List<List<string>>();

        public static List<Tuple<Label,PictureBox>> spielerstapel_anzeige = new List<Tuple<Label,PictureBox>>();
        public static Label übrige_spielerstapelkarten = new Label();

        public static List<Label> ablegestapel_anzeigen = new List<Label>();

        public static int aktuellerSpieler = 0;
        public static int anzahl_Karten = 0;

        // Funktion zur Vorbereitung des Spiels
        public static void spielvorbereitung()
        {
            // Variablen spielernamen und anzahl_Karten bekommen
            Szenen.spieleinstellungen();

            kartenstock = generiere_Kartendeck(); // Kartenstock erstellen
            ablegestapel = new List<List<string>>() { new List<string>() { "Leer" }, new List<string>() { "Leer" }, new List<string>() { "Leer" }, new List<string>() { "Leer" } };

            // Schleife, um für jeden Spieler "Spielstapel" zu erstellen
            for (int i = 0; i < spielernamen.Count(); i++)
            {
                hilfsstapel.Add(new List<List<string>>() { new List<string>() { "Leer" }, new List<string>() { "Leer" }, new List<string>() { "Leer" }, new List<string>() { "Leer" } });
                spielerstapel.Add(erstelle_Stapel(anzahl_Karten));
                spielerhand.Add(erstelle_Stapel(5));
            }

            // Erstes Mal die Bilder auf die PictureBoxen bringen
            spielerstapel_box.Image = bekomme_image(spielerstapel[aktuellerSpieler][0]);
            for (int i = 0; i < 5; i++) {
                handkarten_boxen[i].Image = bekomme_image(spielerhand[aktuellerSpieler][i]);
            }
            for (int i = 0; i < 4; i++) {
                hilfsstapel_boxen[i].Image = bekomme_image(hilfsstapel[aktuellerSpieler][i][0]);
                ablegestapel_boxen[i].Image = bekomme_image(ablegestapel[i][0]);
            }

            // Text des Spielerstapels anpassen
            übrige_spielerstapelkarten.Text = anzahl_Karten + " übrig";

            // Text der GroupBox auf den aktuellen Spieler anpassen
            infobox_box.Text = spielernamen[aktuellerSpieler];

            // Spielerstapelanzeige anpassen
            for (int i = 1; i < spielernamen.Count(); i++) {
                (Label spieler, PictureBox karte) = spielerstapel_anzeige[i - 1];

                spieler.Text = spielernamen[i] + " (" + anzahl_Karten + ")";
                karte.Image = bekomme_image(spielerstapel[i][0]);
            }
            for (int i = spielernamen.Count(); i < 4; i++) {
                (Label label, PictureBox picBox) = spielerstapel_anzeige[i - 1];
                label.Visible = false; picBox.Visible = false;
            }
        }

        // Funktion, die einen Spielerzug verarbeitet
        public static Tuple<bool, bool> mache_Zug()
        {
            bool spiel_Ende = false;
            (bool zug_Ende, bool zugMöglich) = karte_Ablegen(); // Versuch die ausgewählte Karte abzulegen

            // Jeden Ablegestapel/Hilfsstapel durchgehen und prüfen, ob er Leer ist/geleert werden muss
            foreach (int i in Enumerable.Range(0, 4))
            {
                if (ablegestapel[i].Count() == 12)
                {
                    ablegestapel[i].Clear();
                    ablegestapel[i].Add("Leer");
                }
                if (hilfsstapel[aktuellerSpieler][i].Count() == 0) { hilfsstapel[aktuellerSpieler][i].Add("Leer"); }
            }

            if (spielerstapel[aktuellerSpieler].Count() == 0) { spiel_Ende = true; } // Falls der aktuelle Spieler keine Karten mehr hat, Gewinn-Screen anzeigen

            else 
            {
                // Falls der Zug des Spielers beendet ist:
                if (zug_Ende)
                {
                    int auffuellKarten = 5 - spielerhand[aktuellerSpieler].Count();
                    ergaenze_Stapel(spielerhand[aktuellerSpieler], auffuellKarten); // Handkarten nachfüllen

                    // Nächster Spieler
                    if (aktuellerSpieler == (spielernamen.Count() - 1)) { aktuellerSpieler = 0; }
                    else { aktuellerSpieler++; }
                }

                // Handkarten des aktuellen Spielers auffüllen, falls leer
                else if (spielerhand[aktuellerSpieler].Count() == 0) { ergaenze_Stapel(spielerhand[aktuellerSpieler], 5); }
            }

            return new Tuple<bool, bool>(spiel_Ende, zugMöglich);
        }

        // Funktion zur Aktualisierung des Windows Forms
        public static void aktualisiere_Spielfeld()
        {
            // Auswahl zurücksetzen
            karte_markieren(ablegen_von_aktuell, true, "schwarz");
            karte_markieren(ablegen_auf_aktuell, true, "schwarz");
            ablegen_von_ausgewählt = false; ablegen_auf_ausgewählt = false;

            // Animations-Karte zurücksetzen
            animationsKarte.Image = null;
            animationsKarte.Location = new System.Drawing.Point(1187, 863);

            // Hilfsstapel und Ablegestapel
            for (int i = 0; i < 4; i++) {
                hilfsstapel_boxen[i].Image = bekomme_image(hilfsstapel[aktuellerSpieler][i][0]);
                ablegestapel_boxen[i].Image = bekomme_image(ablegestapel[i][0]);
                if (ablegestapel[i][0] == "skip-bo") {
                    ablegestapel_anzeigen[i].Visible = true; ablegestapel_anzeigen[i].Enabled = true;
                    ablegestapel_anzeigen[i].Text = ablegestapel[i].Count().ToString();
                }
                else { ablegestapel_anzeigen[i].Visible = false; ablegestapel_anzeigen[i].Enabled = false; }

                hilfsstapel_boxen[i].Refresh(); ablegestapel_boxen[i].Refresh(); ablegestapel_anzeigen[i].Refresh();
            }

            // Spielerstapel
            if (spielerstapel[aktuellerSpieler].Count() == 0) { spielerstapel_box.Image = bekomme_image("Leer"); }
            else { spielerstapel_box.Image = bekomme_image(spielerstapel[aktuellerSpieler][0]); spielerstapel_box.Refresh(); } 

            // Vorhandene Handkarten
            for (int i = 0; i < spielerhand[aktuellerSpieler].Count(); i++)
            {
                handkarten_boxen[i].Image = bekomme_image(spielerhand[aktuellerSpieler][i]);
                handkarten_boxen[i].Visible = true;
                handkarten_boxen[i].Enabled = true;

                handkarten_boxen[i].Refresh();
            }
            // Nicht vorhandende Handkarten
            for (int i = spielerhand[aktuellerSpieler].Count(); i < 5; i++)
            {
                handkarten_boxen[i].Visible = false;
                handkarten_boxen[i].Enabled = false;

                handkarten_boxen[i].Refresh();
            }

            // Text des Spielerstapels anpassen
            übrige_spielerstapelkarten.Text = spielerstapel[aktuellerSpieler].Count() + " übrig";

            // Text der GroupBox auf den aktuellen Spieler anpassen
            infobox_box.Text = spielernamen[aktuellerSpieler];

            // Spielerstapelanzeige anpassen
            int temp_spieler = aktuellerSpieler;
            for (int i = 0; i < spielernamen.Count() - 1; i++)
            {
                if (temp_spieler == spielernamen.Count() - 1) { temp_spieler = -1; }
                temp_spieler++;

                (Label spieler, PictureBox karte) = spielerstapel_anzeige[i];

                spieler.Text = spielernamen[temp_spieler] + " (" + spielerstapel[temp_spieler].Count().ToString() + ")";
                karte.Image = bekomme_image(spielerstapel[temp_spieler][0]);
            }
        }
    }
}

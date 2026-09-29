using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PracticaNetflix_A1_i_A2.MODEL
{
    public class RawTitle:IComparable<RawTitle>
    {
        private int index;
        private string id;
        private string title;
        private string type;
        private string releaseYear;
        private string[] genres;
        private double seasons;
        private double imdbScore;
        private double imdbVotes;
        public RawTitle() { }

        public RawTitle(int index, string id, string title, string type, string releaseYear, string[] genres, double seasons, double imdbScore, double imdbVotes)
        {
            this.Index = index;
            this.Id = id;
            this.Title = title;
            this.Type = type;
            this.ReleaseYear = releaseYear;
            this.Genres = genres;
            this.Seasons = seasons;
            this.ImdbScore = imdbScore;
            this.ImdbVotes = imdbVotes;
        }

        public int Index { get => index; set => index = value; }
        public string Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Type { get => type; set => type = value; }
        public string ReleaseYear { get => releaseYear; set => releaseYear = value; }
        public string[] Genres { get => genres; set => genres = value; }
        public double Seasons { get => seasons; set => seasons = value; }
        public double ImdbScore { get => imdbScore; set => imdbScore = value; }
        public double ImdbVotes { get => imdbVotes; set => imdbVotes = value; }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"[");
            for (int i = 0; i < Genres.Length; i++)
            {
                sb.Append(Genres[i]);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]\"");
            return $"{Index},{Id},{Title},{Type},{ReleaseYear},{sb.ToString()},{Seasons.ToString(CultureInfo.InvariantCulture)},{ImdbScore.ToString(CultureInfo.InvariantCulture)},{ImdbVotes.ToString(CultureInfo.InvariantCulture)}";
        }

        public override bool Equals(object? obj)
        {
            bool resultat = false;
            if (obj == null)
                resultat = false;
            else if (obj.GetType() != this.GetType())
                resultat = false;
            else
                resultat = Id == ((RawTitle)obj).Id;
            return resultat;
        }

        public int CompareTo(RawTitle altre)
        {
            return altre.ImdbScore.CompareTo(this.ImdbScore);
        }
    }
}
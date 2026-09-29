using PracticaNetflix_A1_i_A2.MODEL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Shapes;

namespace PracticaNetflix_A1_i_A2.DAO
{
    public class DAOCSVImpl : IDAO
    {
        public const string FILENAME = "raw_titles.csv";
        private string RUTA_CSV = System.IO.Path.Combine(AppContext.BaseDirectory, FILENAME);
        private const string PATTERN = @",(?=(?:[^""]*""[^""]*"")*[^""]*$)";
        private const int CAMP_INDEX = 0;
        private const int CAMP_ID = 1;
        private const int CAMP_TITLE = 2;
        private const int CAMP_TYPE = 3;
        private const int CAMP_RELEASE_YEAR = 4;
        private const int CAMP_GENRES = 7;
        private const int CAMP_SEASONS = 9;
        private const int CAMP_IMDB_SCORE = 11;
        private const int CAMP_IMDB_VOTE = 12;
        private const int CAMP_IMDB_SCORE_NEW = 7;

        public DAOCSVImpl()
        {

        }

        //Exercici 1: Genera un fitxer de text (outputFile) que conté només registres del gènere especificat. El fitxer
        //ha de ser un CSV amb un registre per linia amb els següents camps: Retorna el nombre de
        //registres trobats. index;id;title;genres  Quin problema us trobeu amb els genres?

        //Tenim , dins de "", hem de utilitzar un regex al split
        public int SelectByGenre(string genre, string outputFile) //USAREM ESQUEMA U-NK perquè genre no és clau única i no està ordenat -> RECORREGUT
        {
            int resultat = 0;
            string i2S = $"'{genre.Trim().ToLower()}'"; //busquem 'drama' perque es com esta al csv
            string rutaSortida = System.IO.Path.Combine(AppContext.BaseDirectory, outputFile);

            using (StreamReader sr = new StreamReader(RUTA_CSV, Encoding.UTF8))
            {
                using (StreamWriter sw = new StreamWriter(rutaSortida))
                {
                    sw.WriteLine("index;id;title;genres"); //Escrivim la capçalera al fitxer de sortida

                    string linia = sr.ReadLine(); //llegim capçalera
                    linia = sr.ReadLine();
                    while (linia != null)           // recorregut: només acaba al final del fitxer
                    {
                        string[] camps = Regex.Split(linia, PATTERN);

                        string[] generes = camps[CAMP_GENRES].Trim('"', '[', ']').Split(','); //eliminiem els [] i "" i fem split per , i ja tenim els gèneres en un array de strings

                        // Cerca dins dels gèneres d'aquest registre
                        bool trobat = false;
                        int i = 0;
                        while (i < generes.Length && !trobat)
                        {
                            if (generes[i].ToLower() == i2S) // actual == 'drama' ?
                                trobat = true;
                            else
                                i++;
                        }

                        // Si trobat, escrivim al fitxer de sortida la peli
                        if (trobat)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append($"{camps[CAMP_INDEX]};{camps[CAMP_ID]};{camps[CAMP_TITLE]};{camps[CAMP_GENRES]}");
                            sw.WriteLine(sb.ToString());
                            resultat++;
                        }
                        linia = sr.ReadLine();
                    }
                }
            }
            return resultat;
        }

        //Exercici 2: Retorna tot el RawTitle amb el index indicat per el paràmetre ( índex és el primer camp).
        //Retorna null si l’ índex no s’ha trobat.

        public RawTitle SelectByIndex(int index) //USAREM ESQUEMA S-K PERQUE ES LA CLAU I ESTA ORDENADA -> CERCA
        {
            RawTitle resultat = null;
            bool trobat = false;
            bool passat = false;
            string linia;
            int indexActual;
            using (StreamReader sr = new StreamReader(RUTA_CSV, Encoding.UTF8))
            {
                string[] camps = null;
                linia = sr.ReadLine(); //legim capçalera
                linia = sr.ReadLine();
                while (linia != null && !trobat && !passat)
                {
                    camps = Regex.Split(linia, PATTERN);
                    indexActual = Convert.ToInt32(camps[CAMP_INDEX]);

                    if (index == indexActual)
                    {
                        trobat = true;
                    }
                    else if (index < indexActual)
                    {
                        passat = true;
                    }
                    else
                    {
                        linia = sr.ReadLine();
                    }
                }
                if (trobat)//si trobem el rawtitle, convertim les dades del registre a objecte RawTitle, els camps seasons i imdb a vegades estan buits i hem de fer una comprovacio, usarem la ternaria
                {
                    double seasons = (camps[CAMP_SEASONS] == "") ? 0 : Convert.ToDouble(camps[CAMP_SEASONS], CultureInfo.InvariantCulture);
                    double imdbScore = (camps[CAMP_IMDB_SCORE] == "") ? 0 : Convert.ToDouble(camps[CAMP_IMDB_SCORE], CultureInfo.InvariantCulture);
                    double imdbVotes = (camps[CAMP_IMDB_VOTE] == "") ? 0 : Convert.ToDouble(camps[CAMP_IMDB_VOTE], CultureInfo.InvariantCulture);
                    string[] generes = camps[CAMP_GENRES].Trim('"', '[', ']').Split(',');

                    resultat = new RawTitle(Convert.ToInt32(camps[CAMP_INDEX]),
                                            camps[CAMP_ID],
                                            camps[CAMP_TITLE],
                                            camps[CAMP_TYPE],
                                            camps[CAMP_RELEASE_YEAR],
                                            generes,
                                            seasons,
                                            imdbScore,
                                            imdbVotes
                                            );
                }
            }
            return resultat;
        }

        //Exercici 3: Retorna el RawTitle amb l’id especificat en el paràmetre. Retorna null si l’id no està present.
        //Id és el segon camp.
        public RawTitle SelectById(string id) //USAREM ESQUEMA U-K perquè ID és clau única i no està ordenat -> CERCA
        {
            RawTitle resultat = null;
            bool trobat = false;
            bool passat = false;
            string linia;
            string idActual;
            using (StreamReader sr = new StreamReader(RUTA_CSV, Encoding.UTF8))
            {
                string[] camps = null;
                linia = sr.ReadLine(); //legim capçalera
                linia = sr.ReadLine();
                while (linia != null && !trobat)
                {
                    camps = Regex.Split(linia, PATTERN);
                    idActual = camps[CAMP_ID];

                    if (id == idActual)
                    {
                        trobat = true;
                    }
                    else
                    {
                        linia = sr.ReadLine();
                    }
                }
                if (trobat)
                {
                    double seasons = (camps[CAMP_SEASONS] == "") ? 0 : Convert.ToDouble(camps[CAMP_SEASONS], CultureInfo.InvariantCulture);
                    double imdbScore = (camps[CAMP_IMDB_SCORE] == "") ? 0 : Convert.ToDouble(camps[CAMP_IMDB_SCORE], CultureInfo.InvariantCulture);
                    double imdbVotes = (camps[CAMP_IMDB_VOTE] == "") ? 0 : Convert.ToDouble(camps[CAMP_IMDB_VOTE], CultureInfo.InvariantCulture);
                    string[] generes = camps[CAMP_GENRES].Trim('"', '[', ']').Split(',');

                    resultat = new RawTitle(Convert.ToInt32(camps[CAMP_INDEX]),
                                            camps[CAMP_ID],
                                            camps[CAMP_TITLE],
                                            camps[CAMP_TYPE],
                                            camps[CAMP_RELEASE_YEAR],
                                            generes,
                                            seasons,
                                            imdbScore,
                                            imdbVotes
                                            );
                }
            }
            return resultat;
        }

        //Exercici 4: A partir de l’índex i el nombre de registres desitjats ( length ) llegeix tots els RawTitles trobats
        //a partir de l’index fins a index+length(com a màxim). Pot ser que n’hi hagin menys.Al final si es
        //dona aquest cas, cal reajustar el tamany de l’array.

        public RawTitle[] ReadTitles(int index, int length)
        {
            List<RawTitle> llistaPelis = new List<RawTitle>();
            RawTitle peli = null;

            bool trobat = false;
            bool passat = false;
            string linia;
            int indexActual;
            using (StreamReader sr = new StreamReader(RUTA_CSV, Encoding.UTF8))
            {
                string[] camps = null;
                linia = sr.ReadLine(); //legim capçalera
                linia = sr.ReadLine();
                while (linia != null && !trobat && !passat) //primer bucle per trobar index o si no existeix doncs el seguent o bé acabem el fitxer, usem esquema S-K
                {
                    camps = Regex.Split(linia, PATTERN);
                    indexActual = Convert.ToInt32(camps[CAMP_INDEX]);

                    if (index == indexActual)
                    {
                        trobat = true;
                    }
                    else if (index < indexActual)
                    {
                        passat = true;
                    }
                    else
                    {
                        linia = sr.ReadLine();
                    }
                }
                while (linia != null && llistaPelis.Count <= length) //segon bucle per afegir els rawtitles desde index fins index+length a la llista
                {
                    camps = Regex.Split(linia, PATTERN);

                    double seasons = (camps[CAMP_SEASONS] == "") ? 0 : Convert.ToDouble(camps[CAMP_SEASONS], CultureInfo.InvariantCulture);
                    double imdbScore = (camps[CAMP_IMDB_SCORE] == "") ? 0 : Convert.ToDouble(camps[CAMP_IMDB_SCORE], CultureInfo.InvariantCulture);
                    double imdbVotes = (camps[CAMP_IMDB_VOTE] == "") ? 0 : Convert.ToDouble(camps[CAMP_IMDB_VOTE], CultureInfo.InvariantCulture);
                    string[] generes = camps[CAMP_GENRES].Trim('"', '[', ']').Split(',');

                    peli = new RawTitle(Convert.ToInt32(camps[CAMP_INDEX]),
                                            camps[CAMP_ID],
                                            camps[CAMP_TITLE],
                                            camps[CAMP_TYPE],
                                            camps[CAMP_RELEASE_YEAR],
                                            generes,
                                            seasons,
                                            imdbScore,
                                            imdbVotes
                                            );
                    llistaPelis.Add(peli);
                    linia = sr.ReadLine();
                }
            }
            return llistaPelis.ToArray();
        }

        //Exercici5: Escriu en el fitxer outputFileName tots els RawTitles de l’array ordenats per imdb_score.
        public void PreMerge(RawTitle[] titles, string outputFileName)
        {
            string rutaSortida = System.IO.Path.Combine(AppContext.BaseDirectory, outputFileName);
            Array.Sort(titles);
            using (StreamWriter sw = new StreamWriter(rutaSortida))
            {
                for (int i = 0; i < titles.Length; i++)
                {
                    sw.WriteLine(titles[i].ToString());
                }
            }
        }

        //Exercici 6:inputFileName1 i inputFilename2 són fitxers d’entrada ordenats per imdb_score. El mètode
        //ha de fusionar els dos fitxers generant un tercer fitxer amb tots els registres, conservant l’ordre
        //per imdb_score - El mètode retorna el total de registres generats en el fitxer de sortida.

        public int Merge(string inputFileName1, string inputFileName2, string outputFileName)
        {
            int resultat = 0;
            string rutaF1 = System.IO.Path.Combine(AppContext.BaseDirectory, inputFileName1);
            string rutaF2 = System.IO.Path.Combine(AppContext.BaseDirectory, inputFileName2);
            string rutaSortida = System.IO.Path.Combine(AppContext.BaseDirectory, outputFileName);
            StreamReader sr1 = new StreamReader(rutaF1, Encoding.UTF8);
            StreamReader sr2 = new StreamReader(rutaF2, Encoding.UTF8);
            string[] campsF1 = null;
            string[] campsF2 = null;
            double imdbScoreF1 = 0;
            double imdbScoreF2 = 0;
            string liniaF1 = sr1.ReadLine();
            string liniaF2 = sr2.ReadLine();
            using (StreamWriter sw = new StreamWriter(rutaSortida))
            {
                sw.WriteLine("index,id,title,type,release_year,genres,seasons,imdb_score,imdb_votes");
                while (liniaF1 != null && liniaF2 != null)
                {
                    campsF1 = Regex.Split(liniaF1, PATTERN);
                    campsF2 = Regex.Split(liniaF2, PATTERN);
                    imdbScoreF1 = Convert.ToDouble(campsF1[CAMP_IMDB_SCORE_NEW]);
                    imdbScoreF2 = Convert.ToDouble(campsF2[CAMP_IMDB_SCORE_NEW]);

                    if (imdbScoreF1 > imdbScoreF2)
                    {
                        sw.WriteLine(liniaF1);
                        liniaF1 = sr1.ReadLine();
                        resultat++;
                    }
                    else if (imdbScoreF1 == imdbScoreF2)
                    {
                        sw.WriteLine(liniaF1);
                        sw.WriteLine(liniaF2);
                        liniaF1 = sr1.ReadLine();
                        liniaF2 = sr2.ReadLine();
                        resultat += 2;
                    }
                    else
                    {
                        sw.WriteLine(liniaF2);
                        liniaF2 = sr2.ReadLine();
                        resultat++;
                    }
                }
                while (liniaF1 != null)
                {
                    sw.WriteLine(liniaF1);
                    liniaF1 = sr1.ReadLine();
                    resultat++;
                }
                while (liniaF2 != null)
                {
                    sw.WriteLine(liniaF2);
                    liniaF2 = sr2.ReadLine();
                    resultat++;
                }
            }

            sr1.Close();
            sr2.Close();
            return resultat;
        }
    }
}

using Microsoft.Win32;
using PracticaNetflix_A1_i_A2.MODEL;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PracticaNetflix_A1_i_A2.DAO
{
    public interface IDAO
    {
        //Exercici 1: Genera un fitxer de text (outputFile) que conté només registres del gènere especificat. El fitxer
        //ha de ser un CSV amb un registre per linia amb els següents camps: Retorna el nombre de
        //registres trobats. index;id;title;genres  Quin problema us trobeu amb els genres?

        //Tenim , dins de "", hem de utilitzar un regex al split
        int SelectByGenre(string genre, string outputFile);

        //Exercici 2: Retorna tot el RawTitle amb el index indicat per el paràmetre ( índex és el primer camp).
        //Retorna null si l’ índex no s’ha trobat.

        RawTitle SelectByIndex(int index);

        //Exercici 3: Retorna el RawTitle amb l’id especificat en el paràmetre. Retorna null si l’id no està present.
        //Id és el segon camp.
        RawTitle SelectById(string id);

        //Exercici 4: A partir de l’índex i el nombre de registres desitjats ( length ) llegeix tots els RawTitles trobats
        //a partir de l’index fins a index+length(com a màxim). Pot ser que n’hi hagin menys.Al final si es
        //dona aquest cas, cal reajustar el tamany de l’array.

        RawTitle[] ReadTitles(int index, int length);

        //Exercici5: Escriu en el fitxer outputFileName tots els RawTitles de l’array ordenats per imdb_score.
        void PreMerge(RawTitle[] titles, string outputFileName);

        //Exercici 6:inputFileName1 i inputFilename2 són fitxers d’entrada ordenats per imdb_score. El mètode
        //ha de fusionar els dos fitxers generant un tercer fitxer amb tots els registres, conservant l’ordre
        //per imdb_score - El mètode retorna el total de registres generats en el fitxer de sortida.

        int Merge(string inputFileName1, string inputFilename2, string outputFileName);
    }
}

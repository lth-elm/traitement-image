using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace EL_MERSHATI_Laith___Projet_Info_A2
{
    class Program
    {
        /// <summary>
        /// Méthode permettant de sélectionner une image parmi celle connu, ou bien d'en choisir une à partir de son nom. 
        /// Donne aussi la possibilité à l'utilisateur d'avoir un aperçu
        /// </summary>
        /// <returns> Un string qui représente le nom du fichier afin de récupérer ses caractéristiques </returns>
        static string Selection()
        {
            Console.WriteLine("Veuillez choisir une image parmi les suivante, ou bien entrez le nom du fichier après l'avoir déposé dans >bin>Debug. " +
                "\n" +
                "\n0. Entrer un nom de fichier" +
                "\n1. Coco" +
                "\n2. Lac en montagne" +
                "\n3. Lena" +
                "\n4. Test" +
                "\n5. Dessiner la fractale de Mandelbrot\n");
            int numeroImage = -1;
            do
            {
                Console.Write("> ");
                try { numeroImage = Convert.ToInt32(Console.ReadLine()); } // On vérifie si c'est bien un entier qui a été entré
                catch { Console.Write("Entrez un numéro valide\n"); }  
            } while (numeroImage != 0 && numeroImage != 1 && numeroImage != 2 && numeroImage != 3 && numeroImage != 4 && numeroImage != 5);

            string filename = null;
            switch (numeroImage)
            {
                case 0:
                    byte[] existence = null;
                    do
                    {
                        Console.Write("Entrer le nom du fichier : ");
                        string fichier = Convert.ToString(Console.ReadLine());
                        filename = fichier + ".bmp";
                        try { existence = File.ReadAllBytes(filename); } // Pour vérifier si c'est bien une image on essaye de la lire en remplissant le tableau de byte "existence"
                        catch { Console.WriteLine("Saisissez un nom d'image existant."); }
                    } while (existence == null);
                    break;
                case 1:
                    filename = "coco.bmp";
                    break;
                case 2:
                    filename = "lac_en_montagne.bmp";
                    break;
                case 3:
                    filename = "lena.bmp";
                    break;
                case 4:
                    filename = "Test.bmp";
                    break;
                case 5:
                    MyImage image = new MyImage();
                    image.fractale(); // On dessine tout de suite la fractale, pas besoin de passer par d'autre étapes
                    Environment.Exit(0);
                    break;
            }
            Console.WriteLine("\n\nVoulez ouvrir cette image ?\n1. Oui\n2. Non\n");
            int ouvrir = -1;
            do
            {
                Console.Write("> ");
                try { ouvrir = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("Entrez un numéro valide\n"); }
            } while (ouvrir != 1 && ouvrir != 2);

            if (ouvrir == 1)
            {
                Process.Start(filename);
            }
            
            return filename;
        }

        /// <summary>
        /// Donne la possibilité d'afficher les propriétés de l'image.
        /// </summary>
        /// <param name="image"> La classe de l'image dont on veut afficher les propriétés </param>
        static void Parametre(MyImage image)
        {
            Console.WriteLine("\n\nVoulez afficher les caracteristiques de " + image.Nom + " ?\n1. Oui\n2. Non\n");
            int afficher = -1;
            do
            {
                Console.Write("> ");
                try { afficher = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("Entrez un numéro valide\n"); }
            } while (afficher != 1 && afficher != 2);

            Console.Clear();
            if (afficher == 1)
            {
                Console.WriteLine("Caractéristique de " + image.Nom + "\n");
                Console.WriteLine("Format : " + image.TypeImage + "\nTaille : " + image.TailleFichier + " bits\nTaille de l'Offset : " + image.TailleOffset + "\nLargeur : " + image.Largeur + " pixels\nHauteur : " + image.Hauteur + " pixels\n\n");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Affiche les différents traitements appliquable à l'image et laisse le choix à l'utilisateur
        /// </summary>
        /// <param name="image"> La classe de l'image sur laquelle on veut effectuer les traitements d'images </param>
        static void Traitement(MyImage image)
        {
            int numeroImage = -1;
            Console.WriteLine("Quelle opération voulez vous effectuer sur " + image.Nom +" :\n" +
                "\n1. Nuance de gris" +
                "\n2. Noir et blanc +" +
                "\n3. Image miroir" +
                "\n4. Rotation" +
                "\n5. Agrandir" +
                "\n6. Rétrécir" +
                "\n7. Appliquer un filtre" +
                "\n8. Miroir contenu" +
                "\n9. Insérer une image dans une autre" +
                "\n\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { numeroImage = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("Entrez un numéro valide\n"); }
            } while (numeroImage != 0 && numeroImage != 1 && numeroImage != 2 && numeroImage != 3 && numeroImage != 4 && numeroImage != 5 && numeroImage != 6 && numeroImage != 7 && numeroImage != 8 && numeroImage != 9);

            switch (numeroImage)
            {
                case 0:
                    Environment.Exit(0); // Si on ne veut pas aller jusqu'au bout
                    break;
                case 1:
                    image.enGris();
                    break;
                case 2:
                    image.NetB();
                    break;
                case 3:
                    image.miroir();
                    break;
                case 4:
                    image.rotation();
                    break;
                case 5:
                    image.agrandir();
                    break;
                case 6:
                    image.retrecir();
                    break;
                case 7:
                    Console.Clear();
                    image.filtre();
                    break;
                case 8:
                    image.miroirContenu();
                    break;
                case 9:
                    Console.Clear();
                    image.dissimulerImage();
                    break;
            }
        }

        static void Main(string[] args)
        {
            MyImage image = new MyImage(Selection());
            Parametre(image);
            Traitement(image);

            Console.ReadKey();
        }
    }
}

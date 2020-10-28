using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace EL_MERSHATI_Laith___Projet_Info_A2
{
    class MyImage
    {
        // * ATTRIBUTS *

        string nom;
        string typeImage;
        int tailleFichier;
        int tailleOffset;
        int largeur;
        int hauteur;
        int nb_Bit_Couleur;
        int offset; // index
        byte[] header = new byte[54];
        byte[] myfile; // Contient le header

        Pixel[,] image; // 


        
        // * CONSTRUCTEUR *

        /// <summary>
        /// Récupère les informations concernant l'image bit par bit qu'elle transmet dans le tableau myfile.
        /// Puis parcours myfile afin de récupérer les attributs
        /// </summary>
        /// <param name="filename"> On récupère l'image à partir de son nom </param>
        public MyImage(string filename)
        {
            From_Image_To_File(filename);
            nom = filename;

            // Récupérer le header : i : 0 à 53

            for (int i = 0; i < 54; i++)
            {
                header[i] = myfile[i];
            }

            // typeImage : i = 0 et 1
            for (offset = 0; offset < 2; offset++)
            {
                if (myfile[offset] == 65) { typeImage += "A"; }
                if (myfile[offset] == 66) { typeImage += "B"; }
                if (myfile[offset] == 67) { typeImage += "C"; }
                if (myfile[offset] == 73) { typeImage += "I"; }
                if (myfile[offset] == 77) { typeImage += "M"; }
                if (myfile[offset] == 80) { typeImage += "P"; }
                if (myfile[offset] == 84) { typeImage += "T"; }
            }

            // tailleFichier : i = 2 à 5
            offset = 2;
            tailleFichier = Convertir_Endian_To_Int(4);


                // INFORMATIONS CONCERNANT L'IMAGE

            // taille offset : i = 14 à 17
            offset = 14;
            tailleOffset = Convertir_Endian_To_Int(4);

            // largeur : i = 18 et 21
            offset = 18;
            largeur = Convertir_Endian_To_Int(4);

            // hauteur : i = 22 et 25
            offset = 22;
            hauteur = Convertir_Endian_To_Int(4);

            // nb_Bit_Couleur : i = 28 et 29
            offset = 28;
            nb_Bit_Couleur = Convertir_Endian_To_Int(2);
            

                // RECUPERER IMAGE -> transformation en matrice de pixels
            
            offset = 54;
            image = new Pixel[hauteur, largeur];

            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    image[i, j] = new Pixel(myfile[offset], myfile[offset + 1], myfile[offset + 2]);
                    offset += 3;
                }
            }
        }

        /// <summary>
        /// Vide dans le cas où on veut dessiner et donc on a rien à récupérer, tout à créer
        /// </summary>
        public MyImage()
        {
            
        }

        
        // * ACCESSEUR *

        public string Nom
        {
            get { return nom; }
        }

        public string TypeImage
        {
            get { return typeImage; }
        }

        public int TailleFichier
        {
            get { return tailleFichier; }
        }

        public int TailleOffset
        {
            get { return tailleOffset; }
        }

        public int Largeur
        {
            get { return largeur; }
        }

        public int Hauteur
        {
            get { return hauteur; }
        }

        public int NbBitCouleurs
        {
            get { return nb_Bit_Couleur; }
        }

        /// <summary>
        /// Pour dissimuler une image dans une autre il faut récupérer sa matrice de pixel
        /// </summary>
        public Pixel[,] Mat_Pixel
        {
            get { return image; }
        }

        
        // * METHODE *

        private void From_Image_To_File(string filename)
        {
            myfile = File.ReadAllBytes(filename);
        }

        private int Convertir_Endian_To_Int(int nbOctet, int fin = 0, int valeur = 0)
        {
            if(fin == nbOctet)
            {
                return valeur;
            }
            else
            {
                valeur += Convert.ToInt32(myfile[offset + fin] * Math.Pow(256, fin));
                fin++;
                return Convertir_Endian_To_Int(nbOctet, fin, valeur);
            }

        }

        private byte[] Convertir_Int_To_Endian(int nombre)  // Récupéré sur internet
        {
            byte[] endian = new byte[4];
            endian[0] = (byte)nombre;
            endian[1] = (byte)(((uint)nombre >> 8) & 0xFF);
            endian[2] = (byte)(((uint)nombre >> 16) & 0xFF);
            endian[3] = (byte)(((uint)nombre >> 24) & 0xFF);
            return endian;
        }


        
        // * METHODE TRAITEMENT IMAGE *

        /// <summary>
        /// Parcours la matrice de pixel et applique à chaque Pixel la méthode de classe permettant de créer la nuance de gris
        /// </summary>
        public void enGris()
        {
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    image[ligne, colonne].nuanceGris();
                }
            }
            CreerImage(0); // Rien à modifier dans le header
        }

        /// <summary>
        /// Parcours la matrice de pixel et applique à chaque Pixel la méthode de classe pour créer du noir ou blanc
        /// </summary>
        public void NetB()
        {
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    image[ligne, colonne].NoirEtBlanc();
                }
            }
            CreerImage(0); // Rien à modifier dans le header
        }
        
        /// <summary>
        /// Double la largeur de la matrice de pixel d'une certaine image à partir d'une matrice temporaire qu'on va remplir normalement pour la première moitié gauche puis de façon symétrique pour la seconde moitié
        /// La nouvelle largeur impliquera de modifier le header
        /// </summary>
        public void miroir() 
        {
            Pixel[,] imageMiroir = new Pixel[hauteur, 2 * largeur];

            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    imageMiroir[ligne, colonne] = image[ligne, colonne];
                    imageMiroir[ligne, imageMiroir.GetLength(1) - 1 - colonne] = image[ligne, colonne];
                }
            }
            image = imageMiroir;
            CreerImage(1); // Besoin de modifier la largeur dans le header
        }

        /// <summary>
        /// Applique une rotation de 90° de 1 à 3 fois en fonction de ce qui est demandé
        /// Si la rotation demandé est de 180° le header sera identique à celui de l'image avant rotation
        /// </summary>
        public void rotation()
        {
            int angle = 0;
            do
            {
                Console.Write("Choisissez un angle entre 90, 180 ou 270 : ");
                try { angle = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("\nLa rotation ne peut s'effectuer qu'à 90, 180 ou 270°"); }
            } while (angle != 90 && angle != 180 && angle != 270);
            
            if (angle == 90)
            {
                rotation90();
            }
            else if (angle == 180)
            {
                rotation90();
                rotation90();
            }
            else if (angle == 270)
            {
                rotation90();
                rotation90();
                rotation90();
            }

            if (angle == 180)
            {
                CreerImage(0); // Rien à modifier dans le header
            }
            else
            {
                CreerImage(2); // Besoin d'intervertir la largeur et hauteur dans le header
            }
        }

        private void rotation90()
        {
            Pixel[,] imageTemporaire = new Pixel[largeur, hauteur];
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    imageTemporaire[colonne, image.GetLength(0) - 1 - ligne] = image[ligne, colonne];
                }
            }
            image = imageTemporaire;
        }
        
        /// <summary>
        /// Créer une matrice temporaire plus grande en multipliant ses dimensions par x, remplit les cases vides à chaque index (grande image) par l'index/x (de l'image réel) ce qui reviendra à reprendre le même pixel plusieurs fois.
        /// L'image étant plus grande le tableau de byte devra être agrandi en conséquence. De plus, la largeur et hauteur dans le header devront être modifiées.
        /// </summary>
        public void agrandir()
        {
            int multiple = 0;
            bool possible;
            do
            {
                possible = true;
                Console.Write("Par combien voulez vous agrandir votre image : ");
                try { multiple = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("\nVeuillez entrer un entier\n"); possible = false; }
            } while (possible == false);
            
            Pixel[,] imageGrande = new Pixel[hauteur * multiple, largeur * multiple];
            for (int ligne = 0; ligne < imageGrande.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < imageGrande.GetLength(1); colonne++)
                {
                    imageGrande[ligne, colonne] = image[ligne / multiple, colonne / multiple];
                }
            }
            image = imageGrande;
            CreerImage(3, multiple); // Largeur et hauteur à modifier dans le header + tableau byte plus grand
        }

        /// <summary>
        /// Créer une matrice temporaire plus petite en divisant ses dimensions par x, on remplit les à chaque index (grande image) par l'index*x (de l'image réel) ce qui reviendra à perdre certains pixels.
        /// L'image étant plus petite le tableau de byte devra être réduit en conséquence. De plus, la largeur et hauteur dans le header devront être modifiées.
        /// </summary>
        public void retrecir()
        {
            int diviseur = 0;
            bool possible;
            do
            {
                possible = true;
                Console.Write("Par combien voulez vous rétrécir votre image : ");
                try { diviseur = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("\nVeuillez entrer un entier\n"); possible = false; }
            } while (possible == false || diviseur == 0); // division par 0

            Pixel[,] imagePetite = new Pixel[hauteur / diviseur, largeur / diviseur];
            for (int ligne = 0; ligne < imagePetite.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < imagePetite.GetLength(1); colonne++)
                {
                    imagePetite[ligne, colonne] = image[ligne * diviseur, colonne * diviseur];
                }
            }
            image = imagePetite;
            CreerImage(4, diviseur); // Largeur et hauteur à modifier dans le header + tableau byte plus petit
        }



        // * FILTRE *

        /// <summary>
        /// Définit la matrice de convolution en fonction du filtre demandé, par ailleurs si on veut appliquer un filtre flou il faudra alors effectuer une division par 9 qui sera effectuer dans la méthode convolution
        /// </summary>
        public void filtre()    
        {
            int numeroImage = -1;
            int[,] convolution = null;
            bool estFlou = false;

            Console.WriteLine("Choisissez le filtre que vous souhaitez appliquer.\n" +
                "\n1. Détection de contour" +
                "\n2. Détection de contour +" +
                "\n3. Détection de contour ++" +
                "\n4. Renforcement des bords" +
                "\n5. Flou" +
                "\n6. Repoussage" +
                "\n\n0. Sortir\n");
            do
            {
                Console.Write("> ");
                try { numeroImage = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("Entrez un numéro valide\n"); }
            } while (numeroImage != 0 && numeroImage != 1 && numeroImage != 2 && numeroImage != 3 && numeroImage != 4 && numeroImage != 5 && numeroImage != 6);

            switch (numeroImage) // On definit la matrice de convolution en fonction du filtre demandé 
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    int[,] contour1 = { { 1, 0, -1 }, { 0, 0, 0 }, { -1, 0, 1 } };
                    convolution = contour1;
                    break;
                case 2:
                    int[,] contour2 = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                    convolution = contour2;
                    break;
                case 3:
                    int[,] contour3 = { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };
                    convolution = contour3;
                    break;
                case 4:
                    int[,] renforcement = { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } };
                    convolution = renforcement;
                    break;
                case 5:
                    int[,] flou = { { 1 , 1, 1}, { 1 , 1 , 1}, { 1 , 1, 1} };
                    convolution = flou;
                    estFlou = true;
                    break;
                case 6:
                    int[,] repoussage = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
                    convolution = repoussage;
                    break;
            }
            Convolution(convolution, estFlou);
        }

        /// <summary>
        /// Effectue le calcul permettant d'obtenir les nouvelles valeurs des couleurs bleues, vertes et rouges en appliquant la matrice de convolution à l'image. 
        /// Pour appliquer la matrice de convolution à notre image, il faut accepter de sacrifier les pixels sur les extrémités ce qui va modifier les dimensions de l'image et par conséquent la taille du tableau de bits.
        /// Pour y remédier, les bords vide sont remplit par les pixels du nouveau bord.
        /// </summary>
        /// <param name="convolution"> matrice de convolution défini dans la méthode "filtre" en fonction de l'attente de l'usager </param>
        /// <param name="flou"> Si on veut appliquer un flou alors le bool sera "true" </param>
        private void Convolution(int[,] convolution, bool flou)
        {
            Pixel[,] imageTemporaire = new Pixel[hauteur, largeur];
            for (int i = 1; i < hauteur - 1; i ++)
            {
                for (int j = 1; j < largeur - 1; j++)
                {
                    int valueBlue = image[i - 1, j - 1].Blue * convolution[0, 0] + image[i - 1, j].Blue * convolution[0, 1] + image[i - 1, j + 1].Blue * convolution[0, 2] +
                                    image[i, j - 1].Blue * convolution[1, 0] + image[i, j].Blue * convolution[1, 1] + image[i, j + 1].Blue * convolution[1, 2] +
                                    image[i + 1, j - 1].Blue * convolution[2, 0] + image[i + 1, j].Blue * convolution[2, 1] + image[i + 1, j + 1].Blue * convolution[2, 2];
                    

                    int valueGreen = image[i - 1, j - 1].Green * convolution[0, 0] + image[i - 1, j].Green * convolution[0, 1] + image[i - 1, j + 1].Green * convolution[0, 2] +
                                     image[i, j - 1].Green * convolution[1, 0] + image[i, j].Green * convolution[1, 1] + image[i, j + 1].Green * convolution[1, 2] +
                                     image[i + 1, j - 1].Green * convolution[2, 0] + image[i + 1, j].Green * convolution[2, 1] + image[i + 1, j + 1].Green * convolution[2, 2];
                    

                    int valueRed =  image[i - 1, j - 1].Red * convolution[0, 0] + image[i - 1, j].Red * convolution[0, 1] + image[i - 1, j + 1].Red * convolution[0, 2] +
                                    image[i, j - 1].Red * convolution[1, 0] + image[i, j].Red * convolution[1, 1] + image[i, j + 1].Red * convolution[1, 2] +
                                    image[i + 1, j - 1].Red * convolution[2, 0] + image[i + 1, j].Red * convolution[2, 1] + image[i + 1, j + 1].Red * convolution[2, 2];

                    if(flou)
                    {
                        valueBlue = valueBlue / 9;
                        valueGreen = valueGreen / 9;
                        valueRed = valueRed / 9;
                    }

                    if (valueBlue < 0) { valueBlue = 0; }
                    if (valueBlue > 255) { valueBlue = 255; }

                    if (valueGreen < 0) { valueGreen = 0; }
                    if (valueGreen > 255) { valueGreen = 255; }

                    if (valueRed < 0) { valueRed = 0; }
                    if (valueRed > 255) { valueRed = 255; }

                    imageTemporaire[i, j] = new Pixel(valueBlue, valueGreen, valueRed);
                }
            }

            imageTemporaire[0, 0] = imageTemporaire[1, 1]; // coin supérieur gauche
            imageTemporaire[0, largeur - 1] = imageTemporaire[1, largeur - 2]; // coin supérieur droit
            imageTemporaire[hauteur - 1, 0] = imageTemporaire[hauteur - 2, 1]; // coin inférieur gauche
            imageTemporaire[hauteur - 1, largeur - 1] = imageTemporaire[hauteur - 2, largeur - 2]; // coin inférieur droit

            for (int ligne = 1; ligne < hauteur - 1; ligne++) 
            {
                imageTemporaire[ligne, 0] = imageTemporaire[ligne, 1]; // bord gauche
                imageTemporaire[ligne, largeur - 1] = imageTemporaire[ligne, largeur - 2]; // bord droit
            }

            for (int colonne = 1; colonne < largeur - 1; colonne++)
            {
                imageTemporaire[0, colonne] = imageTemporaire[1, colonne]; // bord supérieur
                imageTemporaire[hauteur - 1, colonne] = imageTemporaire[hauteur - 2, colonne]; // inférieur
            }

            image = imageTemporaire;
            CreerImage(0); // Rien à modifier dans le header
        }


        // * AUTRES *

        /// <summary>
        /// Dissimule une image au centre d'une autre (possèdant de plus grandes dimensions sinon l'image ne sera plus vraiment cachée...).
        /// </summary>
        public void dissimulerImage()
        {
            Pixel[,] imageTemporaire = new Pixel[hauteur, largeur];
            MyImage aDissimuler = null;
            do
            {
                aDissimuler = new MyImage(Selection2());
            } while (aDissimuler.Hauteur > Hauteur && aDissimuler.Largeur > Largeur);
            Pixel[,] image2 = aDissimuler.Mat_Pixel; // Récupérer la matrice de pixels de l'image qu'on veut dissimuler

            int ajustHauteur = (hauteur - aDissimuler.Hauteur) / 2; 
            int ajustLargeur = (largeur - aDissimuler.Largeur) / 2;

            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    if (ligne >= ajustHauteur && ligne < (hauteur - ajustHauteur) && colonne >= ajustLargeur && colonne < (largeur - ajustLargeur))
                    {
                        int[] binaire1_Blue = Convertir_Int_ToByte(image[ligne, colonne].Blue); // On récupère l'octet associé à la couleur bleue à cet emplacement
                        int[] binaire2_Blue = Convertir_Int_ToByte(image2[ligne - ajustHauteur, colonne - ajustLargeur].Blue); // On récupère l'octet associé à la couleur bleue de l'image à dissimuler
                        int[] nouveauBinaire_Blue = new int[8];
                        for (int i = 0; i < 8; i++)
                        {
                            if (i <= 3)
                            {
                                nouveauBinaire_Blue[i] = binaire2_Blue[4 + i]; // Les 4 premiers bits du nouvelle octet correspondent aux 4 derniers de l'image à dissimuler
                            }
                            else
                            {
                                nouveauBinaire_Blue[i] = binaire1_Blue[i]; // Les 4 derniers bits du nouvelle octet correspondent aux 4 premiers de l'image dans laquelle on veut dissimuler
                            }
                        }
                        int valueBlue = Convert.ToInt32(Convertir_Byte_ToInt(nouveauBinaire_Blue));

                        int[] binaire1_Green = Convertir_Int_ToByte(image[ligne, colonne].Green);
                        int[] binaire2_Green = Convertir_Int_ToByte(image2[ligne - ajustHauteur, colonne - ajustLargeur].Green);
                        int[] nouveauBinaire_Green = new int[8];
                        for (int i = 0; i < 8; i++)
                        {
                            if (i <= 3)
                            {
                                nouveauBinaire_Green[i] = binaire2_Green[4 + i];
                            }
                            else
                            {
                                nouveauBinaire_Green[i] = binaire1_Green[i];
                            }
                        }
                        int valueGreen = Convert.ToInt32(Convertir_Byte_ToInt(nouveauBinaire_Green));

                        int[] binaire1_Red = Convertir_Int_ToByte(image[ligne, colonne].Red);
                        int[] binaire2_Red = Convertir_Int_ToByte(image2[ligne - ajustHauteur, colonne - ajustLargeur].Red);
                        int[] nouveauBinaire_Red = new int[8];
                        for (int i = 0; i < 8; i++)
                        {
                            if (i <= 3)
                            {
                                nouveauBinaire_Red[i] = binaire2_Red[4 + i];
                            }
                            else
                            {
                                nouveauBinaire_Red[i] = binaire1_Red[i];
                            }
                        }
                        int valueRed = Convertir_Byte_ToInt(nouveauBinaire_Red);

                        imageTemporaire[ligne, colonne] = new Pixel(valueBlue, valueGreen, valueRed);
                    }
                    else // Si on ne se situe pas dans la zone où l'on veut dissimuler l'image
                    {
                        imageTemporaire[ligne, colonne] = image[ligne, colonne]; 
                    }
                }
            }
            image = imageTemporaire;
            CreerImage(0); // Rien à modifier dans le header
        }

        private string Selection2()
        {
            Console.WriteLine("Choisissez une image à dissimuler qui soit plus petite que celle que vous aviez sélectionner" +
                "\n" +
                "\n0. Entrer un nom de fichier" +
                "\n1. Coco" +
                "\n2. Lac en montagne" +
                "\n3. Lena" +
                "\n4. Test\n " +
                "\n9. Sortir\n");
            int numeroImage = -1;
            do
            {
                Console.Write("> ");
                try { numeroImage = Convert.ToInt32(Console.ReadLine()); }
                catch { Console.Write("Entrez un numéro valide\n"); }
            } while (numeroImage != 0 && numeroImage != 1 && numeroImage != 2 && numeroImage != 3 && numeroImage != 4 && numeroImage != 9);

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
                        try { existence = File.ReadAllBytes(filename); }
                        catch { Console.WriteLine("Saisissez un nom de fichier existant."); }
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
                case 9:
                    Environment.Exit(0);
                    break;
            }
            return filename;
        }

        /// <summary>
        /// Convertit un entier en un octet (tableau d'entier de dimension 8)
        /// </summary>
        /// <param name="nombre"> nombre à convertir </param>
        /// <returns> tableau d'entier de dimension 8 </returns>
        public int[] Convertir_Int_ToByte(int nombre)
        {
            int reste;
            int[] octet = new int [8];
            int i = 0;
            while(nombre != 0 && i < 8)
            {
                if (nombre == 0)
                {
                    octet[i] = 0;
                }
                reste = nombre % 2;
                octet[i] = reste;
                nombre = (nombre - reste) / 2;
                i++;
            }
            return octet;
        }

        /// <summary>
        /// Convertit un octet (tableau d'entier de dimension 8) en un entier
        /// </summary>
        /// <param name="octet"> octet à convertir en entier </param>
        /// <returns> retourne un entier </returns>
        public int Convertir_Byte_ToInt(int[] octet)
        {
            int nombre = 0;
            for (int i = 0; i < 8; i++)
            {
                nombre += Convert.ToInt32(octet[i] * Math.Pow(2, i));
            }
            return nombre;
        }

        // Fractale de Mandelbrot

        /// <summary>
        /// Dessine la fractale de Mandelbrot en noir et bleue
        /// </summary>
        public void fractale()
        {
            // L'ensemble de Mandelbrot est toujours compris entre -2.1 et 0.6 sur l'axe des abscisse et entre -1.2 et 1.2 sur l'axe des ordonnées.
            double x1 = -2.1; // limite gauche
            double x2 = 0.6; // limite droite
            double y1 = -1.2; // limite hausse
            double y2 = 1.2; // limite basse
            int zoom = 100;
            int iterationMax = 50;

            int imageX = Convert.ToInt32((x2 - x1) * zoom);
            largeur = imageX;
            int imageY = Convert.ToInt32((y2 - y1) * zoom);
            hauteur = imageY;

            Pixel[,] fractale = new Pixel[imageX, imageY];

            for (int x = 0; x < imageX; x++)
            {
                for (int y = 0; y < imageY; y++)
                {
                    double c_r = (x / zoom) + x1;
                    double c_i = (y / zoom) + y1;
                    double z_r = 0; // partie réelle
                    double z_i = 0; // partie imaginaire
                    int i = 0;

                    do
                    {
                        double temporaire = z_r;
                        z_r = z_r * z_r - z_i * z_i + c_r;
                        z_i = 2 * z_i * temporaire + c_i;
                        i++;
                    }
                    while ((z_r * z_r + z_i * z_i) < 4 && i < iterationMax); // évite de calculer le module de z (appliquer des racines), donc on compare à 4 au lieu de 2

                    if (i == iterationMax)
                    {
                        fractale[x, y] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        fractale[x, y] = new Pixel(0, 0, (i * 255) / iterationMax);
                    }
                }
            }

            image = fractale;
            CreerImage(5); // Tout le header est à faire
        }

        // Ma Création --> Miroir contenu

        /// <summary>
        /// Créer un effet miroir au sein de la même image, sans toucher à ses dimensions
        /// </summary>
        public void miroirContenu()
        {
            Pixel[,] imageTemporaire = new Pixel[hauteur, largeur];
            int alterner = 0; // alternera entre la valeur 1 ou 2

            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                alterner = 0;
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    alterner = (alterner % 2) + 1; // Une fois sur deux selon les colonnes il faudra récupérer le pixel symmétriques
                    if (alterner == 1)
                    {
                        imageTemporaire[ligne, colonne] = image[ligne, colonne];
                    }
                    if (alterner == 2)
                    {
                        imageTemporaire[ligne, colonne] = image[ligne, largeur - 1 - colonne];
                    }
                }
            }
            image = imageTemporaire;
            CreerImage(0); // Rien à modifier dans le header
        }


        // Créer la nouvelle image

        /// <summary>
        /// Effectue dans un premier temps les modications nécessaire dans le header en fonction du traitement effectué, puis remplit entièrement le nouveau tableau de byte en reprenant le header pour les 54 premiers bits, puis les valeurs des couleurs bleue, vert et rouge pour le reste du tableau.
        /// Finit par écrire l'image, la créer puis l'afficher.
        /// </summary>
        /// <param name="operation"> modification à apporter dans le header selon le traitement à effectuer </param>
        /// <param name="multiple"> agrandissement ou rétrécissement de l'image </param>
        private void CreerImage(int operation, int multiple = 0)
        {
            byte[] creerFile = null; // Même principe que le tableau de byte myfile
            byte[] temporaire = new byte[4];

            switch (operation)
            {
                case 0: // nuance gris ; noir et blanc ; rotation 180 ; convolution ; insérer image dans une autre
                    creerFile = new byte[tailleFichier];
                    break;
                case 1: // Miroir
                    int nouvelLargeur = 2 * largeur;
                    temporaire = Convertir_Int_To_Endian(nouvelLargeur);
                    int nouvelTaille = (tailleFichier - 54) * 2 + 54;
                    creerFile = new byte[nouvelTaille];
                    for (int i = 0; i <= 3; i++)
                    {
                        header[18 + i] = temporaire[i];
                    }
                    break;
                case 2: // Rotation 90° ou 270°
                    creerFile = new byte[tailleFichier];
                    for (int i = 0; i <= 3; i++)
                    {
                    // Permuter dans le header, la largeur et hauteur
                        temporaire[i] = header[22 + i];
                        header[22 + i] = header[18 + i];
                        header[18 + i] = temporaire[i];
                    }
                    break;
                case 3: // Agrandir
                    creerFile = new byte[((largeur * multiple) * (hauteur * multiple) * 3) + 54]; // Multiplier par 3 pour les 3 couleurs primaires
                    temporaire = Convertir_Int_To_Endian(largeur * multiple);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[18 + i] = temporaire[i];
                    }
                    temporaire = Convertir_Int_To_Endian(hauteur * multiple);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[22 + i] = temporaire[i];
                    }
                    break;
                case 4: // Rétrécir
                    creerFile = new byte[((largeur / multiple) * (hauteur / multiple) * 3) + 54]; // Multiplier par 3 pour les 3 couleurs primaires
                    temporaire = Convertir_Int_To_Endian(largeur / multiple);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[18 + i] = temporaire[i];
                    }
                    temporaire = Convertir_Int_To_Endian(hauteur / multiple);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[22 + i] = temporaire[i];
                    }
                    break;
                case 5: // Fractale
                    tailleFichier = (largeur * hauteur * 3) + 54;
                    creerFile = new byte[tailleFichier];

                    header[0] = 66; // B
                    header[1] = 77; // M

                    temporaire = Convertir_Int_To_Endian(tailleFichier);
                    for (int i = 0; i <= 3; i++) // taille fichier
                    {
                        header[2 + i] = temporaire[i];
                    }

                    for (int i = 0; i <= 3; i++)
                    {
                        header[6 + i] = 0;
                    }

                    header[10] = 54; // taille header

                    for (int i = 0; i <= 2; i++)
                    {
                        header[11 + i] = 0;
                    }

                    header[14] = 40; // taille offset

                    for (int i = 0; i <= 2; i++)
                    {
                        header[15 + i] = 0; // taille offset
                    }

                    temporaire = Convertir_Int_To_Endian(largeur);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[18 + i] = temporaire[i]; // largeur
                    }

                    temporaire = Convertir_Int_To_Endian(hauteur);
                    for (int i = 0; i <= 3; i++)
                    {
                        header[22 + i] = temporaire[i]; // hauteur
                    }

                    header[26] = 1;
                    header[27] = 0;
                    header[28] = 24; // nb bit par couleur + (i = 29)

                    for (int i = 0; i <= 24; i++)
                    {
                        header[29 + i] = 0;
                    }
                    break;
            }

            for (int offset = 0; offset < 54; offset++)
            {
                creerFile[offset] = header[offset];
            }

            int ajustOffset = 0;

                for (int ligne = 0; ligne < image.GetLength(0); ligne++)
                {
                    for (int colonne = 0; colonne < image.GetLength(1); colonne++)
                    {
                        creerFile[54 + ajustOffset] = Convert.ToByte(image[ligne, colonne].Blue);
                        creerFile[55 + ajustOffset] = Convert.ToByte(image[ligne, colonne].Green);
                        creerFile[56 + ajustOffset] = Convert.ToByte(image[ligne, colonne].Red);
                        ajustOffset += 3;
                    }
                }
    
            // Afficher l'image avant de la créer ? proposer de lui donner un nom ?
            
            File.WriteAllBytes("nouvel_image.bmp",creerFile);
            Process.Start("nouvel_image.bmp");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL_MERSHATI_Laith___Projet_Info_A2
{
    class Pixel
    {
        // Attributs

        int blue;
        int green;
        int red;


        // Constructeurs

        public Pixel(int B, int G, int R)
        {
            if (R >= 0 && R < 256 && B >= 0 && B < 256 && G >= 0 && G < 256)
            {
                blue = B;
                green = G;
                red = R;
            }
        }

        public Pixel(Pixel pixel)
        {
            blue = pixel.Blue;
            green = pixel.Green;
            red = pixel.Red;
        }


        // Accesseurs
                
        public int Blue
        {
            get { return blue; }
            set { blue = value; }
        }
        public int Green
        {
            get { return green; }
            set { green = value; }
        }
        public int Red
        {
            get { return red; }
            set { red = value; }
        }


        // Méthodes


        public void nuanceGris()
        {
            int gris = (this.red + this.blue + this.green) / 3;
            this.red = gris;
            this.blue = gris;
            this.green = gris;
        }

        public void NoirEtBlanc()
        {
            int gris = (this.red + this.blue + this.green) / 3;
            if (gris <= 128)
            {
                this.red = 0;
                this.blue = 0;
                this.green = 0;
            }
            else
            {
                this.red = 255;
                this.blue = 255;
                this.green = 255;
            }
        }
    }
}

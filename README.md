# Algorithme de traitements d'images

## Présentation

L’idée est de créer un outil qui lit une image dans un format donné (bitmap ou csv), traite cette
image (agrandir, rétrécir…) et sauvegarde l’image dans un fichier de sortie **différent** de celui donné en entrée (toujours format Bitmap ou csv).

* Lire et écrire une image (à partir d’un format .bmp ou .csv)

* Traiter une image :
    * Passage d’une photo couleur à une photo en nuances de gris ET en noir et blanc
    * Agrandir/Rétrécir une image
    * Rotation (90,180 ou 270°)
    * Effet Miroir

* Appliquer un filtre (**matrice de convolution**) :
    * Détection de contour
    * Renforcement des bords
    * Flou
    * Repoussage

* Coder et Décoder une image dans une image

* Une création, autre que ce qui est proposé ci-dessus


## Structure du code

Pour se faire, il faut d’abord lire tous les bits du fichier et les stocker dans un autre tableau de bit qu’on utilisera pour récupérer les infos concernant l’image un par un. Les pixels sont récupérés par trois (bleue, vert et rouge) et sont associés à un emplacement précis dans une matrice de pixels.

Ainsi, j’ai créé **2 classes** en plus du Main :

### Classe - Pixel

Un pixel est constitué de 3 composants qui sont les 3 couleurs primaires et qui correspondent donc à mes **attributs** : des entiers compris entre 0 et 255 pour le bleue, le vert et le rouge qu’on obtient à travers le **constructeur** en entrant les valeurs, ou bien en copiant un autre pixel.

Ces attributs sont accessibles en **lecture** (pour récupérer les valeurs et les traiter selon les besoins), et en **écriture** (effectuer les modifications nécessaires).

Les *méthodes* permettent de modifier la couleur envoyée par le pixel en gris (selon la moyenne de la somme des 3 couleurs primaires) ou bien en noir/blanc en modifiant les valeurs des 3 attributs à l’identique.

### Classe - MyImage

Cette classe est assez imposante car c’est là que la récupération de l’image va se faire et le transfert dans une **matrice de la classe Pixel**, mais aussi les traitements d’images et la restitution dans un fichier de sortie différent de celui donné en entrée.

Les attributs sont donc les informations propres à l’image tel que le nom, type d’image, taille du fichier, dimensions… qu’on affichera mais aussi sur lesquels on apportera des modifications ce qui implique de donner accès en **lecture** mais **pas en écriture** car on ne les traites pas en dehors de la classe.

On retrouve aussi un tableau contenant tous les bits et un autre contenant seulement le header, mais aussi une matrice de la classe Pixel qui contiendra les valeurs de RVB.

Un **constructeur** récupère donc toutes ses informations et les organises, un autre est vide car il est utile dans le cas de la création d’image (rien à récupérer).

Les méthodes de traitement d’image sont **publiques** car elles seront lancées depuis le Main. Par ailleurs, toutes ces méthodes renvoient vers une méthode **private** « CreerImage » qui, à travers les attributs modifiés reconstituera le header dans un premier temps (selon les modifications nécessaires qu’on aura entrées), puis tout le tableau de bits en parcourant la matrice de pixel, avant d’écrire le fichier grâce au tableau complet.
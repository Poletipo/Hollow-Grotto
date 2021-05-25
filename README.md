# Hollow Grotto
## Sommaire
Le projet est un jeu vidéo créer dans Unity, explorant des environnements générés aléatoirement dont le terrain est destructible, permettant des tunnels. La création de l'environnement est réalisé en utilisant l’algorithme Marching Cubes. 

Vous incarnez un mécanicien dans une caverne qui a pour mission de retrouver des robot miniers brisés, et de les réparer. Pour y arriver vous pouvez creuser les parois grace à votre combinaison robotique et atteindre l'objectif.

## Installation
- Télécharger et installer Unity Hub: https://unity3d.com/fr/get-unity/download 
    - Dans Unity Hub, dans l'onglet "Installs", Cliquer sur "Add".
    - Sélectionner la version recommendée et cliquer "Next"
    - Cocher "Microsoft Visual Studio Community 2019" et cliquer "Next"
    - Accepter les termes et conditions et cliquer "Done"
    - L'installation devrait commencer et finir après un certain temps
- Dans Unity Hub, dans l'onglet "Projects", cliquer sur "ADD"
    - Ajouter le dossier "dev" du projet synthèse
- Dans Unity Hub, dans l'onglet "Projects", assurez vous que le projet "dev" ait la version téléchargée, et cliquer sur le projet.
    - Si un avertissement s'affiche pour un upgrade, cliquer "Confirm"

## Utilisation
### Pour jouer
- Première méthode:
    - Ouvrir le projet Unity
    - Une fois Unity ouvert, pour partir le jeu, cliquer sur le bouton Play au dessus du viewport (la flèche triangle).
    - Pour quitter, appuyer sur la touche "Escape", et réapuyer sur le bouton Play au dessus du viewport (la flèche triangle).
- Deuxième méthode:
    - Dans le dossier de base, il y a un dossier appellé "HollowGrottoBuild".
    - Dans ce dossier, double clic sur l'executable "Hollow Grotto.exe"

## Références
- Table de marching Cubes : http://paulbourke.net/geometry/polygonise/
- Inspiration original pour marching cubes: https://github.com/SebLague/Marching-Cubes
- Shader triplanar: https://github.com/keijiro/StandardTriplanar/blob/master/Assets/Triplanar/Shaders/StandardTriplanar.shader
- Inspiration pour le mouvement: https://catlikecoding.com/unity/tutorials/movement/
- Tutoriel pour le grappin: https://github.com/DaniDevy/FPS_Movement_Rigidbody/blob/master/GrapplingGun.cs
- Noise: https://github.com/keijiro/NoiseShader

## Remerciements
Merci à tous mes amis pour avoir tester le jeu, même s'il était plate à jouer.

## Licence
Copyright <2021> <Jimmy Labrecque>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

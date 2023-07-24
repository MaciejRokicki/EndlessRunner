# EndlessRunner

#### Założenia gry
Prosta gry typu endless runner. Polega na omijaniu przeszkód i pozostaniu jak najdłużej w ruchu. Gracz cały czas porusza się do przodu, za pomocą myszki może zmieniać kierunek ruchu (prawo/lewo) oraz spoglądać (góra/dół) oraz skakać za pomocą spacji. Prędkość ruchu gracza z czasem wzrasta. Teren, po którym gracz się porusza generowany jest przy pomocy szumu Perlin'a, dodatkowo na tym terenie generowane są w sposób losowy przeszkody, różnej rzadkości. Występują 3 poziomy rzadkości - łatwe, średnie i trudne, które określają szanse na wygenerowanie przeszkody z danego poziomu rzadkości. W grze występują również modyfikatory - zielone bloki przyśpieszające gracza oraz fioletowe, które go spowolniają.

#### Aspekt techniczny
W projekcie zastosowano następujące wzorce projektowe: singleton (do tworzenia managerów, np. GameManager), obserwator (w postaci c# events, np. do informowania o czasie), strategia (do wykonania określonej czynności po wejściu na dany obszar).
Do implementacji systemu poruszania się gracza wykorzystano CharacterController, który jest używany w PlayerController. W celu nadania nieregularnego kształtu terenu, po którym porusza się gracz użyto szumu Perlin'a.
Z zastosowanych elementów silnika użyto: ScriptableObjects (do tworzenia nowych przeszkód), URP, skrypty edytora, do lepszej organizacji elementów skryptu w inspektorze.

### Windows build
https://github.com/MaciejRokicki/EndlessRunner/releases/download/v1.0.0/EndlessRunner.zip

### Screenshots
![](/../master/Media/1.png)
![](/../master/Media/2.png)
![](/../master/Media/3.png)
![](/../master/Media/4.png)
![](/../master/Media/5.png)

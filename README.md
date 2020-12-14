# PEC3 - Juego de Plataformas 3D

Este proyecto consiste en un juego en tercera persona dónde el jugador tiene que sobrevivir en una ciudad llena de zombis. Estos zombis van deambulando por la ciudad y cuando sienten que el jugador está cerca van hacia él.
El jugador tiene una pistola para ir matando a los zombis y un temporizador para ver cuanto tiempo ha sobrevivido y poder superar su tiempo en la siguiente partida.


## Cómo jugar

Para poder jugar a este juego se tiene que descargar este repositorio y abrirlo con la versión 2019.4.10f1 de Unity.
Para mover al jugador se puede mover con las teclas [W, A, S, D] y direccionando la cámara con el ratón. Se tiene que tener en cuenta que el jugador dispara hacia donde está mirando el jugador y no la cámara.
Luego se puede saltar en la tecla [Espacio] y se puede recargar el arma con la [R].


## Desarrollo del juego

### Construcción del terreno
El terreno ha sido creado con la herramienta que proporciona Unity para crear 'terrains' y se ha construido de una manera que el jugador no pueda caer del mapa con montañas rodeando la ciudad.
La ciudad se ha construido con el Asset 'Toon Gas Station' utilizando la ciudad de prueba que se proporciona. Se ha incrementado el salto del jugador para poder llegar a todos los sitios elevados.

### Arma
El jugador dispone de un arma para poder matar a los zombis. Esta arma tiene una munición limitada y unos cartuchos limitados por lo que se tiene que ir recargando.

### Vida e ítems
El jugador tiene una vida y los zombis le irán quitando bastante vida al jugador si se acercan demasiado. Cuando el jugador consigue matar a un zombi, este puede dejar caer un item de vida, que sumará vida al jugador cuando lo recoja, y un item de munición para sumar la munición máxima que tiene el jugador.

### HUD
Por pantalla se puede ver la vida que le queda al jugador, la munición que le queda en el cartucho, la munición total y un temporizador que va mirando cuanto tiempo lleva en la partida.

### Enemigos
Los enemigos son zombis policías que van caminando hacia puntos aleatorios de la ciudad. Cuando el jugador se acerca demasiado a un zombi, este va hacia él rápidamente y si consigue aproximarse le restará vida al jugador haciendo la animación de atacar.

## Vídeo
[https://youtu.be/X3HypgFxTwU](https://youtu.be/X3HypgFxTwU)
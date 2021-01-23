# tetris-ai

## Introduction
Tetris looked like the perfect game to test a powerfull machine learning ai. The goal of this project is to test the vaibility of machine learning in a realy basic tertis game.
The project will consist of 3 main parts: making the tetris game, researching how en what kind of machine learning to use, implementing the machine learning into the game.

# The making of my tetris
First thing on the todo list is decide to use a game enige or to make it completely from scratch. I decided that using unity is the best option because bevause of the ease of use and I can finish the game way faster in unity. To keep it simple I just made block prefabs of al the tetris block then made a block script that lowers them down. once they reach the bottom they get added to a static grid and disabled. if a block moves down onto a grid it also get added to the grid and disabled. Then I made a spawner script that spawnes a block everytime a block gets disabled and 1 time at the start of the game. thats it the basic game is completed.

 ![Alt Text](/ReadMeImages/)
 
# Reasearching machine learning
After a bit of research I found a document [Here](https://melax.github.io/tetris/tetris.html) about using reinforcement learning for tetris. After a few hours trying to understand it I still couldn't anyting written there so I kept looking. 

https://github.com/Unity-Technologies/ml-agents
https://melax.github.io/tetris/tetris.html

md python-envs
python-envs\sample-env\Scripts\activate
mlagents-learn.exe C:\Users\Bodek\gitshit\ml-agents\config\ppo\Tetris.yaml --run-id=try3

# tetris-ai

# Introduction
Tetris looked like the perfect game to test a powerfull machine learning ai. The goal of this project is to test the vaibility of machine learning in a realy basic tertis game.
The project will consist of 3 main parts: making the tetris game, researching how en what kind of machine learning to use, implementing the machine learning into the game.

# Design/implementation
## The making of my tetris game
First thing on the todo list is decide to use a game engine or to make it completely from scratch. I decided that using unity is the best option because of the ease of use and I can finish the game way faster in unity. To keep it simple I just made block prefabs of al the tetris blocks. Then I made a block script that lowers them down. once they reach the bottom they get added to a static grid and disabled. If a block moves down onto the grid grid it also get added to the grid and disabled. Then I made a spawner script that gets called by the block and spawns a new one every time one gets disabled and 1 time at the start of the game. Finaly every time a block gets placed a check is run to see if there is a row. If there is a row it is deleted and all block above it are moved down.

## Reasearching machine learning
After a bit of research I found a document [Here](https://melax.github.io/tetris/tetris.html) about using reinforcement learning for tetris. After a few hours trying to understand it I still couldn't grasp anything usefull written there so I kept looking. Then I found a unity package [Here](https://github.com/Unity-Technologies/ml-agents) that made using machine learning easy to use in unity. ml-agents can use ppo of sac I decided that ppo........

## implementing machine learning
To start implementing the ai. I had to refactor some parts of the tetris code. As you might have noticed I forgot to make a gameover state. I also had to give some information to the the ml-agent and reward the agent for good moves. To fix this I added a game manager script. The manager script now holds the grid of blocks and the grid itself is no longer static to allow more than one game to run at once (for faster learning). I added a score system to the game manager script. The manager gives all the needed information to the ml agent.

![](/Images/AIObservatiobs.png)  

This includes the grid of blockes and the current block. The game manager rewards the ml agent for "good moves" the bot gains. A good move is decided by the movescore. The movescore gets increased by 10 if the placed block does not increase the total height of the grid. The movescore also gets increased by 10 if the placed block does not create new holes(a empty space with a block above it).
![](/Images/blockReward.png) 

The ai is also rewarded 1000 points for removing a row.

![](/Images/AIrewardRow.png).

# Conclusion/Future work
The result is disappointing. 

 ![Result](/Images/)
 
 I think there are 2 main reasons the ai is not learning correctly. Reason 1 I think the reward system. The ai gets rewarded too often for random moves and doesn't get rewarded enough for good moves. The way to fix this is to add extra ways to reward the ai. Maybe adding reward for good moves with less inputs. The other porblem has to dowith my implementation that of tetris. There is a maximum amount of inputs/sec because of this the ai gives inputs that do not get registered. This ai to learn incorrectly. The po
 
https://melax.github.io/tetris/tetris.html

md python-envs
python-envs\sample-env\Scripts\activate
mlagents-learn.exe C:\Users\Bodek\gitshit\ml-agents\config\ppo\Tetris.yaml --run-id=try3
